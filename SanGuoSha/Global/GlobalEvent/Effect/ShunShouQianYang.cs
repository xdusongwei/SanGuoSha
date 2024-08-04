using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Data;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 顺手牵羊的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard ShunShouQianYangProc(EventRecoard r)
        {
            //玩家自己和对方不能死亡,并且对方有牌
            if (!GamePlayers[r.Source].Dead && !GamePlayers[r.Target].Dead && GamePlayers[r.Target].HasCardWithJudgementArea)
            {
                //无懈可击的过程
                if (WuXieProc(r.Target, Card.Effect.ShunShouQianYang)) return r;
                AsynchronousCore.SendMessage(
                    new Beaver("askfor.ssqy.select",r.Source.ChiefName, r.Target.ChiefName).ToString());
                    //new XElement("askfor.ssqy.select",
                    //    new XElement("target", r.Source.ChiefName),
                    //    new XElement("target2", r.Target.ChiefName)
                    //));
                //问询玩家选择对方一张牌
                MessageCore.AskForResult? res = AsynchronousCore.AskForCards(r.Source, MessageCore.AskForEnum.TargetCardWithJudgementArea, r.Target);
                //如果没有选那就自动抽一张
                if (res.Effect == Card.Effect.None || res.Cards.Count() != 1)
                {
                    Card auto = AutoSelect(r.Target);
                    if (auto == null)
                        res = new MessageCore.AskForResult(false, res.Leader, res.Targets, [], Card.Effect.ShunShouQianYang, false, true, string.Empty);
                    else
                        res = new MessageCore.AskForResult(false, res.Leader, res.Targets, [auto], Card.Effect.ShunShouQianYang, false, true, string.Empty);
                }

                if (res.Cards.Count() != 0)
                {
                    Move(r.Target, r.Source, res.Cards);
                    ////把牌给玩家
                    //foreach (Card c in res.Cards)
                    //{
                    //    AsynchronousCore.SendStealMessage(r.Target, r.Source, new Card[] { c }, GamePlayers);
                    //    GamePlayers[r.Source].Hands.Add(c.GetOriginalCard());
                    //}
                    ////增加节点:顺手牵羊选择的牌
                    //EventNode(false, CardFrom.HandAndEquipageAndJudgement, res.SkillName, res.Cards, Card.Effect.None, r.Target, r.Source, null);

                }
            }
            return r;
        }
    }
}
