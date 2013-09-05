using SGS.ServerCore.Contest.Data;
using SGS.ServerCore.Contest.Equipage;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 安置乐不思蜀的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard LeBuSiShuProc(EventRecoard r)
        {
            //玩家不能是自己并且对方不能死亡
            if (r.Target != r.Source && !GamePlayers[r.Target].Dead)
            {
                //对方不能有乐不思蜀的buff
                if (!GamePlayers[r.Target].HasDebuff(Card.Effect.LeBuSiShu))
                {
                    //把乐不思蜀从垃圾桶中拿出来
                    PickRubbish(r.Cards);
                    //把乐不思蜀加入到对方的buff列表中
                    GamePlayers[r.Target].Debuff.Push(r.Cards[0]);

                    AsynchronousCore.SendMessage(
                        new Beaver("lbss" , r.Source.ChiefName , r.Target.ChiefName , r.SkillName , Card.Cards2Beaver("cards"  , r.Cards )).ToString());
                        //new XElement("lbss",
                        //    new XElement("from", r.Source.ChiefName),
                        //    new XElement("to", r.Target.ChiefName),
                        //    new XElement("skill", r.SkillName),
                        //    Card.Cards2XML("cards", r.Cards)
                        //    )
                        //);
                }
            }
            return r;
        }
    }
}
