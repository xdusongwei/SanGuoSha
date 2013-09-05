using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SGS.ServerCore.Contest.Global;
using SGS.ServerCore.Contest.Data;

namespace SGS
{
    class Program
    {
        /// <summary>
        /// 自定义ICallback行为
        /// </summary>
        class MyCallback : SGS.ServerCore.Contest.Apusic.ICallback
        {
            /// <summary>
            /// 一个保存服务实例接口的属性,以便在回调调试中直接使用服务接口
            /// </summary>
            private SGS.ServerCore.Contest.Apusic.IService Service
            {
                get;
                set;
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="aService">服务实例的接口</param>
            public MyCallback(SGS.ServerCore.Contest.Apusic.IService aService)
            {
                Service = aService;
            }

            public void Message(string aText)
            {
                /* do some thing */
                /* Console.WriteLine(aText);*/
            }

            public void LeadingValid()
            {

            }

            public void LeadingInvalid(int[] aHand, int Weapon, int Armor, int Jia1, int Jian1)
            {

            }
        }

        static SGS.ServerCore.Contest.Global.Game g;

        static void Main(string[] args)
        {
            //创建一个游戏对象
            g = new Game(1000, GameBase.GameMode.FiveSTD, new GameBase.GamePack[] { });
            //对Create事件添加匿名委托
            g.OnCreate += new SGS.ServerCore.Contest.Global.Game.CreateDelegate((aHeap , aPlayers)=>
                {

                    //添加原版的卡牌,另外牌的号码保持从1开始连续,以免牌检查器报缺牌错误,游戏运行必须有至少9张牌
                    
                    aHeap.AddCards(new Card[] { new Card(1, Card.Suit.HeiTao, 1, Card.Effect.Sha) });
                    aHeap.AddCards(new Card[] { new Card(2, Card.Suit.HeiTao, 1, Card.Effect.Shan) });
                    aHeap.AddCards(new Card[] { new Card(3, Card.Suit.HeiTao, 1, Card.Effect.Shan) });
                    aHeap.AddCards(new Card[] { new Card(4, Card.Suit.HeiTao, 1, Card.Effect.Shan) });
                    aHeap.AddCards(new Card[] { new Card(5, Card.Suit.HeiTao, 1, Card.Effect.Shan) });
                    aHeap.AddCards(new Card[] { new Card(6, Card.Suit.HeiTao, 1, Card.Effect.Shan) });
                    aHeap.AddCards(new Card[] { new Card(7, Card.Suit.HeiTao, 1, Card.Effect.Shan) });
                    Card csha = new Card(8, Card.Suit.HeiTao, 1, Card.Effect.Sha); //这里添加一个杀
                    aHeap.AddCards(new Card[] { csha });
                    Card c = new Card(9, Card.Suit.HeiTao, 1, Card.Effect.BaGuaZhen); //这里添加一个八卦阵
                    aHeap.AddCards(new Card[] { c });
                    //最后设置当前牌数量
                    aHeap.TotalCards = 9;
                    //先把八卦阵这张牌从牌堆去掉
                    aHeap.PopCard(c.ID);
                    //把csha这张牌从牌堆中去掉
                    aHeap.PopCard(csha.ID);

                    //创建假定的服务实例
                    ServerCore.Contest.Apusic.ServiceCore server = new ServerCore.Contest.Apusic.ServiceCore(null, g);
                    //创建玩家对象
                    Player p = new Player(g, new MyCallback(server), server);
                    //让服务实例保存玩家对象
                    server.ThisPlayer = p;
                    //设置其武将为曹操
                    p.Chief = new CaoCao(aPlayers);
                    //把防具添加到玩家的防具中
                    p.Armor = c;
                    //把杀放到玩家的手牌中
                    p.Hands.Add(csha);

                    //创建假定的服务实例
                    server = new ServerCore.Contest.Apusic.ServiceCore(null, g);
                    //创建玩家对象
                    Player p2 = new Player(g, new MyCallback(server), server);
                    //让服务实例保存玩家对象
                    server.ThisPlayer = p2;
                    //设置玩家的武将为夏侯惇
                    p2.Chief = new XiaHouDun(aPlayers);
                    //将玩家添加到游戏中
                    aPlayers.AddPlayer(p);
                    aPlayers.AddPlayer(p2);
                }
            );
            //创建游戏
            g.Create();
            //从线程中启动游戏对象,也可以直接在主线程中启动(只要不干扰调试的话)

            Thread th = new Thread(() => { g.ActiveLogic(true); });
            th.Start();
            Console.ReadLine();
        }
    }
}
