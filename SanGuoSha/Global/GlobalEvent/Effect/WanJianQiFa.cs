using SanGuoSha.Contest.Data;
using SanGuoSha.Contest.Equipage;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 万箭齐发的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard WanJianQiFaProc(EventRecoard r)
        {
            string msg = null;
            //对方不能死亡
            if (!GamePlayers[r.Target].Dead)
            {
                //进入无懈可击的过程
                if (WuXieProc(r.Target, Card.Effect.WanJianQiFa)) return r;
                if (GamePlayers[r.Target].Armor == null || Armor.EnableFor(GamePlayers[r.Target].Armor.CardEffect, r.Cards, Card.Effect.WanJianQiFa, r.Target))
                {
                    MessageCore.AskForResult? resShan = null;
                    msg = new Beaver("askfor.wjqf.shan", r.Target.ChiefName, r.Source.ChiefName).ToString();
                        //new XElement("askfor.wjqf.shan",
                        //    new XElement("target", r.Target.ChiefName),
                        //    new XElement("source", r.Source.ChiefName)
                        //);
                        //问询闪
                        resShan = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.Shan, new AskForWrapper(msg, this), true, gData);
                        ValidityResult(r.Target, ref resShan);
                    //}
                    if (resShan.PlayerLead)
                        DropCards(true, CardFrom.Hand, resShan.SkillName, resShan.Cards, resShan.Effect, resShan.Leader, r.Source, null);
                    else
                        DropCards(false, CardFrom.None, resShan.SkillName, resShan.Cards, resShan.Effect, resShan.Leader, r.Source, null);
                    //将问询的牌放入到子事件节点
                    //EventNode(true, CardFrom.Hand, resShan.SkillName , resShan.Cards, resShan.Effect, r.Target, r.Source, null);
                    foreach (ASkill s in resShan.Leader.Skills)
                        s.OnUseEffect(resShan.Leader, Card.Effect.Shan, gData);
                    if (resShan.Effect == Card.Effect.Shan)
                    {
                        if (resShan.PlayerLead)
                            AsynchronousCore.SendMessage(
                                new Beaver("wjqf.shan",r.Target.ChiefName , r.Source.ChiefName , resShan.SkillName , Card.Cards2Beaver("cards" , resShan.Cards )).ToString());
                                //new XElement("wjqf.shan",
                                //    new XElement("target", r.Target.ChiefName),
                                //    new XElement("source", r.Source.ChiefName),
                                //    new XElement("skill", resShan.SkillName),
                                //    Card.Cards2XML("cards", resShan.Cards)
                                //));
                    }
                    else
                    {
                        //没有出闪费血
                        DamageHealth(r.Target, 1, r.Source, r);
                    }
                }
            }
            return r;
        }
    }
}
