using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 南蛮入侵的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard NanManRuQinProc(EventRecoard r)
        {
            //对方不能死亡
            if (!GamePlayers[r.Target].Dead)
            {
                //无懈可击的执行过程
                if (WuXieProc(r.Target, Card.Effect.NanManRuQin))
                {
                    return r;
                }

                if (GamePlayers[r.Target].Armor == null || Armor.EnableFor(GamePlayers[r.Target].Armor.CardEffect, r.Cards, Card.Effect.NanManRuQin, r.Target))
                {
                    string msg = new Beaver("askfor.nmrq.sha", r.Target.ChiefName, r.Source.ChiefName).ToString();
                        //new XElement("askfor.nmrq.sha",
                        //    new XElement("target", r.Target.ChiefName),
                        //    new XElement("source", r.Source.ChiefName)
                        //);
                    //问询杀
                    MessageCore.AskForResult resSha = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.Sha, new AskForWrapper(msg, this), gData);
                    ValidityResult(r.Target, ref resSha);
                    if (resSha.Effect == Card.Effect.Sha)
                    {
                        //将杀放入子事件节点
                        if (resSha.PlayerLead)
                        {
                            DropCards(true, CardFrom.Hand, resSha.SkillName, resSha.Cards, Card.Effect.None, resSha.Leader, r.Source, null);
                            foreach (ASkill s in resSha.Leader.Skills)
                                s.OnUseEffect(resSha.Leader, Card.Effect.Sha, gData);
                            AsynchronousCore.SendMessage(
                                new Beaver("nmrq.sha", r.Target.ChiefName , r.Source.ChiefName , resSha.SkillName , Card.Cards2Beaver("cards" , resSha.Cards )).ToString());
                        //        new XElement("nmrq.sha",
                        //            new XElement("target", r.Target.ChiefName),
                        //            new XElement("source", r.Source.ChiefName),
                        //            new XElement("skill", resSha.SkillName),
                        //            Card.Cards2XML("cards", resSha.Cards)
                        //        ));
                        }
                    }
                    else
                    {
                        //没有杀,费血
                        DamageHealth(r.Target, 1, r.Source, r);
                    }
                }
            }
            return r;
        }
    }
}
