using SGS.ServerCore.Contest.Data;
using SGS.ServerCore.Contest.Equipage;
using System.Xml.Linq;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 桃的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard TaoProc(EventRecoard r)
        {
            //加血
            RegainHealth(r.Source, 1);
            return r;
        }
    }
}
