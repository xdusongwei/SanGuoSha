using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    internal class SkillXiaoJi : SkillBase
    {
        public SkillXiaoJi()
            : base("枭姬", SkillEnabled.Passive, false)
        {

        }

        public override void DropEquipage(ChiefBase aChief, GlobalData aData)
        {
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { }, new Card[] { }));
            aData.Game.TakeingCards(aChief, 2);
        }
    }

    internal class SkillJieYin : SkillBase
    {
        public SkillJieYin()
            : base("结姻", SkillEnabled.Disable, false)
        {

        }

        public override void Leading(ChiefBase aChief, GlobalData aData)
        {
            if (aData.Game.GamePlayers[aChief].Hands.Count > 1)
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
        }

        public override void BeforeAbandonment(ChiefBase aChief, GlobalData aData)
        {
            if (SkillStatus == SkillEnabled.Enable)
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (aSkillName == SkillName)
            {
                if (SkillStatus == SkillEnabled.Disable) return false;
                if (aCards.Count() != 2) return false;
                if (!aData.Game.GamePlayers[aChief].HasCardsInHand(aCards)) return false;
                if (aTargets.Count() != 1) return false;
                if (aTargets[0].Sex != ChiefBase.SexType.Male) return false;
                if (aTargets[0] == aChief) return false;
                if (aData.Game.GamePlayers[aTargets[0]].Dead || aData.Game.GamePlayers[aTargets[0]].Health == aData.Game.GamePlayers[aTargets[0]].MaxHealth) return false;
            }
            return true;
        }

        public override SGS.ServerCore.Contest.Global.MessageCore.AskForResult AskFor(SGS.ServerCore.Contest.Data.ChiefBase aChief, SGS.ServerCore.Contest.Global.MessageCore.AskForEnum aAskFor, SGS.ServerCore.Contest.Data.GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName)
            {
                if (SkillStatus == SkillEnabled.Enable)
                {
                    SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { aData.Game.Response.Targets[0].Chief }, aData.Game.Response.Cards));
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, SkillName, aData.Game.Response.Cards, Card.Effect.Skill, aChief, null, null);
                    aData.Game.RegainHealth(aData.Game.Response.Targets[0].Chief, 1);
                    aData.Game.RegainHealth(aChief, 1);
                }
            }
            return null;
        }
    }
}
