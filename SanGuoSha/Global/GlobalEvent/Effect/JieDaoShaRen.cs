using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Data;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 借刀杀人的过程
        /// </summary>
        /// <param name="r">子事件的起始节点</param>
        /// <returns></returns>
        private EventRecoard JieDaoShaRenProc(EventRecoard r)
        {
            //玩家自己和两个目标不能死亡,目标不能没有武器
            if (!GamePlayers[r.Source].Dead && !GamePlayers[r.Target].Dead && !GamePlayers[r.Target2].Dead && GamePlayers[r.Target].Weapon != null)
            {
                //无懈可击
                if (WuXieProc(r.Target, Card.Effect.JieDaoShaRen)) return r;
                string msg = new Beaver("askfor.jdsr.sha", r.Target.ChiefName, r.Target2.ChiefName, r.Source.ChiefName).ToString();
                    //new XElement("askfor.jdsr.sha",
                    //    new XElement("target", r.Target.ChiefName),
                    //    new XElement("target2", r.Target2.ChiefName),
                    //    new XElement("source", r.Source.ChiefName)
                    //);
                //向目标问询杀
                MessageCore.AskForResult res = AsynchronousCore.AskForCards(r.Target, MessageCore.AskForEnum.Sha, new AskForWrapper(msg, this), gData);
                ValidityResult(r.Target, ref res);
                if (res.Effect == Card.Effect.None)
                {
                    //获得对方的武器对象
                    Card weapon = GamePlayers[r.Target].Weapon;
                    Move(r.Target, r.Source, [weapon]);
                    //玩家得到武器牌
                    AsynchronousCore.SendMessage(MessageCore.MakeStealMessage(r.Target, r.Source, [weapon]));
                        //new XElement("steal",
                        //    new XElement("from", r.Target.ChiefName),
                        //    new XElement("to", r.Source.ChiefName),
                        //    Card.Cards2XML("cards", new Card[] { weapon })
                        //    )
                        //);
                }
                else if (res.Effect == Card.Effect.Sha)
                {
                    AsynchronousCore.SendMessage(
                        new Beaver("sha", r.Target.ChiefName, ChiefBase.Chiefs2Beaver("to", [r.Target2]), res.SkillName, Card.Cards2Beaver("cards", res.Cards)).ToString());
                        //new XElement("sha",
                        //    new XElement("from", r.Target.ChiefName),
                        //    ChiefBase.Chiefs2XML("to", new ChiefBase[] { r.Target2 }),
                        //    new XElement("skill", res.SkillName),
                        //    Card.Cards2XML("cards", res.Cards)
                        //    )
                        //    );
                    //再此子事件上创建杀的子事件
                    DropCards(true, CardFrom.Hand, string.Empty, res.Cards, Card.Effect.Sha, r.Target, r.Target2, null);
                    //执行杀的子事件
                    ShaProc(lstRecoard.Last());// lstRecoard.ElementAt(lstRecoard.Count - 1)
                }
            }
            return r;
        }
    }
}
