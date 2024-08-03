/*
 * GameBase.cs
 * Namespace: SanGuoSha.Contest.Global
 * GameBase定义了游戏基类，其中包含了游戏基本数据和游戏功能相关的对象
 * 在这个层面上,数据和扩展功能是这个类主要的描述任务
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanGuoSha.Contest.Data;
using System.Threading;
using SanGuoSha.Contest.Apusic;
//using System.Security.Cryptography;

namespace SanGuoSha.Contest.Global
{
    /// <summary>
    /// 游戏对象的基类
    /// </summary>
    public class GameBase : IDisposable
    {
        /// <summary>
        /// 游戏对象基类的构造函数
        /// </summary>
        /// <remarks></remarks>
        public GameBase()
        {
            //基类大部分的数据对象没有在构造函数中处理
            //这里给有关通信的部分做了引用
            CommService = new ServiceController();   //初始化服务控制器
            WaittingData = new ThreadData(this);     //初始化问询线程的数据区域
            Response = new ResponseData();           //初始化问询线程取的回应数据区域
            EventChain = []; //初始化游戏公共事件信息链
            //创建问询与消息对象
            AsynchronousCore = new MessageCore(this);
            GamePacks = Array.Empty<GamePack>();
            Mode = GameMode.FiveSTD;
            CardsHeap = new CardHeap(this); //初始化牌堆
            CardsBuffer = new SlotContainer(); //创建一个用于公共信息的牌槽

            //复位游戏规则控制数据
            gData = new GlobalData
            {
                Game = (this as GlobalEvent)!,
            };

            //设置牌槽
            CardsBuffer.Slots.Add(new Slot(WGFDSlotName, true, true)); //五谷丰登牌槽
        }

        /// <summary>
        /// 释放对象的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //if (_random != null)
                    //{
                    //    _random.Dispose();
                    //    _random = null;
                    //}
                    WaittingData.Dispose();

                    if (CardsHeap != null)
                    {
                        CardsHeap.Dispose();
                        CardsHeap = null;
                    }
                }
                disposed = true;
            }
        }


        /// <summary>
        /// 对象的销毁方法
        /// </summary>
        ~GameBase()
        {
            Dispose(false);
            Console.WriteLine("GameBase destroy");
        }

        /// <summary>
        /// 游戏事件所需的牌槽容器
        /// </summary>
        internal SlotContainer CardsBuffer
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置弃牌堆
        /// </summary>
        public CardHeap CardsHeap
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏数据结构
        /// </summary>
        internal GlobalData gData = null; 
       
        /// <summary>
        /// 玩家集合
        /// </summary>
        public Players GamePlayers = new Players();

        /// <summary>
        /// 获取通信对象属性
        /// 用于将同步请求转化为异步请求发送到指定回调对象，以及将结果的异步请求转化为同步请求
        /// </summary>
        internal ServiceController CommService
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 问询线程的数据类
        /// 同时也是为通信回应的合法性检查保存简单依据
        /// </summary>
        internal class ThreadData :IDisposable
        {
            /// <summary>
            /// 线程可以控制的互斥信号
            /// </summary>
            public Mutex SynchronizationMutex;
            /// <summary>
            /// 线程访问ThreadData以及ResponseData所要使用的同步锁
            /// </summary>
            public readonly object Achievelock;
            /// <summary>
            /// 用于记录问询线程同步锁是否已锁
            /// </summary>
            public bool SynchronizationReady;
            /// <summary>
            /// 问询线程对象
            /// </summary>
            public Thread WaittingThread;
            /// <summary>
            /// 线程问询的内容
            /// </summary>
            public SanGuoSha.Contest.Global.MessageCore.AskForEnum AskFor;
            /// <summary>
            /// 线程问询的对方,如果是所有人这里应为null
            /// </summary>
            public Player AskTo;                       
            /// <summary>
            /// 是否问询所有活着的玩家
            /// </summary>
            public bool ToEvery;
            /// <summary>
            /// 线程的等待时间,毫秒,超过算作超时
            /// </summary>
            public long timeout;
            /// <summary>
            /// 游戏基类的引用
            /// </summary>
            public readonly GameBase Game;
            /// <summary>
            /// 问询状态是否有效
            /// </summary>
            public bool Enable;
            /// <summary>
            /// 问询附加上的目标玩家参数
            /// </summary>
            public Player Target;

            /// <summary>
            /// 否决表,用来记录放弃无懈可击的玩家
            /// </summary>
            public Dictionary<Player, bool> Abstention;
            /// <summary>
            /// 问询最大时间
            /// </summary>
            public volatile int Latency;

            /// <summary>
            /// 问询的牌数量，用于问询选择多张牌的情形
            /// </summary>
            public int CardsCount;

            /// <summary>
            /// 线程数据的构造函数
            /// </summary>
            /// <param name="aGame">游戏的基类</param>
            public ThreadData(GameBase aGame)
            {
                Game = aGame;
                Enable = false;
                SynchronizationMutex = new Mutex();
                SynchronizationReady = false;
                Achievelock = new object();
                AskFor = MessageCore.AskForEnum.Sha;
                ToEvery = false;
                AskTo = null;
                timeout = 100;
                WaittingThread = null;
                Target = null;
                Abstention = null;
                CardsCount = 1;
            }

            public void Dispose()
            {
                if (SynchronizationMutex != null)
                {
                    try
                    {
                        SynchronizationMutex.Close();
                        SynchronizationMutex = null;
                    }
                    catch
                    {

                    }
                }
            }

            ~ThreadData()
            {
                Dispose();
            }
        }

        /// <summary>
        /// 问询的回复结果类，存放问询的结果。通常的，此类的对象结果不直接参与游戏控制，而需要在MessageCore中被MessageCore.AskForResult对象转化
        /// </summary>
        internal class ResponseData : System.Object 
        {
            /// <summary>
            /// 获得浅复制的对象。在多重问询的情形下，保存当前问询结果是很重要的，这样可以防止后面的问询将回应结果破坏而无法继续当前的信息处理
            /// </summary>
            /// <returns>复制的对象</returns>
            public ResponseData Copy()
            {
                return MemberwiseClone() as ResponseData;
            }

            public ResponseData()
            {
                HasResponse = false;
                IsTimeout = false;
                SkillName = string.Empty;
                Cards = [];
                Answer = false;
            }

            public bool HasResponse
            {
                get;
                set;
            }

            /// <summary>
            /// 设置或者获取问询结果是否超时
            /// </summary>
            public bool IsTimeout
            {
                get;
                set;
            }

            /// <summary>
            /// 设置或者获取回应中使用的技能名称
            /// </summary>
            public string SkillName
            {
                get;
                set;
            }

            /// <summary>
            /// 设置或者获取回应中使用的牌
            /// </summary>
            public Card[] Cards
            {
                get;
                set;
            }

            /// <summary>
            /// 设置或者获取回应中的应答是否问询的结果
            /// </summary>
            public bool Answer
            {
                get;
                set;
            }

            /// <summary>
            /// 设置或者获取回应中选择的玩家对象
            /// </summary>
            public Player[] Targets
            {
                get;
                set;
            }

            /// <summary>
            /// 异步层判断出的回应效果
            /// </summary>
            public Card.Effect Effect;

            /// <summary>
            /// 问询的玩家
            /// </summary>
            public Player Source
            {
                get;
                set;
            }

            /// <summary>
            /// 重置问询结果中的部分成员
            /// </summary>
            public void Reset()
            {
                HasResponse = false;
                IsTimeout = false;
            }
        }

        /// <summary>
        /// 问询线程的数据。
        /// </summary>
        internal ThreadData WaittingData
        {
            get;
            set;
        }

        /// <summary>
        /// 问询回应的数据
        /// </summary>
        internal ResponseData Response
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏的公共事件链，公共事件即排除了私有XML消息的事件集合，用于给观看者提供初始数据的数据依据，注意，由于一般情况下使用公共事件链通常是信息缺失造成的，所以，此链首元素是一个关于全局玩家情况的XML信息
        /// </summary>
        internal List<string> EventChain
        {
            get;
            private set;
        }

        /// <summary>
        /// 访问或修改公共事件链的锁
        /// </summary>
        internal object EventChainLocker = new object();

        /// <summary>
        /// 获取或设置问询与消息对象
        /// </summary>
        internal MessageCore AsynchronousCore
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏中允许使用的包
        /// </summary>
        public enum GamePack { 
            /// <summary>
            /// 原版，通常不使用，因为不考虑这个枚举值
            /// </summary>
            Origin, 
            /// <summary>
            /// 风包
            /// </summary>
            Feng, 
            /// <summary>
            /// 火包
            /// </summary>
            Huo, 
            /// <summary>
            /// 林包
            /// </summary>
            Lin };
        /// <summary>
        /// 游戏模式
        /// </summary>
        public enum GameMode { 
            /// <summary>
            /// 五人标准
            /// </summary>
            FiveSTD, 
            /// <summary>
            /// 五人军争
            /// </summary>
            FiveJunZheng, 
            /// <summary>
            /// 八人标准
            /// </summary>
            EightSTD, 
            /// <summary>
            /// 八人军争
            /// </summary>
            EightJunZheng, 
            /// <summary>
            /// 八人双内
            /// </summary>
            EightShuangNei };

        /// <summary>
        /// 当前游戏的游戏扩展包
        /// </summary>
        public GamePack[] GamePacks
        {
            get;
            protected set;
        }

        /// <summary>
        /// 当前游戏的模式
        /// </summary>
        public GameMode Mode
        {
            get;
            protected set;
        }

        /// <summary>
        /// 五谷丰登牌槽名称
        /// </summary>
        internal readonly string WGFDSlotName = "WGFD";


        /// <summary>
        /// 一个对列表对象进行乱序排列的方法
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="aList">列表对象</param>
        /// <returns>乱序列表</returns>
        internal List<T> ShuffleList<T>(List<T> aList)
        {
            List<T> ret = [];
            foreach (T item in aList)
            {
                ret.Insert(GetRandom(ret.Count + 1), item);
            }
            return ret;
        }

        /// <summary>
        /// 一个随机数产生对象
        /// </summary>
        //private RNGCryptoServiceProvider _random;
        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <param name="mod">模</param>
        /// <returns>[ 0 , mod ) 内任意整数</returns>
        internal int GetRandom(int mod)
        {
            Guid id = Guid.NewGuid();
            byte[] arr = id.ToByteArray();
            int ret = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (i % 4 == 3)
                {
                    int mask = 0;
                    mask = (mask << 8) | arr[i];
                    mask = (mask << 8) | arr[i-1];
                    mask = (mask << 8) | arr[i-2];
                    mask = (mask << 8) | arr[i-3];
                    ret = ret ^ mask;
                }
            }
            ret = Math.Abs(ret);
            return ret % mod;
            //if (_random == null) _random = new RNGCryptoServiceProvider();
            //int max = mod;
            //int rnd = int.MinValue;
            //decimal _base = (decimal)long.MaxValue;
            //byte[] rndSeries = new byte[8];
            //_random.GetBytes(rndSeries);
            //rnd = (int)(Math.Abs(BitConverter.ToInt64(rndSeries, 0)) / _base * (max));
            //return rnd;
        }
    }
}
