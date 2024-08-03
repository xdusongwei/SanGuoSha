using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Global;

namespace SanGuoSha.ServerCore.Contest.Data
{
    internal class SkillJiuYuan : SkillBase
    {
        public SkillJiuYuan()
            : base("救援", SkillEnabled.Passive, true)
        {

        }

        public override sbyte CalcRescuePoint(ChiefBase aChief, ChiefBase aRescuer, Card.Effect aEffect, sbyte aOldPoint, GlobalData aData)
        {
            if (aRescuer != null && aData.Game.GamePlayers[aRescuer].Chief.ChiefCamp == ChiefBase.Camp.Wu && aEffect == Card.Effect.Tao)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { }, new Card[] { }));
                return ++aOldPoint;
            }
            else
            {
                return aOldPoint;
            }
        }
    }

    internal class SkillZhiHeng : SkillBase
    {
        public SkillZhiHeng()
            : base("制衡", SkillEnabled.Disable, false)
        {

        }

        public override void Leading(ChiefBase aChief, GlobalData aData)
        {
            SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (aSkillName == SkillName)
            {
                if (SkillStatus == SkillEnabled.Disable) return false;
                if (aCards.Count() == 0 || !aData.Game.GamePlayers[aChief].HasCardsInHandOrEquipage(aCards)) return false;
            }
            return true;
        }

        public override MessageCore.AskForResult AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName)
            {
                if (SkillStatus == SkillEnabled.Enable)
                {
                    SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { }, aData.Game.Response.Cards));
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.HandAndEquipage, SkillName, aData.Game.Response.Cards, Card.Effect.Skill, aChief, null, null);
                    aData.Game.TakeingCards(aChief, aData.Game.Response.Cards.Count());
                    return new MessageCore.AskForResult(false, aChief, new ChiefBase[] { }, new Card[] { }, Card.Effect.Skill, false, false, SkillName);
                }
            }
            return null;
        }

        public override void BeforeAbandonment(ChiefBase aChief, GlobalData aData)
        {
            if (SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
            }
        }
    }
}
