using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Data
{
    internal class SkillLiuLi : SkillBase
    {
        public SkillLiuLi()
            : base("流离", SkillEnabled.Passive, false)
        {

        }

        public override void PreprocessingSubEvent(ChiefBase aTargetChief,ref GlobalEvent.EventRecoard aEvent, GlobalData aData)
        {
            if (aEvent.Effect == Card.Effect.Sha)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aTargetChief, this));
                MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aTargetChief);
                if (res.YN)
                {
                    aData.Game.AsynchronousCore.SendMessage(new Beaver("askfor.liuli.args" , aTargetChief.ChiefName).ToString());// new XElement("askfor.liuli.args", new XElement("target", aTargetChief.ChiefName)));
                    res = aData.Game.AsynchronousCore.AskForCards(aTargetChief, MessageCore.AskForEnum.TargetHand, aTargetChief);
                    if (res.Targets.Count() != 1) return;
                    if (res.Targets[0] == aTargetChief) return;
                    if (res.Targets[0] == aEvent.Source) return;
                    if (aData.Game.GamePlayers[res.Targets[0]].Dead) return;
                    if (!aData.Game.WithinShaRange(aTargetChief, res.Targets[0])) return;
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aTargetChief, this, res.Targets, res.Cards));
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, SkillName, aData.Game.Response.Cards, Card.Effect.Skill, aTargetChief, null, null);
                    aEvent.Target = res.Targets[0];
                }
            }
        }
    }

    internal class SkillGuoSe : SkillBase
    {
        public SkillGuoSe()
            : base("国色", SkillEnabled.Disable, false)
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
                return new MessageCore.AskForResult(false, aChief, Player.Players2Chiefs(aData.Game.Response.Targets), aData.Game.Response.Cards, Card.Effect.LeBuSiShu, false, true, SkillName);
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
                if (aCards[0].CardSuit != Card.Suit.Diamond) return false;
                if (aTargets.Count() != 1) return false;
                if (aTargets[0] == aChief) return false;
                if (aData.Game.GamePlayers[aTargets[0]].HasDebuff(Card.Effect.LeBuSiShu)) return false;
                if (aData.Game.GamePlayers[aTargets[0]].Dead) return false;
                aCards[0].CardEffect = Card.Effect.LeBuSiShu;
                aEffect = Card.Effect.LeBuSiShu;
            }
            return true;
        }
    }
}
