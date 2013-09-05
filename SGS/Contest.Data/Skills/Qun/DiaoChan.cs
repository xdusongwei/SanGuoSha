using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    internal class SkillLiJian : SkillBase
    {
        public SkillLiJian()
            : base("离间", SkillEnabled.Enable, false)
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
                if (aCards.Count() != 1 || !aData.Game.GamePlayers[aChief].HasCardsInHand(aCards)) return false;
                if (aTargets.Count() != 2) return false;
                if (aData.Game.GamePlayers[aTargets[0]].Chief.Sex != ChiefBase.SexType.Male || aData.Game.GamePlayers[aTargets[1]].Chief.Sex != ChiefBase.SexType.Male) return false;
                if (aData.Game.GamePlayers[aTargets[0]].Dead || aData.Game.GamePlayers[aTargets[1]].Dead) return false;
                if (aTargets[0] == aTargets[1]) return false;
                if (aTargets[0] == aChief || aTargets[1] == aChief) return false;
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
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, Player.Players2Chiefs(aData.Game.Response.Targets), aData.Game.Response.Cards));
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, SkillName, aData.Game.Response.Cards, Card.Effect.Skill, aChief, null, null);
                    return new MessageCore.AskForResult(false, aChief, new ChiefBase[] { aData.Game.Response.Targets[0].Chief , aData.Game.Response.Targets[1].Chief }, new Card[] { }, Card.Effect.JueDou, false, false, SkillName);
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

    internal class SkillBiYue : SkillBase
    {
        public SkillBiYue()
            : base("闭月", SkillEnabled.Passive , false)
        {

        }

        public override void AfterTurnEnd(ChiefBase aChief, GlobalData aData)
        {
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief , this , new ChiefBase[]{} , new Card[]{}));
            aData.Game.TakeingCards(aChief, 1);
        }
    }
}
