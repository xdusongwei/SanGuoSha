using SGS.ServerCore.Contest.Data;
using SGS.ServerCore.Contest.Equipage;
using System.Xml.Linq;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 桃园结义的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard TaoYuanJieYi(EventRecoard r)
        {
            //玩家不能死亡,并且血没有满
            if (!GamePlayers[r.Target].Dead && GamePlayers[r.Target].Health != GamePlayers[r.Target].MaxHealth)
            {
                //进入无懈可击的过程
                if (WuXieProc(r.Target, Card.Effect.TaoYuanJieYi)) return r;
                //玩家血量增加
                RegainHealth(r.Target, 1);
            }
            return r;
        }
    }
}
