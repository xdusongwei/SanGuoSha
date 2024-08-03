using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillQiXi : SkillBase
    {
        public SkillQiXi()
            : base("奇袭", SkillEnabled.Disable, false)
        {

        }

        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Aggressive && aData.Game.GamePlayers[aChief].HasHand)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Aggressive && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
            }
        }

        public override MessageCore.AskForResult? AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aAskFor == MessageCore.AskForEnum.Aggressive && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, Player.Players2Chiefs(aData.Game.Response.Targets), aData.Game.Response.Cards));
                return new MessageCore.AskForResult(false, aChief, Player.Players2Chiefs(aData.Game.Response.Targets), aData.Game.Response.Cards, Card.Effect.GuoHeChaiQiao, false, true, SkillName);
            }
            return null;
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (SkillName == aSkillName)
            {
                if (SkillStatus != SkillEnabled.Enable) return false;
                if (aAskFor != MessageCore.AskForEnum.Aggressive) return false;
                if (aCards.Count() != 1) return false;
                if (aCards[0].CardSuit != Card.Suit.Club && aCards[0].CardSuit != Card.Suit.Spade) return false;
                if (aTargets.Count() != 1) return false;
                if (aTargets[0] == aChief) return false;
                if (!aData.Game.GamePlayers[aTargets[0]].HasCardWithJudgementArea) return false;
                if (aData.Game.GamePlayers[aTargets[0]].Dead) return false;
                aEffect = Card.Effect.GuoHeChaiQiao;
            }
            return true;
        }
    }
}
