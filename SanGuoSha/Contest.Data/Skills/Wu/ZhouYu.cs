using SanGuoSha.Contest.Global;
using System.Linq;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Data
{
    internal class SkillFanJian : SkillBase
    {
        public SkillFanJian()
            : base("反间", SkillEnabled.Disable, false)
        {

        }

        public override void Leading(ChiefBase aChief, GlobalData aData)
        {
            if(aData.Game.GamePlayers[aChief].Hands.Count != 0 )
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (aSkillName == SkillName)
            {
                if (SkillStatus != SkillEnabled.Enable) return false;
                if (!aData.Game.GamePlayers[aChief].HasHand) return false;
                if (aCards.Count() != 0) return false;
                if (aTargets.Count() == 0 || aData.Game.GamePlayers[aTargets[0]].Dead || aTargets[0] == aChief) return false;
            }
            return true;
        }

        public override MessageCore.AskForResult? AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName)
            {
                if (SkillStatus == SkillEnabled.Enable && aData.Game.GamePlayers[aChief].HasHand)
                {
                    SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, Player.Players2Chiefs(aData.Game.Response.Targets), []));
                    ChiefBase target = aData.Game.Response.Targets[0].Chief;
                    aData.Game.AsynchronousCore.SendMessage(new Beaver("askfor.fanjian.suit" , target.ChiefName).ToString());// new XElement("askfor.fanjian.suit", new XElement("target", target.ChiefName)));
                    MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForCards(target, MessageCore.AskForEnum.Suit, target);
                    if (res.Cards.Count() == 0)
                        aData.Game.AsynchronousCore.SendMessage(new Beaver("fanjian.suit" , target.ChiefName , Card.Cards2Beaver("cards" , [CardHeap.Spade])).ToString());// new XElement("fanjian.suit", new XElement("target", target.ChiefName), Card.Cards2XML("cards", new Card[] { CardHeap.Spade })));
                    else
                        aData.Game.AsynchronousCore.SendMessage(new Beaver("fanjian.suit" , target.ChiefName , Card.Cards2Beaver("cards" , res.Cards)).ToString());// new XElement("fanjian.suit", new XElement("target", target.ChiefName), Card.Cards2XML("cards", res.Cards)));
                    System.Threading.Thread.Sleep(10);
                    aData.Game.AsynchronousCore.SendMessage(new Beaver("askfor.fanjian.card" ,target.ChiefName , aChief.ChiefName).ToString());//  new XElement("askfor.fanjian.card", new XElement("target", target.ChiefName), new XElement("target2", aChief)));
                    MessageCore.AskForResult? res2 = aData.Game.AsynchronousCore.AskForCards(target, MessageCore.AskForEnum.TargetCard, aChief);
                    Card c = aData.Game.AutoSelect(aChief);
                    aData.Game.AsynchronousCore.SendMessage(new Beaver("fanjian.card" , target.ChiefName , aChief.ChiefName , Card.Cards2Beaver("cards" , [c])).ToString());// new XElement("fanjian.card", new XElement("target", target.ChiefName), new XElement("target2", aChief.ChiefName), Card.Cards2XML("cards", new Card[] { c })));
                    if (c.CardSuit != res.Cards[0].CardSuit)
                    {
                        aData.Game.DamageHealth(target, 1, aChief, new GlobalEvent.EventRecoard(aChief, target, [], Card.Effect.Skill, SkillName));
                    }
                    if (!aData.Game.GamePlayers[target].Dead)
                    {
                        if (aData.Game.GamePlayers[aChief].HasCardsInHand([c]))
                        {
                            aData.Game.AsynchronousCore.SendGiveMessage(aChief, target, [c], aData.Game.GamePlayers);
                            aData.Game.Move(aChief, target, [c]);
                        }
                        else if (aData.Game.HasCardsInBin([c]))
                        {
                            if (aData.Game.PickRubbish([c]))
                            {
                                aData.Game.AsynchronousCore.SendPickMessage(target, [c]);
                                aData.Game.GamePlayers[aChief].Hands.Add(c.GetOriginalCard());
                            }
                        }
                    }
                    return new MessageCore.AskForResult(false, aChief, [], [], Card.Effect.Skill, false, false, SkillName);
                }
            }
            return null;
        }

        public override void BeforeAbandonment(ChiefBase aChief, GlobalData aData)
        {
            if (SkillStatus == SkillEnabled.Enable)
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
        }
    }

    internal class SkillYingZi : SkillBase
    {
        public SkillYingZi()
            : base("英姿", SkillEnabled.Passive, false)
        {

        }

        public override void TakingCards(ChiefBase aChief, GlobalData aData)
        {
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
            aData.TakeCardsCount++;
        }
    }
}
