using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Data;
using SGS.ServerCore.Contest.Global;
using SGS.ServerCore.Contest.Data.GameException;

namespace SGS.ServerCore.Contest.Apusic
{
    /// <summary>
    /// 服实例控制器,用于发送消息和问询,将异步信息同步到上层
    /// </summary>
    internal class ServiceController
    {
        #region 询问出牌
        public void AskForCards(GameBase aGame, Player aPlayer, SGS.ServerCore.Contest.Global.MessageCore.AskForEnum aAskFor, Player aTarget)
        {
            AskForCards(aGame, aPlayer, aAskFor, aTarget, null ,0);
        }

        public void AskForCards(GameBase aGame, Player aPlayer, SGS.ServerCore.Contest.Global.MessageCore.AskForEnum aAskFor)
        {
            AskForCards( aGame,  aPlayer,  aAskFor , null , null,0);
        }

        /// <summary>
        /// 开始异步问询通信
        /// </summary>
        /// <param name="aGame">游戏基类</param>
        /// <param name="aPlayer">问询的玩家对象</param>
        /// <param name="aAskFor">问询类型</param>
        /// <param name="aTarget">问询目标</param>
        /// <param name="aCount">问询的牌数量</param>
        /// <param name="aAbstentionTable">表决字典</param>
        public void AskForCards(GameBase aGame, Player aPlayer, SGS.ServerCore.Contest.Global.MessageCore.AskForEnum aAskFor, Player aTarget, Dictionary<Player, bool> aAbstentionTable , int aCount)
        {
            if (aGame.GamePlayers.All.Where(c => !c.IsEscaped && !c.IsOffline).Count() <2)
            {
                aGame.gData.Game.RefereeProc();
            }
            else if (aPlayer != null && (aPlayer.IsOffline || aPlayer.IsEscaped))
            {
                Thread.Sleep(1200);
                aGame.WaittingData.timeout = 0;
                aGame.Response.IsTimeout = true;
                aGame.Response.HasResponse = false;
                aGame.Response.Effect = Card.Effect.None;
                aGame.Response.Source = aPlayer;
                aGame.Response.Targets = new Player[0] { };
                aGame.Response.Cards = new Card[0] { };
                aGame.Response.SkillName = string.Empty;
                return;
            }

            if (aPlayer != null)
            {
                if (aPlayer.IsEscaped || aPlayer.IsOffline)
                {
                    Thread.Sleep(1500);
                    aGame.Response.Answer = false;
                    aGame.Response.Cards = new Card[] { };
                    aGame.Response.SkillName = string.Empty;
                    aGame.Response.Targets = new Player[] { };
                    aGame.Response.IsTimeout = false;
                    aGame.Response.HasResponse = true;
                    return;
                }
            }
            else if (aAbstentionTable.Where(c => c.Value == false).Count() == 0)
            {
                //Thread.Sleep(1500);
                aGame.Response.Answer = false;
                aGame.Response.Cards = new Card[] { };
                aGame.Response.SkillName = string.Empty;
                aGame.Response.Targets = new Player[] { };
                aGame.Response.IsTimeout = false;
                aGame.Response.HasResponse = true;
                return;
            }
            //创建等待进程
            Thread waitting = new Thread(WaittingProc);
            //访问临界锁
            lock (aGame.WaittingData.Achievelock)
            {
                //重置回应数据区
                aGame.Response.Reset();
                aGame.Response.Cards = new Card[0];
                aGame.Response.Effect = Card.Effect.None;
                //如果问询不是弃牌,那么倒数计时重置(因为弃牌可以多次,但时间总和必须在规定超时内)
                if (aAskFor != MessageCore.AskForEnum.AbandonmentNext)
                    aGame.WaittingData.timeout = aGame.WaittingData.Latency;
                else if (aGame.WaittingData.timeout == 0)
                    aGame.WaittingData.timeout = aGame.WaittingData.Latency;
                //如果剩余时间太小,就视作超时跳出方法
                else if (aGame.WaittingData.timeout < 10)
                {
                    aGame.WaittingData.timeout = 0;
                    aGame.Response.IsTimeout = true;
                    aGame.Response.HasResponse = false;
                    aGame.Response.Effect = Card.Effect.None;
                    aGame.Response.Source = aPlayer;
                    aGame.Response.SkillName = string.Empty;
                    return;
                }
                //弃牌问询如果剩余时间为0,重置剩余时间
                
                //将线程信息设置到专门的数据区中
                if (aAskFor == MessageCore.AskForEnum.AbandonmentNext)
                    aGame.WaittingData.AskFor = MessageCore.AskForEnum.Abandonment;
                else
                    aGame.WaittingData.AskFor = aAskFor;
                aGame.WaittingData.AskTo = aPlayer;
                aGame.WaittingData.ToEvery = aPlayer == null ? true : false;
                aGame.WaittingData.Enable = false;
                aGame.WaittingData.Target = aTarget;
                aGame.WaittingData.SynchronizationReady = false;
                aGame.WaittingData.WaittingThread = waitting;
                aGame.WaittingData.Abstention = aAbstentionTable;
                aGame.WaittingData.CardsCount = aCount;
            }//释放临界锁
            //开启线程
            waitting.Start(aGame.WaittingData);
            DateTime synStart = DateTime.Now;
            //等待响应
            while (true)
            {
                //抢临界锁
                lock (aGame.WaittingData.Achievelock)
                {
                    //如果线程回应了同步信号(等待线程进入了互斥信号),本线程继续运行后面的代码
                    if (aGame.WaittingData.SynchronizationReady)
                        break;
                }//释放临界锁
                Thread.Sleep(80);
                if ((DateTime.Now - synStart).TotalSeconds > 5) return;//throw new Exception("无法同步");
            }

            aGame.WaittingData.Enable = true;
            //主线程抢占互斥信号,被阻塞
            aGame.WaittingData.SynchronizationMutex.WaitOne();
            try
            {
                //尝试释放临界锁
                aGame.WaittingData.SynchronizationMutex.ReleaseMutex();
                aGame.WaittingData.SynchronizationMutex.Dispose();
                aGame.WaittingData.SynchronizationMutex = null;
            }
            catch
            {

            }

            //抢临界锁
            lock (aGame.WaittingData.Achievelock)
            {
                //置线程不可用
                aGame.WaittingData.Enable = false;
            }//释放临界锁
            if (aGame.GamePlayers.All.Where(c => !c.IsEscaped && !c.IsOffline).Count() < 2)
            {
                aGame.gData.Game.RefereeProc();
            }
        }

