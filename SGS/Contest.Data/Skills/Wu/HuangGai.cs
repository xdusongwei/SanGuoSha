using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    internal class SkillKuRou : SkillBase
    {
        public SkillKuRou()
            : base("苦肉" , SkillEnabled.Disable , false)
        {

        }

        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Aggressive)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Aggressive && SkillStatus == SkillEnabled.Disable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
            }
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (SkillName == aSkillName)
            {
                if (SkillStatus != SkillEnabled.Enable) return false;
                if (aAskFor != MessageCore.AskForEnum.Aggressive) return false;
                if (aCards.Count() != 0) return false;
            }
            return true;
        }

        public override MessageCore.AskForResult AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aAskFor == MessageCore.AskForEnum.Aggressive && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { }, new Card[] { }));
                aData.Game.DamageHealth(aChief, 1, null, new GlobalEvent.EventRecoard(aChief, aChief, new Card[] { }, Card.Effect.Skill, SkillName));
                aData.Game.TakeingCards(aChief, 2);
                return new MessageCore.AskForResult(false, aChief, new ChiefBase[] { }, new Card[] { }, Card.Effect.Skill, false, false, SkillName);
            }
            return null;
        }
    }
}
