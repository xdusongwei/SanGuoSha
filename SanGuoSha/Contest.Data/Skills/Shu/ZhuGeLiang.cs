using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Data
{
    internal class SkillKongCheng : SkillBase
    {
        public SkillKongCheng()
            : base("空城", SkillEnabled.Passive, false)
        {

        }

        public override bool EffectFeasible(Card[] aCards ,Card.Effect aEffect, ChiefBase aTarget, bool aFeasible, GlobalData aData)
        {
            switch (aEffect)
            {
                case Card.Effect.Sha:
                case  Card.Effect.JueDou:
                    if (aData.Game.GamePlayers[aTarget].Hands.Count == 0)
                        return false;
                    break;
            }
            return aFeasible;
        }
    }

    internal class SkillGuanXing : SkillBase
    {
        public SkillGuanXing()
            : base("观星", SkillEnabled.Passive, false)
        {

        }

        private readonly string Top = "观星顶";
        private readonly string Bottom = "观星底";
        private readonly string Total = "观星全部";
        public override void OnCreate(ChiefBase aChief)
        {
            aChief.SlotsBuffer.Slots.Add(new Slot(Top, false, false));
            aChief.SlotsBuffer.Slots.Add(new Slot(Bottom, false, false));
            aChief.SlotsBuffer.Slots.Add(new Slot(Total, false, false));
        }

        public override void BeforeTurnStart(ChiefBase aChief, GlobalData aData)
        {
            aChief.SlotsBuffer[Top].Cards.Clear();
            aChief.SlotsBuffer[Bottom].Cards.Clear();
            aChief.SlotsBuffer[Total].Cards.Clear();
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aChief, this));
            MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForYN(aChief);
            if (res.YN)
            {
                int alive = aData.Game.GamePlayers.PeoplealiveCount;
                alive = alive > 5 ? 5 : alive;
                Card[] cards = aData.Game.CardsHeap.Pop(alive);
                aChief.SlotsBuffer[Total].Cards.AddRange(cards);
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                aData.Game.AsynchronousCore.SendPrivateMessageWithOpenMessage(aChief,
                    new Beaver("askfor.guanxing.cards" , aChief.ChiefName , Card.Cards2Beaver("cards" ,cards)).ToString().ToString(),
                    new Beaver("askfor.guanxing.cards" , aChief.ChiefName).ToString(),
                    aData.Game.GamePlayers);
                    //new XElement("askfor.guanxing.cards", new XElement("target", aChief.ChiefName), Card.Cards2XML("cards", cards)),
                    //new XElement("askfor.guanxing.cards", new XElement("target", aChief.ChiefName)),
                    //aData.Game.GamePlayers
                    //);
                res = aData.Game.AsynchronousCore.AskForCards(aChief, MessageCore.AskForEnum.SlotCards, aChief);
                List<Card> target = aChief.SlotsBuffer[Top].Cards;
                foreach(Card c in  res.Cards)
                    if (!cards.Contains(c))
                    {
                        if (c.ID == 0)
                        {
                            target = aChief.SlotsBuffer[Bottom].Cards;
                        }
                        else
                        {
                            aChief.SlotsBuffer[Top].Cards.Clear();
                            aChief.SlotsBuffer[Bottom].Cards.AddRange(cards);
                            break;
                        }
                    }
                    else
                    {
                        target.Add(c);
                    }
                if ((aChief.SlotsBuffer[Top].Cards.Count + aChief.SlotsBuffer[Bottom].Cards.Count) != cards.Count())
                {
                    aChief.SlotsBuffer[Top].Cards.Clear();
                    aChief.SlotsBuffer[Bottom].Cards.AddRange(cards);
                }

                aData.Game.AsynchronousCore.SendMessage(
                    new Beaver("guanxing.info", aChief.SlotsBuffer[Top].Cards.Count.ToString() , aChief.SlotsBuffer[Bottom].Cards.Count.ToString()).ToString());
                    //new XElement("guanxing.info", 
                    //    new XElement("up", aChief.SlotsBuffer[Top].Cards.Count), 
                    //    new XElement("down", aChief.SlotsBuffer[Bottom].Cards.Count)
                    //    )
                    //    );
                aData.Game.CardsHeap.PutOnTop([.. aChief.SlotsBuffer[Top].Cards]);
                aData.Game.CardsHeap.PutOnBottom([.. aChief.SlotsBuffer[Bottom].Cards]);
                aChief.SlotsBuffer[Top].Cards.Clear();
                aChief.SlotsBuffer[Bottom].Cards.Clear();
                aChief.SlotsBuffer[Total].Cards.Clear();
            }
        }

        public override void AfterTurnEnd(ChiefBase aChief, GlobalData aData)
        {
            aChief.SlotsBuffer[Top].Cards.Clear();
            aChief.SlotsBuffer[Bottom].Cards.Clear();
            aChief.SlotsBuffer[Total].Cards.Clear();
        }
    }
}
