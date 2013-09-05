using System.Linq;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Data;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 无中生有的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard WuZhongShengYouProc(EventRecoard r)
        {
            //玩家不能死亡
            if (!GamePlayers[r.Source].Dead)
            {
                //进入无懈可击的过程
                if (WuXieProc(r.Target, Card.Effect.WuZhongShengYou)) return r;
                //从牌堆拿出来两张牌
                Card[] ret = TakeingCards(r.Source, 2);
                //加入到子事件中
                DropCards(false, CardFrom.None, r.SkillName, ret, Card.Effect.None, r.Source, r.Source, null);
            }
            return r;
        }
    }
}
