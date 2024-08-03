using System.Linq;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Global;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Data
{
    internal class SkillJiJiang : SkillBase
    {
        public SkillJiJiang()
            : base("激将", SkillEnabled.Disable, true)
        {

        }

        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aChief.ChiefStatus == ChiefBase.Status.Majesty && (aEffect == MessageCore.AskForEnum.Sha || (aEffect == MessageCore.AskForEnum.Aggressive && (aData.ShaNoLimit || aData.KillRemain > 0))))
            {
                bool ShuAlive = false;
                foreach (Player p in aData.Game.GamePlayers.All)
                    if (!p.Dead && p.Chief.ChiefCamp == ChiefBase.Camp.Shu)
                    {
                        ShuAlive = true;
                        break;
                    }
                if (ShuAlive)
                    SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (aSkillName == SkillName)
                if (SkillStatus == SkillEnabled.Enable && (aAskFor == MessageCore.AskForEnum.Sha || aAskFor == MessageCore.AskForEnum.Aggressive) && aCards.Count() == 0)
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
                    return true;
                }
                else
                {
                    return false; //false
                }
            else
            {
                return true;
            }
        }

        public override MessageCore.AskForResult AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName && aChief.ChiefStatus == ChiefBase.Status.Majesty && (aAskFor == MessageCore.AskForEnum.Sha || aAskFor == MessageCore.AskForEnum.Aggressive) && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                ChiefBase[] targets = Player.Players2Chiefs(aData.Game.Response.Targets);
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, targets, new Card[] { }));
                ChiefBase t = aChief.Next;
                
                while (!t.IsMe(aChief))
                {
                    if (t.ChiefCamp == ChiefBase.Camp.Shu)
                    {
                        string msg = new Beaver("askfor.jijiang.sha", t.ChiefName).ToString();
                            new XElement("askfor.jijiang.sha",
                            new XElement("target", t.ChiefName)
                            );
                        MessageCore.AskForResult res2 = aData.Game.AsynchronousCore.AskForCards(t, MessageCore.AskForEnum.Sha, new AskForWrapper(msg, aData.Game), aData);
                        if (res2.Effect == Card.Effect.Sha)
                        {
                            aData.Game.AsynchronousCore.SendMessage(
                                new Beaver("jijiang.sha",t.ChiefName , Card.Cards2Beaver("cards" , res2.Cards) , res2.SkillName).ToString());
                            //new XElement("jijiang.sha",
                            //    new XElement("target", t.ChiefName),
                            //    Card.Cards2XML("cards", res2.Cards),
                            //    new XElement("skill" , res2.SkillName)
                            //    )
                            //);
                            if (res2.PlayerLead)
                                aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, string.Empty, res2.Cards, Card.Effect.Sha, t, null, null);
                            return new MessageCore.AskForResult(false, aChief, targets, res2.Cards, Card.Effect.Sha, false, false , SkillName);
                        }
                    }
                    t = t.Next;
                }
            }
            return null;
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if ((aEffect == MessageCore.AskForEnum.Sha || aEffect == MessageCore.AskForEnum.Aggressive) && SkillStatus == SkillEnabled.Enable)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
            }
        }
    }

    internal class SkillRenDe : SkillBase
    {
        public SkillRenDe()
            : base("仁德", SkillEnabled.Disable , false )
        {

        }

        private int TotalCount = 0;
        private bool HasRegainHealth = false;
        
        public override void BeforeTurnStart(ChiefBase aChief, GlobalData aData)
        {
            TotalCount = 0;
            HasRegainHealth = false;
        }

        public override void AfterTurnEnd(ChiefBase aChief, GlobalData aData)
        {
            TotalCount = 0;
            HasRegainHealth = false;
        }

        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Aggressive && aData.Game.GamePlayers[aChief].Hands.Count > 0)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
            }
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aEffect == MessageCore.AskForEnum.Aggressive && SkillStatus == SkillEnabled.Enable)
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (SkillName == aSkillName)
            {
                if (aCards.Count() != 0 && aTargets.Count() == 1 && aTargets[0] != aChief && !aData.Game.GamePlayers[aTargets[0]].Dead)
                {
                    if (!aData.Game.GamePlayers[aChief].HasCardsInHand(aCards)) return false;
                    return true;
                }
            }
            return true;
        }

        public override MessageCore.AskForResult AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName && SkillStatus == SkillEnabled.Enable && aAskFor == MessageCore.AskForEnum.Aggressive)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
                ChiefBase target = aData.Game.Response.Targets[0].Chief;
                Card[] cards = aData.Game.Response.Cards;
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { target }, new Card[] { }));
                TotalCount += aData.Game.Response.Cards.Count();
                aData.Game.AsynchronousCore.SendGiveMessage(aChief, target, cards, aData.Game.GamePlayers);
                aData.Game.Move(aChief, target, cards);
                if (TotalCount > 1 && !HasRegainHealth)
                {
                    aData.Game.RegainHealth(aChief, 1);
                    HasRegainHealth = true;
                }
            }
            return null;
        }
    }
}
