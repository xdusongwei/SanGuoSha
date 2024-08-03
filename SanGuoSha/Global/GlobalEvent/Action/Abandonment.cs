using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>弃牌问询,注意该方法仅用于弃牌阶段
        /// </summary>
        /// <param name="aChief">需要弃牌的武将</param>
        /// <returns>返回true表示弃牌正常</returns>
        private bool Abandonment(ChiefBase aChief)
        {
            bool first = true;
            //确定玩家弃牌的要求是否达到
            while (GamePlayers[aChief].Hands.Count > (GamePlayers[aChief].Health < 0 ? 0 : GamePlayers[aChief].Health))
            {
                //问询武将出牌
                string msg = new Beaver("askfor.abandonment", aChief.ChiefName).ToString();
                    //new XElement("askfor.abandonment",
                    //    new XElement("target", aChief.ChiefName)
                    //);
                MessageCore.AskForResult res = null;
                if (first)
                {
                    first = false;
                    //弃牌问询
                    res = AsynchronousCore.AskForCards(aChief, MessageCore.AskForEnum.Abandonment, new AskForWrapper(msg, this), gData);
                }
                else
                {
                    //弃牌问询
                    res = AsynchronousCore.AskForCards(aChief, MessageCore.AskForEnum.AbandonmentNext, new AskForWrapper(msg, this), gData);
                }

                if (res.TimeOut || res.Cards.Count() == 0)
                    res = new MessageCore.AskForResult(false, aChief, new ChiefBase[0], AutoAbandonment(aChief), Card.Effect.None, false, true, string.Empty);
                //是否超过弃牌数
                if (GamePlayers[aChief].Hands.Count - res.Cards.Count() < (GamePlayers[aChief].Health < 0 ? 0 : GamePlayers[aChief].Health))
                {
                    //超过了，不合法，自动选择
                    Card[] cards = AutoAbandonment(aChief);
                    if (!RemoveHand(aChief, cards)) return false;
                    CardsHeap.AddCards(cards);
                    AsynchronousCore.SendMessage(new Beaver("abandonment", aChief.ChiefName , GamePlayers[aChief].Hands.Count.ToString() , Card.Cards2Beaver("cards" , res.Cards) ).ToString());
                        //new XElement("abandonment",
                        //    new XElement("target", aChief.ChiefName),
                        //    new XElement("count", GamePlayers[aChief].Hands.Count),
                        //    Card.Cards2XML("cards", res.Cards)
                        //));
                }
                else
                {
                    if (!RemoveHand(aChief, res.Cards)) return false;
                    //EventNode(true, CardFrom.Hand, res.Cards, Card.Effect.None, aChief, null, null);
                    CardsHeap.AddCards(res.Cards);
                    AsynchronousCore.SendMessage(
                        new Beaver("abandonment", aChief.ChiefName , GamePlayers[aChief].Hands.Count , Card.Cards2Beaver("cards" , res.Cards )).ToString());
                        //new XElement("abandonment",
                        //    new XElement("target", aChief.ChiefName),
                        //    new XElement("count", GamePlayers[aChief].Hands.Count),
                        //    Card.Cards2XML("cards", res.Cards)
                        //));

                }
                //若牌数满足上限,弃牌完成
                if (GamePlayers[aChief].Hands.Count == GamePlayers[aChief].Health)
                {
                    return true;
                }
            }
            //牌数小于限制,跳过弃牌
            return true;
        }
    }
}
