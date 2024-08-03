using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillLuoYi : SkillBase
    {
        public SkillLuoYi()
            : base("裸衣" , SkillEnabled.Passive , false)
        {

        }

        private bool Active = false;

        public override void BeforeTurnStart(ChiefBase aChief, GlobalData aData)
        {
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aChief, this));
            MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aChief);
            if (res.YN)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                Active = true;
                aData.TakeCardsCount--;
            }
        }

        public override void AfterTurnEnd(ChiefBase aChief, GlobalData aData)
        {
            Active = false;
        }

        public override sbyte CalcDamage(ChiefBase aChief, Card.Effect aEffect, sbyte aDamage, GlobalData aData)
        {
            if (Active)
                switch (aEffect)
                {
                    case Card.Effect.Sha:
                    case Card.Effect.JueDou:
                        return ++aDamage;
                }
            return aDamage;
        }
    }
}