        /// <summary>
        /// 问询等待线程
        /// </summary>
        /// <param name="param">ThreadData对象</param>
        private static void WaittingProc(object param)
        {
            //计时器
            Stopwatch watch = new Stopwatch();
            //睡眠时间
            long sleep = 0;
            GameBase.ThreadData p = (GameBase.ThreadData)param;
            //进入互斥信号
            
            do
            {
                if (p.SynchronizationMutex != null)
                {
                    p.SynchronizationMutex.Dispose();
                    p.SynchronizationMutex = null;
                }
                p.SynchronizationMutex = new Mutex();
            }while(!p.SynchronizationMutex.WaitOne(new TimeSpan(1)));
            
            //抢临界锁
            lock (p.Achievelock)
            {
                //设置同步信号
                p.SynchronizationReady = true;
            }//释放临界锁
            try
            {
                //抢临界锁
                lock (p.Achievelock)
                {
                    //设置睡眠时间
                    sleep = p.timeout;
                }//释放临界锁
                //计时
                watch.Start();
                //睡眠
                Thread.Sleep((int)sleep);

                //超时
                //抢临界锁
                lock (p.Achievelock)
                {
                    //设置相关变量,置超时
                    p.timeout = 0;
                    p.Game.Response.HasResponse = false;
                    p.Game.Response.IsTimeout = true;
                    p.Game.WaittingData.Enable = false;
                }
            }
            catch
            {
                //异常
                watch.Stop();
                lock (p.Achievelock)
                {
                    if (p.AskFor == MessageCore.AskForEnum.Abandonment)
                    {
                        p.timeout -= watch.ElapsedMilliseconds - 1;
                        if (p.timeout < 0) p.timeout = 0;
                    }
                    else
                        p.timeout = 0;
                }
            }
            finally
            {
                //释放互斥信号,同步游戏的流程
                p.SynchronizationMutex.ReleaseMutex();
            }
        }

        //private class AskObject
        //{
        //    public readonly ICallback Call;
        //    public readonly AskForCore.AskForEnum Ask;
        //    public AskObject(ICallback aCall, AskForCore.AskForEnum aAsk)
        //    {
        //        Call = aCall;
        //        Ask = aAsk;
        //    }
        //}

        //private class AskWithTargetObject
        //{
        //    public readonly ICallback Call;
        //    public readonly AskForCore.AskForEnum Ask;
        //    public readonly Player Target;
        //    public AskWithTargetObject(ICallback aCall, AskForCore.AskForEnum aAsk, Player aTarget)
        //    {
        //        Call = aCall;
        //        Ask = aAsk;
        //        Target = aTarget;
        //    }
        //}

        //private class AskYN
        //{
        //    public readonly ICallback Call;
        //    public AskYN(ICallback aCall)
        //    {
        //        Call = aCall;
        //    }
        //}

        

        //private static void AskProc(object param)
        //{
        //    if (param is AskObject)
        //    {
        //        AskObject p = (AskObject)param;
        //        //p.Call.AskFor(p.Ask, null);
        //    }
        //    else if (param is AskWithTargetObject)
        //    {
        //        AskWithTargetObject p = (AskWithTargetObject)param;
        //        //p.Call.AskFor(p.Ask, p.Target.Chief.ChiefName);
        //    }
        //    else if (param is AskYN)
        //    {
        //        AskYN p = (AskYN)param;
        //        //p.Call.AskFor(AskForCore.AskForEnum.YN, null);
        //    }
        //}
        #endregion
        #region 消息
        //public void SendMessage(GameBase aGame , XElement aMessage)
        //{
        //    Console.WriteLine(aMessage.ToString());
        //    foreach (Player p in aGame.GamePlayers.All)
        //    {
        //        new Thread(MessageProc).Start(new PushMessage(p.Callback, aMessage));
        //    }
        //}

        public void SendMessage(Player[] aPlayers, string aMessage)
        {
            //Console.WriteLine(aMessage.ToString());
            foreach (Player p in aPlayers)
            {
                new Thread(MessageProc).Start(new PushMessage(p.Callback, aMessage));
            }
        }

        private class PushMessage
        {
            public readonly ICallback Call;
            public readonly string Message;
            public PushMessage(ICallback aCall, string aMessage)
            {
                Call = aCall;
                Message = aMessage;
            }
        }

        private static void MessageProc(object param )
        {
            if (param is PushMessage)
            {
                PushMessage p = (PushMessage)param;
                p.Call.Message(p.Message.ToString());
            }
        }
        #endregion
    }
}
