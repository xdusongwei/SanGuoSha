using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 开始玩家的选将过程，要求玩家数量必须等于游戏模式中指定的数量
        /// </summary>
        /// <returns>若该过程无误，则返回true</returns>
        public bool SelectChiefs()
        {
            if (GamePlayers.All.Count() < 2) return false;
            foreach (Player p in GamePlayers.All)
            {
                p.Chief = null;
                p.AvailableChiefs = new ChiefBase[0] { };
            }
            //发送游戏环境数据给所有玩家
            AsynchronousCore.SendEnvironmentMessage();
            int m = 0;
            int l1 = 0;
            int l2 = 0;
            int i1 = 0;
            int i2 = 0;
            int i3 = 0;
            int i4 = 0;
            int s1 = 0;
            int s2 = 0;
            //获取场上玩家数量
            int max = GamePlayers.All.Count();
            //重置牌堆
            CardsHeap = new CardHeap(this);
            //一个用来放置可选武将的列表
            List<ChiefBase> heap = new List<ChiefBase>();
            //根据模式设置牌堆里面的牌
            switch (Mode)
            {
                case GameMode.FiveJunZheng:
                case GameMode.EightJunZheng:
                    CardsHeap.FillOriginCards();
                    CardsHeap.FillExCards();
                    CardsHeap.FillShenCards();
                    break;
                case GameMode.FiveSTD:
                case GameMode.EightSTD:
                    CardsHeap.FillOriginCards();
                    CardsHeap.FillExCards();
                    break;
            }
            //根据扩展包情况加入武将
            heap = ChiefHeap.GetOriginChiefs();
            //主公可选武将列表
            List<ChiefBase> mLst = new List<ChiefBase>();

            mLst.AddRange(heap.Where(c => c.ChiefName == "刘备" || c.ChiefName == "孙权" || c.ChiefName == "曹操"));
            if (GamePacks.Contains(GamePack.Feng))
            {
                heap.AddRange(ChiefHeap.GetFengPackChiefs());
                mLst.AddRange(heap.Where(c => c.ChiefName == "张角"));
            }
            if (GamePacks.Contains(GamePack.Huo))
            {
                heap.AddRange(ChiefHeap.GetHuoPackChiefs());
                mLst.AddRange(heap.Where(c => c.ChiefName == "袁绍"));
            }
            if (GamePacks.Contains(GamePack.Lin))
            {
                heap.AddRange(ChiefHeap.GetLinPackChiefs());
                mLst.AddRange(heap.Where(c => c.ChiefName == "董卓"));
            }
            //乱序可选武将
            heap = ShuffleList<ChiefBase>(heap);
            //将非主公武将从heap中找到放置到heap2
            ChiefBase[] heap2 = heap.Where(c => c.ChiefName != "刘备" && c.ChiefName != "孙权" && c.ChiefName != "曹操" && c.ChiefName != "张角" && c.ChiefName != "袁绍" && c.ChiefName != "董卓").ToArray();
            //加入两个非主公武将到主公可选武将列表中
            mLst.Add(heap2[0]);
            mLst.Add(heap2[1]);
            //把主公可选武将写成XML格式
            Beaver r = null;
            foreach (ChiefBase c in mLst)
                if (r == null)
                {
                    r = new Beaver("chiefs", c.ChiefName);
                }
                else
                {
                    r.Add(string.Empty, c.ChiefName);
                }
            if (r == null)
                r = new Beaver("chiefs");
            //表决字典
            Dictionary<Player, bool> Abstention = new Dictionary<Player, bool>();
            //这个列表用来产生身份序列
            List<int> l = null;
            //按照人数开始配置角色
            switch (Mode)
            {
                case GameMode.FiveSTD:
                case GameMode.FiveJunZheng:
                    //五人场人数必须为5
                    if (GamePlayers.All.Count() != 5) return false;
                    l = new List<int>(new int[] { 0, 1, 2, 3, 4 });
                    l = ShuffleList<int>(l);
                    m = l[0];   //主
                    l1 = l[1];  //忠
                    s1 = l[2];  //内
                    i1 = l[3];  //反
                    i2 = l[4];  //反
                    //给所有玩家群发主公是谁
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[m] }, new Beaver("ChiefStatus", ChiefBase.Status.Majesty.ToString(), GamePlayers[m].UID).ToString(), true);
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Majesty),
                        //    new XElement("UID", GamePlayers[m].UID)
                        //    ), true);
                    //给其他角色发送自己身份是什么
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[l1] }, new Beaver("ChiefStatus", ChiefBase.Status.Loyalist.ToString(), GamePlayers[l1].UID).ToString(), false);
                        //new Player[] { GamePlayers[l1] },
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Loyalist),
                        //    new XElement("UID", GamePlayers[l1].UID)
                        //    ), false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[s1] }, new Beaver("ChiefStatus", ChiefBase.Status.Spy.ToString(), GamePlayers[s1].UID).ToString(), false);
                        //new Player[] { GamePlayers[s1] },
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Spy),
                        //    new XElement("UID", GamePlayers[s1].UID)
                        //    ), false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[i1] }, new Beaver("ChiefStatus", ChiefBase.Status.Insurgent.ToString(), GamePlayers[i1].UID).ToString(), false);
                        //new Player[] { GamePlayers[i1] },
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Insurgent),
                        //    new XElement("UID", GamePlayers[i1].UID)
                        //    ), false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[i2] }, new Beaver("ChiefStatus", ChiefBase.Status.Insurgent.ToString(), GamePlayers[i2].UID).ToString(), false);
                        //new Player[] { GamePlayers[i2] },
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Insurgent),
                        //    new XElement("UID", GamePlayers[i2].UID)
                        //    ), false);
                    //设置主公可选武将列表
                    GamePlayers[m].AvailableChiefs = mLst.ToArray();
                    //通知主公选武将
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[m] },
                        new Beaver("askfor.selectchief", GamePlayers[m].UID, r).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[m].UID),
                        //    r)
                        , false);
                    //通知其他玩家主公正在选武将
                    AsynchronousCore.SendMessage(
                        GamePlayers.All.Where(c => c.UID != GamePlayers[m].UID).ToArray(),
                        new Beaver("askfor.selectchief", GamePlayers[m].UID).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[m].UID))
                        , true);

                    //设置表决字典,除了主公玩家置false，其他玩家置true
                    foreach (Player p in GamePlayers.All)
                        if (GamePlayers[m] == p && !p.IsEscaped && !p.IsOffline)
                            Abstention.Add(p, false);
                        else
                            Abstention.Add(p, true);
                    
                    //等待问询结果
                    AsynchronousCore.AskForChief(Abstention);
                    //主公没选择武将的话，随机配置一个
                    if (GamePlayers[m].Chief == null) GamePlayers[m].Chief = mLst[GetRandom(mLst.Count)];
                    //设置主公的一些属性
                    GamePlayers[m].Chief.ChiefStatus = ChiefBase.Status.Majesty;
                    GamePlayers[m].Chief.playersObject = GamePlayers;
                    GamePlayers[m].MaxHealth = GamePlayers[m].Chief.Health;
                    GamePlayers[m].MaxHealth++;
                    GamePlayers[m].Health = GamePlayers[m].MaxHealth;
                    //通知玩家主公选择了谁
                    AsynchronousCore.SendMessage(
                        new Beaver("selectchief", GamePlayers[m].UID ,GamePlayers[m].Chief.ChiefName , GamePlayers[m].MaxHealth.ToString()).ToString());
                        //new XElement("selectchief",
                        //    new XElement("UID", GamePlayers[m].UID),
                        //    new XElement("chief_name", GamePlayers[m].Chief.ChiefName),
                        //    new XElement("max_health", GamePlayers[m].MaxHealth)
                        //    )
                        //);
                    //heap中去除掉主公选择的武将并重新乱序排列
                    heap = ShuffleList<ChiefBase>(heap.Where(c => c != GamePlayers[m].Chief).ToList());
                    //其他玩家每人抽三个武将到自己的可选武将中
                    GamePlayers[l1].AvailableChiefs = heap.GetRange(0, 3).ToArray();
                    GamePlayers[s1].AvailableChiefs = heap.GetRange(3, 3).ToArray();
                    GamePlayers[i1].AvailableChiefs = heap.GetRange(6, 3).ToArray();
                    GamePlayers[i2].AvailableChiefs = heap.GetRange(9, 3).ToArray();
                    //重新设置表决字典,除了主公,其他人都置false
                    Abstention = new Dictionary<Player, bool>();
                    foreach (Player p in GamePlayers.All)
                        if (GamePlayers[m] == p)
                            Abstention.Add(p, true);
                        else
                            Abstention.Add(p, false);
                    //通知非主公玩家去选自己的武将
                    r = Chiefs2Beaver(l1);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[l1] },
                        new Beaver("askfor.selectchief",GamePlayers[l1].UID,r).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[l1].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(s1);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[s1] },
                        new Beaver("askfor.selectchief", GamePlayers[s1].UID ,r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[s1].UID),
                        //    new XElement("chiefs",
                        //        new XElement("chief", GamePlayers[s1].AvailableChiefs[0].ChiefName),
                        //        new XElement("chief", GamePlayers[s1].AvailableChiefs[1].ChiefName),
                        //        new XElement("chief", GamePlayers[s1].AvailableChiefs[2].ChiefName)
                        //        ))
                        , false);
                    r = Chiefs2Beaver(i1);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[i1] },
                         new Beaver("askfor.selectchief", GamePlayers[i1].UID ,r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[i1].UID),
                        //    new XElement("chiefs",
                        //        new XElement("chief", GamePlayers[i1].AvailableChiefs[0].ChiefName),
                        //        new XElement("chief", GamePlayers[i1].AvailableChiefs[1].ChiefName),
                        //        new XElement("chief", GamePlayers[i1].AvailableChiefs[2].ChiefName)
                        //        ))
                        , false);
                    r = Chiefs2Beaver(i2);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[i2] },
                        new Beaver("askfor.selectchief", GamePlayers[i2].UID ,r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[i2].UID),
                        //    new XElement("chiefs",
                        //        new XElement("chief", GamePlayers[i2].AvailableChiefs[0].ChiefName),
                        //        new XElement("chief", GamePlayers[i2].AvailableChiefs[1].ChiefName),
                        //        new XElement("chief", GamePlayers[i2].AvailableChiefs[2].ChiefName)
                        //        ))
                        , false);
                    //问询玩家选择武将
                    AsynchronousCore.AskForChief(Abstention);
                    //针对非主公玩家配置数据
                    foreach (Player p in GamePlayers.All)
                    {

                        p.Score = 0;
                        if (GamePlayers[m] == p) continue;
                        if (p.Chief == null)
                        {
                            //没选武将随机配置一个
                            p.Chief = p.AvailableChiefs[GetRandom(p.AvailableChiefs.Count())];
                        }
                        p.Chief.playersObject = GamePlayers;
                        p.MaxHealth = p.Chief.Health;
                        p.Health = p.MaxHealth;
                        //通知所有玩家该玩家选择的武将
                        AsynchronousCore.SendMessage(
                            new Beaver("selectchief",p.UID , p.Chief.ChiefName , p.MaxHealth).ToString()
                        //new XElement("selectchief",
                        //    new XElement("UID", p.UID),
                        //    new XElement("chief_name", p.Chief.ChiefName),
                        //    new XElement("max_health", p.MaxHealth)
                        //    )
                        );
                    }
                    ////开始分配武将的技能
                    //foreach (Player p in GamePlayers.All)
                    //{
                    //    //再发送出去
                    //    p.Chief.ReportSkills(gData);
                    //}
                    //设置非主公武将的身份
                    GamePlayers[l1].Chief.ChiefStatus = Data.ChiefBase.Status.Loyalist;
                    GamePlayers[s1].Chief.ChiefStatus = Data.ChiefBase.Status.Spy;
                    GamePlayers[i1].Chief.ChiefStatus = Data.ChiefBase.Status.Insurgent;
                    GamePlayers[i2].Chief.ChiefStatus = Data.ChiefBase.Status.Insurgent;
                    //事件结束
                    AsynchronousCore.SendClearMessage();
                    return true;
                case GameMode.EightSTD:
                case GameMode.EightJunZheng:
                    //玩家人数不为8则不能执行
                    if (GamePlayers.All.Count() != 8) return false;
                    l = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                    l = ShuffleList<int>(l);
                    m = l[0];   //主
                    l1 = l[1];  //忠
                    l2 = l[2];  //忠
                    s1 = l[3];  //内
                    i1 = l[4];  //反
                    i2 = l[5];  //反
                    i3 = l[6];  //反
                    i4 = l[7];  //反
                    //给所有玩家通知主公是谁
                    AsynchronousCore.SendMessage(
                        new Beaver("ChiefStatus", ChiefBase.Status.Majesty.ToString() , GamePlayers[m].UID).ToString());
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Majesty),
                        //    new XElement("UID", GamePlayers[m].UID)
                        //    ));
                    //其他玩家自己收到自己的身份
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[l1] },
                        new Beaver("ChiefStatus",ChiefBase.Status.Loyalist.ToString() , GamePlayers[l1].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Loyalist),
                        //    new XElement("UID", GamePlayers[l1].UID))
                            , false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[l2] },
                        new Beaver("ChiefStatus",ChiefBase.Status.Loyalist.ToString() , GamePlayers[l2].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Loyalist),
                        //    new XElement("UID", GamePlayers[l2].UID))
                            , false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[s1] },
                        new Beaver("ChiefStatus", ChiefBase.Status.Spy.ToString() , GamePlayers[s1].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Spy),
                        //    new XElement("UID", GamePlayers[s1].UID))
                            , false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[i1] },
                        new Beaver("ChiefStatus", ChiefBase.Status.Insurgent.ToString() , GamePlayers[i1].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Insurgent),
                        //    new XElement("UID", GamePlayers[i1].UID))
                            , false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[i2] },
                        new Beaver("ChiefStatus", ChiefBase.Status.Insurgent.ToString() , GamePlayers[i2].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Insurgent),
                        //    new XElement("UID", GamePlayers[i2].UID))
                            , false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[i3] },
                        new Beaver("ChiefStatus", ChiefBase.Status.Insurgent.ToString() , GamePlayers[i3].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Insurgent),
                        //    new XElement("UID", GamePlayers[i3].UID))
                            , false);
                    AsynchronousCore.SendMessage(new Player[] { GamePlayers[i4] },
                        new Beaver("ChiefStatus", ChiefBase.Status.Insurgent.ToString() , GamePlayers[i4].UID).ToString()
                        //new XElement("ChiefStatus",
                        //    new XElement("Status", ChiefBase.Status.Insurgent),
                        //    new XElement("UID", GamePlayers[i4].UID))
                            , false);
                    //设置主公可选的武将列表
                    GamePlayers[m].AvailableChiefs = mLst.ToArray();
                    //给主公私下发送的选将事件
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[m] },
                        new Beaver("askfor.selectchief",GamePlayers[m].UID ,r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[m].UID),
                        //    r)
                        , false);
                    //给所有非主公玩家通知主公在选将
                    AsynchronousCore.SendMessage(
                        GamePlayers.All.Where(c => c.UID != GamePlayers[m].UID).ToArray(),
                        new Beaver("askfor.selectchief",GamePlayers[m].UID).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[m].UID))
                        , true);
                    //设置表决字典,让主公玩家未表决
                    Abstention = new Dictionary<Player, bool>();
                    foreach (Player p in GamePlayers.All)
                        if (GamePlayers[m] == p)
                            Abstention.Add(p, false);
                        else
                            Abstention.Add(p, true);
                    //开始问询
                    AsynchronousCore.AskForChief(Abstention);
                    //没有选将设置随机选择武将
                    if (GamePlayers[m].Chief == null) GamePlayers[m].Chief = mLst[GetRandom(mLst.Count)];
                    //设置主公玩家的一些属性
                    GamePlayers[m].Chief.ChiefStatus = ChiefBase.Status.Majesty;
                    GamePlayers[m].Chief.playersObject = GamePlayers;
                    GamePlayers[m].MaxHealth = GamePlayers[m].Chief.Health;
                    GamePlayers[m].MaxHealth++;
                    GamePlayers[m].Health = GamePlayers[m].MaxHealth;
                    //通知玩家主公选择了什么以及血量
                    AsynchronousCore.SendMessage(
                        new Beaver("selectchief", GamePlayers[m].UID, GamePlayers[m].Chief.ChiefName, GamePlayers[m].MaxHealth.ToString()).ToString());
                        //new XElement("selectchief",
                        //    new XElement("UID", GamePlayers[m].UID),
                        //    new XElement("chief_name", GamePlayers[m].Chief.ChiefName),
                        //    new XElement("max_health", GamePlayers[m].MaxHealth)
                        //    )
                        //);
                    //武将堆排除掉主公选择的武将之后重新排序
                    heap = ShuffleList<ChiefBase>(heap.Where(c => c != GamePlayers[m].Chief).ToList());
                    //其他玩家从heap抽3个武将作为可选武将
                    GamePlayers[l1].AvailableChiefs = heap.GetRange(0, 3).ToArray();
                    GamePlayers[l2].AvailableChiefs = heap.GetRange(3, 3).ToArray();
                    GamePlayers[s1].AvailableChiefs = heap.GetRange(6, 3).ToArray();
                    GamePlayers[i1].AvailableChiefs = heap.GetRange(9, 3).ToArray();
                    GamePlayers[i2].AvailableChiefs = heap.GetRange(12, 3).ToArray();
                    GamePlayers[i3].AvailableChiefs = heap.GetRange(15, 3).ToArray();
                    GamePlayers[i4].AvailableChiefs = heap.GetRange(18, 3).ToArray();
                    //设置表决字典,让非主公玩家选将
                    Abstention = new Dictionary<Player, bool>();
                    foreach (Player p in GamePlayers.All)
                        if (GamePlayers[m] == p)
                            Abstention.Add(p, true);
                        else
                            Abstention.Add(p, false);
                    AsynchronousCore.SendMessage(
                        new Beaver("askfor.selectchief").ToString());
                        //new XElement("askfor.selectchief")
                        //);
                    r = Chiefs2Beaver(l1);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[l1] },
                        new Beaver("askfor.selectchief",GamePlayers[l1].UID ,r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[l1].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(l2);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[l2] },
                        new Beaver("askfor.selectchief",GamePlayers[l2].UID ,r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[l2].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(s1);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[s1] },
                        new Beaver("askfor.selectchief",GamePlayers[s1].UID ,r).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[s1].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(i1);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[i1] },
                        new Beaver("askfor.selectchief", GamePlayers[i1].UID , r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[i1].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(i2);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[i2] },
                        new Beaver("askfor.selectchief", GamePlayers[i2].UID , r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[i2].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(i3);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[i3] },
                        new Beaver("askfor.selectchief", GamePlayers[i3].UID , r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[i3].UID),
                        //    r)
                        , false);
                    r = Chiefs2Beaver(i4);
                    AsynchronousCore.SendMessage(
                        new Player[] { GamePlayers[i4] },
                        new Beaver("askfor.selectchief", GamePlayers[i4].UID , r ).ToString()
                        //new XElement("askfor.selectchief",
                        //    new XElement("UID", GamePlayers[i4].UID),
                        //    r)
                        , false);
                    AsynchronousCore.AskForChief(Abstention);
                    foreach (Player p in GamePlayers.All)
                    {
                        p.Score = 0;
                        if (GamePlayers[m] == p) continue;
                        if (p.Chief == null)
                        {
                            p.Chief = p.AvailableChiefs[GetRandom(p.AvailableChiefs.Count())];
                        }
                        p.Chief.playersObject = GamePlayers;
                        p.MaxHealth = p.Chief.Health;
                        p.Health = p.MaxHealth;
                        p.Dead = false;
                        AsynchronousCore.SendMessage(
                            new Beaver("selectchief", p.UID , p.Chief.ChiefName , p.MaxHealth.ToString()).ToString());
                        //new XElement("selectchief",
                        //    new XElement("UID", p.UID),
                        //    new XElement("chief_name", p.Chief.ChiefName),
                        //    new XElement("max_health", p.MaxHealth)
                        //    )
                        //);
                    }
                    ////开始分配武将的技能
                    //foreach (Player p in GamePlayers.All)
                    //{
                    //    //再发送出去
                    //    p.Chief.ReportSkills(gData);
                    //}
                    GamePlayers[l1].Chief.ChiefStatus = Data.ChiefBase.Status.Loyalist;
                    GamePlayers[l2].Chief.ChiefStatus = ChiefBase.Status.Loyalist;
                    GamePlayers[s1].Chief.ChiefStatus = Data.ChiefBase.Status.Spy;
                    GamePlayers[i1].Chief.ChiefStatus = Data.ChiefBase.Status.Insurgent;
                    GamePlayers[i2].Chief.ChiefStatus = Data.ChiefBase.Status.Insurgent;
                    GamePlayers[i3].Chief.ChiefStatus = Data.ChiefBase.Status.Insurgent;
                    GamePlayers[i4].Chief.ChiefStatus = Data.ChiefBase.Status.Insurgent;
                    AsynchronousCore.SendClearMessage();
                    return true;
            }
            return false;

        }
    }
}
