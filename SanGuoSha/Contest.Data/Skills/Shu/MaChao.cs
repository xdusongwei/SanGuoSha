using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillMaShu : SkillBase
    {
        public SkillMaShu()
            : base("马术", SkillEnabled.Passive, false)
        {

        }


        public override byte CalcKitDistance(ChiefBase aChief, byte aOldRange, GlobalData aData)
        {
            return ++aOldRange;
        }

        public override byte CalcShaDistance(ChiefBase aChief, byte aOldRange, GlobalData aData)
        {
            return ++aOldRange;
        }
    }

    internal class SkillTieQi :SkillBase
    {
        public SkillTieQi()
            : base("铁骑", SkillEnabled.Passive, false)
        {

        }


        public override int CalcAskforTimes(ChiefBase aChief, ChiefBase aTarget, Card.Effect aEffect, int aOldTimes, GlobalData aData)
        {
            if (aEffect == Card.Effect.Sha)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aChief, this));
                MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForYN(aChief);
                if (res.YN)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                    Card c = aData.Game.popJudgementCard(aChief, Card.Effect.TieQi);
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.JudgementCard, SkillName, [c], Card.Effect.TieQi, aChief, null, null);
                    if (c.CardSuit == Card.Suit.Heart || c.CardSuit == Card.Suit.Diamond)
                    {
                        return 0;
                    }
                }
            }
            return aOldTimes;
        }
    }
}
