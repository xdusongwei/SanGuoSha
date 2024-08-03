using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 开启逻辑循环
        /// </summary>
        /// <param name="aChiefStart">设置首个进入回合的武将</param>
        /// <param name="aIgnoreTakeCards">是否忽略一开始对玩家每人发4张牌的过程</param>
        protected void LogicLoop(ChiefBase aChiefStart, bool aIgnoreTakeCards)
        {
            AsynchronousCore.SendEnvironmentMessage();
            ChiefBase target = aChiefStart; //target , 问询的目标
            ChiefBase loop = aChiefStart;
            do
            {
                foreach (ASkill s in loop.Skills)
                    s.OnCreate(loop);
                loop.ReportSkills(gData);
                loop = loop.Next;
            } while (loop != target);

            if (!aIgnoreTakeCards)
            {
                loop = aChiefStart;
                //轮询给每个武将4张牌
                do
                {
                    TakeingCards(loop, 4);
                    loop = loop.Next;
                } while (loop != target);
            }
            RefereeProc();
            //游戏的轮询
            do
            {
                
                //复位游戏规则控制数据
                gData = new GlobalData();
                gData.Game = this;
                //设置执行回合的武将
                gData.Active = target;
                //若武将有武器,尝试让该武将的武器配置玩家的某些进攻性属性
                if (GamePlayers[target].Weapon != null)
                    Weapon.ActiveWeapon(GamePlayers[target].Weapon.CardEffect, gData);
                //改变武将状态-回合开始
                AsynchronousCore.SendChangeStatusMessage(target, MessageCore.PlayerStatus.Start);
                {
                    gData.ChiefStatus = "turnStart";
                    //通知该武将的技能该武将进入回合开始阶段
                    foreach (ASkill s in target.Skills)
                        s.BeforeTurnStart(target, gData);
                    //事件结束
                    ClearEventProc();
                }
                //改变武将状态-判定
                AsynchronousCore.SendChangeStatusMessage(target, MessageCore.PlayerStatus.Judgment);
                {
                    gData.ChiefStatus = "judgment";
                    //执行武将判定区的判定
                    Judgement(target, gData);
                    //事件结束
                    ClearEventProc();
                }
                foreach (ASkill s in target.Skills)
                    s.BeforeTakeCards(target, gData);
                if (gData.Take)
                {
                    //改变武将状态-拿牌
                    AsynchronousCore.SendChangeStatusMessage(target, MessageCore.PlayerStatus.Take);
                    {
                        gData.ChiefStatus = "take";
                        foreach (ASkill s in target.Skills)
                            s.TakingCards(target, gData);
                        //从牌堆取 gData.TakeCardsCount 张牌
                        Card[] ret = TakeingCards(target, gData.TakeCardsCount);
                        //事件结束
                        ClearEventProc();
                    }
                }
                //武将状态-出牌
                //要求 允许出牌且玩家未死亡
                if (gData.Lead && !GamePlayers[target].Dead)
                {
                    //改变武将状态-出牌
                    AsynchronousCore.SendChangeStatusMessage(target, MessageCore.PlayerStatus.Lead);
                    gData.ChiefStatus = "lead";
                    foreach (ASkill s in target.Skills)
                        s.Leading(target, gData);
                    //这里是一个循环,不断问询玩家出牌,若玩家Effect为None,就跳过这个阶段
                    while (!GamePlayers[target].Dead)
                    {
                        //重置全局数据中的活动部分
                        gData.Reset();

                        string msg = new Beaver("askfor.aggressive", target.ChiefName).ToString();
                            //new XElement("askfor.aggressive",
                            //    new XElement("target", target.ChiefName)
                            //);
                        //开始问询
                        MessageCore.AskForResult res = AsynchronousCore.AskForCards(target, MessageCore.AskForEnum.Aggressive, new AskForWrapper(msg, this), gData);
                        //是否跳过该阶段
                        if (res.Effect == Card.Effect.None)
                        {
                            AsynchronousCore.SendClearMessage();
                            break;
                        }
                        //出牌进行处理,并反馈是否符合规则
                        if (!LeadEvent(res))
                        {
                            ClearEventProc();
                            break;
                        }
                        else
                        {
                            ClearEventProc();
                        }
                    }

                }

                //在进入弃牌阶段前,通知武将的技能
                foreach (ASkill s in target.Skills)
                    s.BeforeAbandonment(target, gData);

                //允许武将进入弃牌阶段
                if (gData.Abandonment)
                {
                    //改变武将状态-弃牌
                    AsynchronousCore.SendChangeStatusMessage(target, MessageCore.PlayerStatus.Abandoment);
                    {
                        gData.ChiefStatus = "abandoment";
                        //玩家弃牌的问询
                        if (!Abandonment(target))
                        {
                            AsynchronousCore.LeadingInvalid(target);
                        }
                        else
                        {
                            AsynchronousCore.LeadingValid(target);
                        }
                        ClearEventProc();
                    }
                }
                //玩家回合结束前通知武将的技能
                foreach (ASkill s in target.Skills)
                    s.AfterTurnEnd(target, gData);
                //牌检查器报告情况
                AsynchronousCore.SendMessage(CardsHeap.CardsChecker(this));
            } while ((target = target.Next) != null); //target=下一个玩家

            //-----------------------------------------------------------
            //如果玩家都死光了...没分胜负,我想这里会有异常吧
        }
    }
}
