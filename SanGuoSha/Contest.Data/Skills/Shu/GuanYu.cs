using System.Linq;
using SanGuoSha.ServerCore.Contest.Global;

namespace SanGuoSha.ServerCore.Contest.Data
{
    internal class SkillWuSheng : SkillBase
    {
        public SkillWuSheng()
            : base("武圣" , SkillEnabled.Disable , false)
        {

        }


        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Sha || (aEffect == MessageCore.AskForEnum.Aggressive && (aData.ShaNoLimit || aData.KillRemain > 0)))
            {
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if ((aEffect == MessageCore.AskForEnum.Sha || aEffect == MessageCore.AskForEnum.Aggressive) && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
            }
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (SkillName == aSkillName)
            {
                if (SkillStatus == SkillEnabled.Enable && aCards.Count() == 1 && (aCards[0].CardSuit == Card.Suit.Diamond || aCards[0].CardSuit == Card.Suit.Heart) && (aAskFor == MessageCore.AskForEnum.Sha || aAskFor == MessageCore.AskForEnum.Aggressive))
                {
                    if (aAskFor == MessageCore.AskForEnum.Aggressive)
                    {
                        //若出杀有限制且已经没有机会杀了不能执行
                        if (!aData.ShaNoLimit && aData.KillRemain < 1) return false; //false 
                        //杀的目标数量高于最大值不能执行
                        if (aData.Game.CalcMaxShaTargets(aChief , aCards) < aTargets.Count()) return false; //false 
                        //没有目标不能执行
                        if (aTargets.Count() == 0) return false; //false 
                        //遍历目标集合,如果目标有自己或者目标已死亡或者 够不到对方不能执行
                        foreach (ChiefBase c in aTargets)
                        {
                            if (c.IsMe(aChief) || aData.Game.GamePlayers[c].Dead || !aData.Game.WithinShaRange(aChief, c))
                            {
                                return false; //false
                            }
                            bool Enable = true;
                            foreach (ASkill s in c.Skills)
                                Enable = s.EffectFeasible(aCards, aEffect, c, Enable, aData);
                            if (!Enable) return false;
                        }
                    }
                    aEffect = Card.Effect.Sha;
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public override MessageCore.AskForResult AskFor(SanGuoSha.ServerCore.Contest.Data.ChiefBase aChief, SanGuoSha.ServerCore.Contest.Global.MessageCore.AskForEnum aAskFor, SanGuoSha.ServerCore.Contest.Data.GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, Player.Players2Chiefs(aData.Game.Response.Targets), aData.Game.Response.Cards));
                return new MessageCore.AskForResult(aData.Game.Response.IsTimeout, aChief, Player.Players2Chiefs(aData.Game.Response.Targets), aData.Game.Response.Cards, Card.Effect.Sha, aData.Game.Response.Answer, true, SkillName);
            }
            return null;
        }
    }
}
