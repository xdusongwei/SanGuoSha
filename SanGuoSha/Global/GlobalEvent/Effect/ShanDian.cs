using SanGuoSha.Contest.Data;
using SanGuoSha.Contest.Equipage;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 安置闪电的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard ShanDianProc(EventRecoard r)
        {
            //玩家本人不能死亡
            if (!GamePlayers[r.Source].Dead)
            {
                //本人不能有闪电buff
                if (!GamePlayers[r.Source].HasDebuff(Card.Effect.ShanDian))
                {
                    //把闪电从垃圾桶中拿出来
                    PickRubbish(r.Cards);
                    //把闪电放置到自己的buff
                    GamePlayers[r.Source].Debuff.Push(r.Cards[0]);
                    AsynchronousCore.SendMessage(
                        new Beaver("sd" , r.Source.ChiefName , r.SkillName , Card.Cards2Beaver("cards" , r.Cards)).ToString());
                        //new XElement("sd",
                        //    new XElement("from", r.Source.ChiefName),
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
