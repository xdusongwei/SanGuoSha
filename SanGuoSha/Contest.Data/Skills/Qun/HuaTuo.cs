using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Global;

namespace SanGuoSha.ServerCore.Contest.Data
{
    internal class SkillJiJiu : SkillBase
    {
        public SkillJiJiu()
            : base("急救", SkillEnabled.Disable, false)
        {

        }

        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.AskForTao && aData.Active != aChief && aData.Game.GamePlayers[aChief].HasCard)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.AskForTao && SkillStatus == SkillEnabled.Enable)
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (aSkillName == SkillName)
            {
                if (SkillStatus == SkillEnabled.Disable) return false;
                if (aAskFor != MessageCore.AskForEnum.AskForTao && aAskFor != MessageCore.AskForEnum.AskForTaoOrJiu) return false;
                if (aCards.Count() != 1) return false;
                if (!aData.Game.GamePlayers[aChief].HasCardsInHandOrEquipage(aCards)) return false;
                if (aCards[0].CardHuaSe != Card.Suit.Heart && aCards[0].CardHuaSe != Card.Suit.Diamond) return false;
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
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.HandAndEquipage, SkillName, aData.Game.Response.Cards, Card.Effect.Tao, aChief, null, null);
                    return new MessageCore.AskForResult(false, aChief, new ChiefBase[] { }, aData.Game.Response.Cards, Card.Effect.Tao, false, false, SkillName);
                }
            }
            return null;
        }
    }

    internal class SkillQingNang : SkillBase
    {
        public SkillQingNang()
            : base("青囊", SkillEnabled.Disable, false)
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
                if (aCards.Count() == 0 || !aData.Game.GamePlayers[aChief].HasCardsInHand(aCards)) return false;
                if (aTargets.Count() != 1) return false;
                if (aData.Game.GamePlayers[aTargets[0]].Dead || aData.Game.GamePlayers[aTargets[0]].Health == aData.Game.GamePlayers[aTargets[0]].MaxHealth) return false;
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
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] {aData.Game.Response.Targets[0].Chief }, aData.Game.Response.Cards));
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, SkillName, aData.Game.Response.Cards, Card.Effect.Skill, aChief, null, null);
                    aData.Game.RegainHealth(aData.Game.Response.Targets[0].Chief, 1);
                    return new MessageCore.AskForResult(false, aChief, new ChiefBase[] { }, aData.Game.Response.Cards, Card.Effect.Skill, false, false, SkillName);
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
