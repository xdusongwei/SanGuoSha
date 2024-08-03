using System.Linq;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Data;
using System.Collections.Generic;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 无懈可击子事件
        /// </summary>
        /// <param name="aTarget">需要无懈可击的目标</param>
        /// <param name="aEffect">无懈可击的效果</param>
        /// <returns>true表示无懈可击成立,反之不成立</returns>
        protected bool WuXieProc(ChiefBase aTarget, Card.Effect aEffect)
        {
            //这个量来表示场上是否有无懈可击存在
            bool WuXieExist = false;
            //表决字典
            //表决字典是用来记录玩家(键)的表态(值)
            //某个玩家的值为false代表没有表态,true表带已表态
            //通信层根据这个字典会对值为False的玩家进行并行问询
            //若其中有玩家问询时选择确认(一般指放弃),将其值定位true
            //若所有玩家都是true或超时,表决结束,问询返回None
            //若有玩家出 无懈可击 时,表决问询将直接结束,返回 WuXieKeJi问询
            Dictionary<Player, bool> abstention = new Dictionary<Player, bool>();
            ChiefBase s = aTarget;
            //遍历场上的玩家
            do
            {
                if (!GamePlayers[s].Dead && GamePlayers[s].Hands.Find(c => c.CardEffect == Card.Effect.WuXieKeJi) != null)
                {
                    //这个玩家有手牌无懈可击,将他加入表决字典并置value为false,表示他可以表决
                    abstention.Add(GamePlayers[s], false);
                    //无懈可击存在
                    WuXieExist = true;
                }
                else
                {
                    //这个玩家没有无懈可击,设置其值为true,表示已表决
                    abstention.Add(GamePlayers[s], true);
                }
                s = s.Next;
            } while (s != aTarget);

            //无懈可击存在,启动问询
            if (WuXieExist)
            {
                AsynchronousCore.SendMessage(
                    new Beaver("askfor.wxkj", aTarget.ChiefName, aEffect.ToString()).ToString());
                    //new XElement("askfor.wxkj",
                    //    new XElement("from", aTarget.ChiefName),
                    //    new XElement("effect", aEffect)
                    //    )
                    //);
                //开始问询
                MessageCore.AskForResult res = AsynchronousCore.AskForCards(MessageCore.AskForEnum.WuXieKeJi, abstention);
                ValidityResult(res.Leader, ref res);
                //若有玩家打出无懈可击
                if (res.Effect == Card.Effect.WuXieKeJi)
                {
                    //事件节点加入
                    DropCards(true, CardFrom.Hand, res.SkillName, res.Cards, Card.Effect.WuXieKeJi, res.Leader, aTarget, null);
                    AsynchronousCore.SendMessage(
                        new Beaver("wxkj",res.Leader.ChiefName , Card.Cards2Beaver("cards" ,res.Cards)).ToString());
                        //new XElement("wxkj",
                        //    new XElement("from", res.Leader.ChiefName),

                        //    Card.Cards2XML("cards", res.Cards)
                        //));
                    //目标换成出牌者
                    aTarget = res.Leader;
                    //两个结果异或即本轮结果
                    return true ^ WuXieProc(aTarget, Card.Effect.WuXieKeJi);
                }
            }
            //返回false
            return false;
        }
    }
}
