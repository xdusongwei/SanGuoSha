using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Data;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 过河拆桥的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard GuoHeChaiQiaoProc(EventRecoard r)
        {
            //双方不能有任何一方,并且对方有牌可以选择死亡
            if (!GamePlayers[r.Source].Dead && !GamePlayers[r.Target].Dead && GamePlayers[r.Target].HasCardWithJudgementArea)
            {
                //进入无懈可击的过程
                if (WuXieProc(r.Target, Card.Effect.GuoHeChaiQiao)) return r;
                AsynchronousCore.SendMessage(
                    new Beaver("askfor.ghcq.select", r.Source.ChiefName, r.Target.ChiefName).ToString());
                    //new XElement("askfor.ghcq.select",
                    //    new XElement("target", r.Source.ChiefName),
                    //    new XElement("target2", r.Target.ChiefName)
                    //));
                //问询玩家选择对方一张牌
                MessageCore.AskForResult res = AsynchronousCore.AskForCards(r.Source, MessageCore.AskForEnum.TargetCardWithJudgementArea, r.Target);
                //如果没有选择那就系统选择一张牌
                if (res.Effect == Card.Effect.None)
                {
                    Card auto = AutoSelect(r.Target);
                    if (auto == null)
                        res = new MessageCore.AskForResult(false, res.Leader, res.Targets, [], Card.Effect.GuoHeChaiQiao, false, true, string.Empty);
                    else
                    {
                        res = new MessageCore.AskForResult(false, res.Leader, res.Targets, [auto], Card.Effect.GuoHeChaiQiao, false, true, string.Empty);
                    }
                }

                if (res.Cards.Count() != 0)
                {

                    //将对方的这张牌加入到子事件节点上并弃置到垃圾桶中
                    DropCards(true, CardFrom.HandAndEquipageAndJudgement, res.SkillName, res.Cards, Card.Effect.None, r.Target, r.Source, null);
                    AsynchronousCore.SendMessage(MessageCore.MakeDropMessage(r.Source, r.Target, res.Cards));
                }
            }
            return r;
        }
    }
}
