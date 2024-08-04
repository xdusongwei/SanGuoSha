using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Data
{
    internal class SkillJianXiong : SkillBase
    {
        public SkillJianXiong()
            : base("奸雄", SkillEnabled.Passive, false)
        {

        }

        public override void OnChiefHarmed(GlobalEvent.EventRecoard aSourceEvent, ChiefBase? aSource, ChiefBase aTarget, GlobalData aData, sbyte aDamage)
        {
            //不能有牌存在，且伤害事件的牌在垃圾桶中
            if (aSourceEvent.Cards.Count() != 0 && aData.Game.HasCardsInBin(aSourceEvent.Cards))
            {
                //问询是否发动技能
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aTarget, this));
                MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForYN(aTarget);
                if (res.YN == true && aData.Game.HasCardsInBin(aSourceEvent.Cards))
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aTarget, this, [], aSourceEvent.Cards));
                    aData.Game.PickRubbish(aSourceEvent.Cards);
                    foreach (Card c in aSourceEvent.Cards)
                        aData.Game.GamePlayers[aTarget].Hands.Add(c.GetOriginalCard());
                }
            }
        }
    }

    internal class SkillHuJia : SkillBase
    {
        public SkillHuJia()
            : base("护驾" , SkillEnabled.Disable , true)
        {

        }

        public override void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (aChief.ChiefStatus == ChiefBase.Status.Majesty)
                if (aEffect == MessageCore.AskForEnum.Shan)
                {
                    bool WeiAlive = false;
                    foreach(Player p in aData.Game.GamePlayers.All)
                        if (!p.Dead && p.Chief.ChiefCamp == ChiefBase.Camp.Wei)
                        {
                            WeiAlive = true;
                            break;
                        }
                    if(WeiAlive)
                        SwitchSkillStatus(aChief, SkillEnabled.Enable, aData);
                }
        }

        public override void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {
            if (SkillStatus == SkillEnabled.Enable && aEffect == MessageCore.AskForEnum.Shan)
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);
        }

        public override bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor, ref Card.Effect aEffect, GlobalData aData)
        {
            if (aSkillName == SkillName && aChief.ChiefStatus == ChiefBase.Status.Majesty) //要求
                if (SkillStatus != SkillEnabled.Enable || aAskFor != MessageCore.AskForEnum.Shan || aCards.Count() != 0) return false; //不满足的条件
            return true;
        }

        public override MessageCore.AskForResult? AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            if (aData.Game.Response.SkillName == SkillName && SkillStatus == SkillEnabled.Enable && aChief.ChiefStatus == ChiefBase.Status.Majesty && aAskFor == MessageCore.AskForEnum.Shan)
            {
                SwitchSkillStatus(aChief, SkillEnabled.Disable, aData);

                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                ChiefBase t = aChief.Next;
                while (!t.IsMe(aChief))
                {
                    if (t.ChiefCamp == ChiefBase.Camp.Wei)
                    {
                        string msg = new Beaver("askfor.hujia.shan", t.ChiefName).ToString();
                        //new XElement("askfor.hujia.shan",
                        //    new XElement("target", t.ChiefName)
                        //    );
                        MessageCore.AskForResult? res2 = aData.Game.AsynchronousCore.AskForCards(t, MessageCore.AskForEnum.Shan, new AskForWrapper(msg, aData.Game), aData);
                        if (res2.Effect == Card.Effect.Shan)
                        {
                            aData.Game.AsynchronousCore.SendMessage(
                                new Beaver("hujia.shan",t.ChiefName ,Card.Cards2Beaver("cards" ,res2.Cards)).ToString());
                            //new XElement("hujia.shan",
                            //    new XElement("target", t.ChiefName),
                            //    Card.Cards2XML("cards", res2.Cards)
                            //    )
                            //);
                            return new MessageCore.AskForResult(false, t, [], res2.Cards, Card.Effect.Shan, false, res2.PlayerLead, SkillName);
                        }
                    }
                    t = t.Next;
                }
            }
            return null;
        }
    }
}
