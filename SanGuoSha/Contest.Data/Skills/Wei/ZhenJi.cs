using System.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillLuoShen : SkillBase
    {
        public SkillLuoShen()
            : base("洛神" , SkillEnabled.Passive , false)
        {

        }

        public override void OnChiefJudgementCardTakeEffect(ChiefBase aChief, Card aCard, ref bool aEnableSendToBin, GlobalData aData)
        {
            if (aCard.CardEffect == Card.Effect.LuoShen)
            {
                if (aCard.CardSuit == Card.Suit.Club || aCard.CardSuit == Card.Suit.Spade)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], [aCard]));
                    aEnableSendToBin = false;
                    aData.Game.GamePlayers[aChief].Hands.Add(aCard.GetOriginalCard());
                }
            }
        }

        public override void BeforeTurnStart(ChiefBase aChief, GlobalData aData)
        {
            Card c = null;
            do
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aChief , this));
                MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForYN(aChief);
                if (res.YN)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                    c = aData.Game.popJudgementCard(aChief, Card.Effect.LuoShen);
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.JudgementCard, res.SkillName, [c], Card.Effect.None, aChief, null, null);
                }
            } while (c != null && (c.CardSuit == Card.Suit.Spade || c.CardSuit == Card.Suit.Club));
        }
    }

    internal class SkillQingGuo : SkillBase
    {
        public SkillQingGuo()
            : base("倾国" , SkillEnabled.Disable , false)
        {

        }
        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Shan && aData.Game.GamePlayers[aChief].HasHand)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }


        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (SkillStatus == SkillEnabled.Enable)
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (SkillName == aSkillName && aCards.Count() == 1 && aAskFor == MessageCore.AskForEnum.Shan && (aCards[0].CardSuit == Card.Suit.Spade || aCards[0].CardSuit == Card.Suit.Club ))
            {
                
                aEffect = Card.Effect.Shan;
                return true;
            }
            else if (aSkillName == SkillName)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override MessageCore.AskForResult? AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (SkillName == aData.Game.Response.SkillName && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], aData.Game.Response.Cards));
                return new MessageCore.AskForResult(false, aChief, [], aData.Game.Response.Cards, Card.Effect.Shan, false, true, SkillName);
            }
            return null;
        }
    }
}
