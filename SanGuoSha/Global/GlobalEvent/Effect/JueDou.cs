using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 决斗的执行过程
        /// </summary>
        /// <param name="r">子事件起始节点</param>
        /// <returns></returns>
        internal EventRecoard JueDouProc(EventRecoard r)
        {
            string msg = null;
            if (r.Target2 != null)
            {
                r.Source = r.Target;
                r.Target = r.Target2;
            }
            //玩家双方都不能死亡
            if (!GamePlayers[r.Target].Dead && !GamePlayers[r.Source].Dead)
            {
                if(r.Target2== null)
                //无懈可击的过程
                    if (WuXieProc(r.Target, Card.Effect.JueDou)) return r;
                while (true)
                {
                    int times = 1;
                    foreach (ASkill s in r.Source.Skills)
                        times = s.CalcAskforTimes(r.Source, r.Target, Card.Effect.JueDou, times, gData);
                    for (int i = 0; i < times; i++)
                    {
                        //先询问对方要出杀
                        MessageCore.AskForResult res = null;
                        msg = new Beaver("askfor.jd.sha", r.Target.ChiefName, r.Source.ChiefName).ToString();
                            //new XElement("askfor.jd.sha",
                            //new XElement("target", r.Target.ChiefName),
                            //new XElement("opponent", r.Source.ChiefName)
                            //);
                        res = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.Sha, new AskForWrapper(msg, this), gData);
                        ValidityResult(r.Target, ref res);
                        if (res.Effect != Card.Effect.Sha)
                        {
                            //不出杀费血
                            sbyte cost = 1;
                            foreach (ASkill s in r.Source.Skills)
                                cost = s.CalcDamage(r.Source, Card.Effect.JueDou, cost, gData);
                            if (GamePlayers[r.Target].Armor != null)
                                cost = Armor.CalcDamage(cost, r.Cards, GamePlayers[r.Target].Armor.CardEffect);
                            DamageHealth(r.Target, cost, r.Source, r);
                            return r;
                        }
                        else
                        {
                            //出杀加入子事件节点
                            if(res.PlayerLead)
                                DropCards(true, CardFrom.Hand, res.SkillName, res.Cards, Card.Effect.Sha, res.Leader, r.Source, null);
                            foreach (ASkill s in res.Leader.Skills)
                                s.OnUseEffect(res.Leader, Card.Effect.Sha, gData);
                            AsynchronousCore.SendMessage(
                                new Beaver("jd.sha" , r.Target.ChiefName , r.Source.ChiefName , Card.Cards2Beaver("cards" , res.Cards )).ToString());
                                //new XElement("jd.sha",
                                //    new XElement("target", r.Target.ChiefName),
                                //    new XElement("opponent", r.Source.ChiefName),
                                //    Card.Cards2XML("cards", res.Cards)
                                //));
                        }
                    }
                    times = 1;
                    foreach (ASkill s in r.Target.Skills)
                        times = s.CalcAskforTimes(r.Target, r.Source, Card.Effect.JueDou, times, gData);
                    for (int i = 0; i < times; i++)
                    {
                        msg = new Beaver("askfor.jd.sha", r.Source.ChiefName, r.Target.ChiefName).ToString();
                        //new XElement("askfor.jd.sha",
                        //new XElement("target", r.Source.ChiefName),
                        //new XElement("opponent", r.Target.ChiefName)
                        //);
                        //问询自己出杀
                        MessageCore.AskForResult res2 = AsynchronousCore.AskForCards(r.Source, MessageCore.AskForEnum.Sha, new AskForWrapper(msg, this), gData);
                        ValidityResult(r.Source, ref res2);
                        if (res2.Effect != Card.Effect.Sha)
                        {
                            //不出杀费血
                            sbyte cost = 1;
                            foreach (ASkill s in r.Target.Skills)
                                cost = s.CalcDamage(r.Target, r.Effect, cost, gData);
                            if (GamePlayers[r.Source].Armor != null)
                                cost = Armor.CalcDamage(cost, r.Cards, GamePlayers[r.Source].Armor.CardEffect);
                            DamageHealth(r.Source, cost, r.Target, r);
                            return r;
                        }
                        else
                        {
                            //出杀加入子事件节点
                            if (res2.PlayerLead)
                                DropCards(true, CardFrom.Hand, res2.SkillName, res2.Cards, Card.Effect.Sha, res2.Leader, r.Target, null);
                            foreach (ASkill s in res2.Leader.Skills)
                                s.OnUseEffect(res2.Leader, Card.Effect.Sha, gData);
                            AsynchronousCore.SendMessage(
                                new Beaver("jd.sha", r.Source.ChiefName , r.Target.ChiefName , Card.Cards2Beaver("cards" ,res2.Cards)).ToString());
                                //new XElement("jd.sha",
                                //    new XElement("target", r.Source.ChiefName),
                                //    new XElement("opponent", r.Target.ChiefName),
                                //    Card.Cards2XML("cards", res2.Cards)
                                //));
                        }
                    }
                }
            }
            return r;
        }
    }
}
