using System.Linq;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Data;
using BeaverMarkupLanguage;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 五谷丰登的执行过程
        /// </summary>
        /// <param name="r">起始子事件对象</param>
        /// <returns>起始子事件</returns>
        private EventRecoard WuGuFengDengProc(EventRecoard r)
        {
            //子事件的目标必须存在且目标不能死亡
            if (r.Target != null && !GamePlayers[r.Target].Dead)
            {
                //开始无懈可击的处理
                if (WuXieProc(r.Target, Card.Effect.WuGuFengDeng)) return r;
                //发送问询
                string msg = new Beaver("askfor.wgfd.select", r.Target.ChiefName).ToString();
                    //new XElement("askfor.wgfd.select",
                    //    new XElement("target", r.Target.ChiefName)
                    //    );
                //获取问询结果
                MessageCore.AskForResult res = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.WuGuFengDeng, new AskForWrapper(msg, this), gData);
                //回应的牌不正确,则自动选择一张
                if (res.Cards.Count() != 1 || (res.Cards.Count() == 1 && !CardsBuffer[WGFDSlotName].Cards.Contains(res.Cards[0])))
                {
                    res = new MessageCore.AskForResult(false, r.Target, new ChiefBase[] { }, new Card[] { CardsBuffer[WGFDSlotName].Cards[GetRandom(CardsBuffer[WGFDSlotName].Cards.Count)] }, Card.Effect.None, false, false, res.SkillName);
                }
                foreach (Card c in res.Cards)
                {
                    CardsBuffer[WGFDSlotName].Cards.Remove(c);
                    GamePlayers[r.Target].Hands.Add(c);
                }
                //发送消息
                AsynchronousCore.SendMessage(
                    new Beaver("wgfd.select", r.Target.ChiefName, Card.Cards2Beaver("cards", res.Cards)).ToString());
                    //new XElement("wgfd.select",
                    //    new XElement("target", r.Target.ChiefName),
                    //    Card.Cards2XML("cards", res.Cards)
                    //));
            }
            return r;
        }
    }
}
