using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillWuShuang : SkillBase
    {
        public SkillWuShuang()
            : base("无双", SkillEnabled.Passive, false)
        {

        }

        public override int CalcAskforTimes(ChiefBase aChief, ChiefBase aTarget, Card.Effect aEffect, int aOldTimes, GlobalData aData)
        {
            switch (aEffect)
            {
                case Card.Effect.Sha:
                case Card.Effect.JueDou:
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [aTarget], []));
                    return 2;
                default:
                    return aOldTimes;
            }
        }
    }
}
