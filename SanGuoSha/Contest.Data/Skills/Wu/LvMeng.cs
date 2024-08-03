using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillKeJi : SkillBase
    {
        public SkillKeJi()
            : base("克己" , SkillEnabled.Passive , false)
        {

        }

        private bool OnLeading = false;
        private bool UseShaEffect = false;

        public override void Leading(ChiefBase aChief, GlobalData aData)
        {
            OnLeading = true;
            UseShaEffect = false;
        }

        public override void OnUseEffect(ChiefBase aChief, Card.Effect aEffect, GlobalData aData)
        {
            if (aEffect == Card.Effect.Sha && OnLeading)
                UseShaEffect = true;
        }

        public override void BeforeAbandonment(ChiefBase aChief, GlobalData aData)
        {
            OnLeading = false;
            if (!UseShaEffect)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aChief, this));
                MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aChief);
                if (res.YN)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                    aData.Abandonment = false;
                }
            }
        }
    }
}
