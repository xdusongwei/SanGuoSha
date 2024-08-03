/*
 * Player Players
 * Namespace SanGuoSha.ServerCore.Contest.Data
 * 玩家和玩家集合的定义
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanGuoSha.ServerCore.Contest.Global;
using SanGuoSha.ServerCore.Contest.Apusic;
using System.Xml.Linq;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Data
{
    /// <summary>
    /// 玩家
    /// </summary>
    public class Player
    {
        #region 数据区
        /// <summary>
        /// 玩家的分数
        /// </summary>
        public int Score
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家可选的武将
        /// </summary>
        public ChiefBase[] AvailableChiefs
        {
            get;
            set;
        }

        /// <summary>
        /// 这是一个标志,用于判断玩家是否逃跑
        /// </summary>
        private volatile bool _IsEscaped = false;

        /// <summary>
        /// 这是一个标志,用于判断玩家是否离开
        /// </summary>
        private volatile bool _IsOffline = false;


        public bool IsOffline
        {
            get
            {
                return _IsOffline;
            }
            set
            {
                _IsOffline = value;
            }
        }

        public bool IsEscaped
        {
            get
            {
                return _IsEscaped;
            }
            set
            {
                _IsEscaped = value;
            }
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public string UID;
        /// <summary>
        /// 玩家的名称
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// 玩家的事件链
        /// </summary>
        public List<string> Messages
        {
            get;
            private set;
        }

        /// <summary>
        /// 玩家武将的血量
        /// </summary>
        private sbyte _Health;
        /// <summary>
        /// 玩家武将的血量
        /// </summary>
        public sbyte Health
        {
            get
            {
                return _Health;
            }

            set
            {
                if (value > MaxHealth)
                    _Health = MaxHealth;
                else if (value < 0)
                    _Health = 0;
                else
                    _Health = value;
            }
        }

        /// <summary>
        /// 玩家武将的最大血量
        /// </summary>
        public sbyte MaxHealth
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家武将是否死亡
        /// </summary>
        public bool Dead
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家判定区的牌
        /// </summary>
        public Stack<Card> Debuff
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家的武将
        /// </summary>
        public ChiefBase Chief
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家的手牌
        /// </summary>
        public List<Card> Hands
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家武将的武器
        /// </summary>
        public Card Weapon
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家武将的护甲
        /// </summary>
        public Card Armor
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家武将的+1马
        /// </summary>
        public Card Jia1Ma
        {
            get;
            set;
        }

        /// <summary>
        /// 玩家武将的-1马
        /// </summary>
        public Card Jian1Ma
        {
            get;
            set;
        }

        /// <summary>
        /// 判断玩家是否有牌,计算范围包括手牌,装备和判定区
        /// </summary>
        public bool HasCardWithJudgementArea
        {
            get
            {
                if (Hands.Count > 0) return true;
                else if (Debuff.Count > 0) return true;
                else if (Weapon != null) return true;
                else if (Armor != null) return true;
                else if (Jia1Ma != null) return true;
                else if (Jian1Ma != null) return true;
                else return false;
            }
        }

        /// <summary>
        /// 玩家是否被铁索连环
        /// </summary>
        public bool HorizontalSet
        {
            set;
            get;
        }

        /// <summary>
        /// 玩家是否被翻面
        /// </summary>
        public bool TurnSet
        {
            set;
            get;
        }

        /// <summary>
        /// 玩家是否有手牌
        /// </summary>
        public bool HasHand
        {
            get
            {
                if (Hands.Count > 0) return true;
                return false;
            }
        }

        /// <summary>
        /// 玩家是否有牌,计算范围包括手牌和装备
        /// </summary>
        public bool HasCard
        {
            get
            {
                if (Hands.Count > 0) return true;
                else if (Weapon != null) return true;
                else if (Armor != null) return true;
                else if (Jia1Ma != null) return true;
                else if (Jian1Ma != null) return true;
                else return false;
            }
        }

        /// <summary>
        /// 安装-1马的方法
        /// </summary>
        /// <param name="aHorse">需要安装的牌</param>
        /// <param name="aObj">游戏事件对象</param>
        /// <param name="aData">游戏数据</param>
        internal void LoadJian1(Card aHorse, GlobalEvent aObj, GlobalData aData)
        {
            if (Jian1Ma != null)
            {
                aObj.DropCards(true, GlobalEvent.CardFrom.Equipage, string.Empty, new Card[] { Jian1Ma }, Card.Effect.None, Chief, null, null);
            }
            Jian1Ma = aHorse;
            aObj.DropCards(false, GlobalEvent.CardFrom.None, string.Empty , new Card[] { aHorse }, aHorse.CardEffect, Chief, Chief, null);
        }

        /// <summary>
        /// 卸掉-1马的方法
        /// </summary>
        /// <param name="aObj">游戏事件对象</param>
        public void UnloadJian1(GlobalEvent aObj)
        {
            if (Jian1Ma != null)
            {
                Jian1Ma = null;
            }
        }

        /// <summary>
        /// 安装+1马的方法
        /// </summary>
        /// <param name="aHorse">被安装的牌</param>
        /// <param name="aObj">游戏事件对象</param>
        /// <param name="aData">游戏数据</param>
        internal void LoadJia1(Card aHorse, GlobalEvent aObj, GlobalData aData)
        {
            if (Jia1Ma != null)
            {
                aObj.DropCards(true, GlobalEvent.CardFrom.Equipage, string.Empty, new Card[] { Jia1Ma }, Card.Effect.None, Chief, null, null);
            }
            Jia1Ma = aHorse;
            aObj.DropCards(false, GlobalEvent.CardFrom.None, string.Empty , new Card[] { aHorse }, aHorse.CardEffect, Chief, Chief, null);
        }

        /// <summary>
        /// 卸掉+1马的事件
        /// </summary>
        /// <param name="aObj">游戏事件对象</param>
        public void UnloadJia1(GlobalEvent aObj)
        {
            if (Jia1Ma != null)
            {
                Jia1Ma = null;
            }
        }

        /// <summary>
        /// 安装武器的方法
        /// </summary>
        /// <param name="aWeapon">被安装的牌</param>
        /// <param name="aObj">游戏事件对象</param>
        /// <param name="aData">游戏数据</param>
        internal void LoadWeapon(Card aWeapon, GlobalEvent aObj, GlobalData aData)
        {
            if (Weapon != null)
            {
                aObj.DropCards(true, GlobalEvent.CardFrom.Equipage, string.Empty, new Card[] { Weapon }, Card.Effect.None, Chief, null, null);
            }
            Weapon = aWeapon;
            SanGuoSha.ServerCore.Contest.Equipage.Weapon.ActiveWeapon(Weapon.CardEffect, aData);
            aData.Game.AsynchronousCore.SendMessage(
                    new Beaver("weapon.load", Chief.ChiefName, aWeapon.CardEffect.ToString(), Card.Cards2Beaver("cards", new Card[] { aWeapon })).ToString());
                    //new XElement("weapon.load",
                    //    new XElement("target", Chief.ChiefName),
                    //    new XElement("effect" , aWeapon.CardEffect),
                    //    Card.Cards2XML("cards", new Card[] { aWeapon })
                    //)
                    //);
            foreach (ASkill s in Chief.Skills)
                s.WeaponUpdated(Chief, Weapon ,aData);
            aObj.DropCards(false, GlobalEvent.CardFrom.None, string.Empty , new Card[] { aWeapon }, aWeapon.CardEffect, Chief, Chief, null);
        }

        /// <summary>
        /// 卸掉武器的方法
        /// </summary>
        /// <param name="aObj">游戏事件对象</param>
        /// <param name="aData">游戏数据</param>
        public void UnloadWeapon(GlobalEvent aObj, GlobalData aData)
        {
            if (Weapon != null)
            {
                SanGuoSha.ServerCore.Contest.Equipage.Weapon.UnloadWeapon(Weapon.CardEffect, aData);
                Weapon = null;
                foreach (ASkill s in Chief.Skills)
                    s.WeaponUpdated(Chief, Weapon, aData);
            }
        }

        //internal void LoadHorse(Card aHorse, GlobalEvent aObj)
        //{
        //    switch (aHorse.CardEffect)
        //    {
        //        case Card.Effect.Jia1:
        //            if (Jia1Ma != null)
        //                aObj.EventNode(true, GlobalEvent.CardFrom.Equipage, string.Empty, new Card[] { Jia1Ma }, Card.Effect.None, null, null, null);

        //            Jia1Ma = aHorse;
        //            aObj.EventNode(false, GlobalEvent.CardFrom.None, string.Empty, new Card[] { aHorse }, aHorse.CardEffect, Chief, Chief, null);
        //            break;
        //        case Card.Effect.Jian1:
        //            if (Jian1Ma != null)
        //                aObj.EventNode(true, GlobalEvent.CardFrom.Equipage, string.Empty, new Card[] { Jian1Ma }, Card.Effect.None, null, null, null);
        //            Jian1Ma = aHorse;
        //            aObj.EventNode(false, GlobalEvent.CardFrom.None, string.Empty, new Card[] { aHorse }, aHorse.CardEffect, Chief, Chief, null);
        //            break;
        //    }
            
        //}


        //internal void UnloadHorse(GlobalEvent aObj)
        //{

        //}

        /// <summary>
        /// 安装防具的方法
        /// </summary>
        /// <param name="aArmor">被安装的牌</param>
        /// <param name="aObj">游戏事件对象</param>
        internal void LoadArmor(Card aArmor , GlobalEvent aObj)
        {
            if (Armor != null)
                aObj.DropCards(true, GlobalEvent.CardFrom.Equipage, string.Empty, new Card[] { Armor }, Card.Effect.None, Chief, null, null);
            aObj.AsynchronousCore.SendMessage(
                    new Beaver("armor.load", Chief.ChiefName , aArmor.CardEffect.ToString() , Card.Cards2Beaver("cards" , new Card[]{aArmor})).ToString());
                    //new XElement("armor.load",
                    //    new XElement("target", Chief.ChiefName),
                    //    new XElement("effect", aArmor.CardEffect),
                    //    Card.Cards2XML("cards", new Card[] { aArmor })
                    //)
                    //);
            Armor = aArmor;
            aObj.DropCards(false, GlobalEvent.CardFrom.None, string.Empty , new Card[] { aArmor }, aArmor.CardEffect, Chief, Chief, null);
        }

        /// <summary>
        /// 卸掉防具的方法
        /// </summary>
        /// <param name="aObj">游戏事件对象</param>
        public void UnloadArmor(GlobalEvent aObj)
        {
            if (Armor != null)
            {
                SanGuoSha.ServerCore.Contest.Equipage.Armor.OnUnloadAromor(Armor.CardEffect, aObj.gData);
                Armor = null;
            }
        }

        /// <summary>
        /// 构造玩家,此方法仅用于测试
        /// </summary>
        /// <param name="aGame">游戏对象</param>
        /// <param name="aCallback">回调接口</param>
        /// <param name="aService">服务接口</param>
        public Player(GameBase aGame, ICallback aCallback , IService aService)
        {
            Messages = new List<string>();
            Hands = new List<Card>();
            Debuff = new Stack<Card>();
            Callback = aCallback;
            Service = aService;
            HorizontalSet = false;
            TurnSet = false;
            AvailableChiefs = new ChiefBase[0] { };
        }

        /// <summary>
        /// 构造玩家
        /// </summary>
        /// <param name="aGame">游戏对象</param>
        /// <param name="aCallback">玩家所对应的服务回调</param>
        /// <param name="aUID">玩家UID</param>
        /// <param name="aUName">玩家名称</param>
        public Player(GameBase aGame, ICallback aCallback, string aUID, string aUName)
        {
            if (aGame == null) throw new Exception("Game is null");
            if (aCallback == null) throw new Exception("Service Or Callback is null");
            Messages = new List<string>();
            Hands = new List<Card>();
            Debuff = new Stack<Card>();
            Callback = aCallback;
            ServiceCore svr = new ServiceCore(this, aGame);
            Service = svr;
            HorizontalSet = false;
            TurnSet = false;
            AvailableChiefs = new ChiefBase[0] { };
            UID = aUID;
            PlayerName = aUName;
        }

        /// <summary>
        /// 构造玩家
        /// </summary>
        /// <param name="aGame">游戏对象</param>
        /// <param name="aService">玩家所对应的服务实例接口</param>
        /// <param name="aCallback">玩家所对应的服务回调</param>
        /// <param name="aUID">玩家UID</param>
        /// <param name="aUName">玩家名称</param>
        public Player(GameBase aGame, IService aService , ICallback aCallback , string aUID , string aUName)
        {
            if (aGame == null) throw new Exception("Game is null");
            if (aService == null || aCallback == null) throw new Exception("Service Or Callback is null");
            Messages = new List<string>();
            Hands = new List<Card>();
            Debuff = new Stack<Card>();
            Callback = aCallback;
            ((ServiceCore)aService).Game = aGame;
            ((ServiceCore)aService).ThisPlayer = this;
            Service = aService;
            HorizontalSet = false;
            TurnSet = false;
            AvailableChiefs = new ChiefBase[0] { };
            UID = aUID;
            PlayerName = aUName;
        }

        /// <summary>
        /// 移除一个在判定区的牌
        /// </summary>
        /// <param name="aCard">牌对象</param>
        /// <returns>移除成功返回true , 失败或者参数为null返回false</returns>
        public bool RemoveBuff(Card aCard)
        {
            if (aCard == null) return false;
            Stack<Card> tmp = new Stack<Card>();
            bool ret = false;
            while (Debuff.Count != 0)
            {
                if (aCard.IsSame(Debuff.Peek()))
                {
                    Debuff.Pop();
                    ret = true;
                }
                else
                    tmp.Push(Debuff.Pop());
            }
            while (tmp.Count != 0)
            {
                Debuff.Push(tmp.Pop());
            }
            return ret;
        }

        /// <summary>
        /// 移除一张手牌
        /// </summary>
        /// <param name="aCard">要移除的手牌对象</param>
        /// <returns>移除成功返回true, 失败或者参数为null返回false</returns>
        public bool RemoveHand(Card aCard)
        {
            if (aCard == null) return false;
            foreach (Card c in Hands)
            {
                if (aCard.IsSame(c))
                {
                    Hands.Remove(c);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 确定这些牌都在玩家的手牌中
        /// </summary>
        /// <param name="aCards">牌数组</param>
        /// <returns>若牌数组的牌都在手牌中,则返回true</returns>
        public bool HasCardsInHand(Card[] aCards)
        {
            foreach (Card c in aCards)
                if (!Hands.Contains(c)) return false;
            return true;
        }

        /// <summary>
        /// 用户检测用户的判定区是否有该类型的Debuff
        /// </summary>
        /// <param name="aEffect">要查找的效果</param>
        /// <returns></returns>
        internal bool HasDebuff(Card.Effect aEffect)
        {
            foreach (Card c in Debuff)
            {
                if (c.CardEffect == aEffect)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 确定这些牌都来自玩家的手牌或者装备中
        /// </summary>
        /// <param name="aCards">需要检查的牌</param>
        /// <returns>若都在手牌或者装备中，返回true</returns>
        public bool HasCardsInHandOrEquipage(Card[] aCards)
        {
            foreach (Card c in aCards)
                if (c != null)
                    if (!Hands.Contains(c))
                        if (c != Weapon)
                            if (c != Armor)
                                if (c != Jia1Ma)
                                    if (c != Jian1Ma)
                                        return false;
            return true;
        }

        #endregion

        #region 服务区

        /// <summary>
        /// 服务接口
        /// </summary>
        public IService Service
        {
            get;
            private set;
        }

        /// <summary>
        /// 回调接口
        /// </summary>
        public ICallback Callback
        {
            get;
            set;
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 玩家转武将的方法
        /// </summary>
        /// <param name="aPlayers">玩家数组</param>
        /// <returns>武将数组</returns>
        internal static ChiefBase[] Players2Chiefs(Player[] aPlayers)
        {
            List<ChiefBase> lst = new List<ChiefBase>();
            if (aPlayers != null)
                foreach (Player p in aPlayers)
                    if (p.Chief != null)
                        lst.Add(p.Chief);
            return lst.ToArray();
        }
#endregion
    }

    /// <summary>
    /// 玩家集合
    /// </summary>
    public class Players
    {
        private volatile List<Player> PlayerList = new List<Player>();

        /// <summary>
        /// 返回由所有玩家构成的数组
        /// </summary>
        public Player[] All
        {
            get
            {
                return PlayerList.ToArray();
            }
        }

        public void UpdatePlayer(string aUID, Player aPlayer)
        {
            PlayerList = PlayerList.Select(i => i.UID == aUID ? aPlayer : i).ToList();
        }

        /// <summary>
        /// 增加一个玩家
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        public void AddPlayer(Player aPlayer)
        {
            PlayerList.Add(aPlayer);
            PlayerList = PlayerList.Distinct().ToList();
        }

        /// <summary>
        /// 查找下一位活着的玩家
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <returns>武将对象</returns>
        public ChiefBase NextChief(ChiefBase aChief)
        {
            return NextChief(aChief.ChiefName);
        }

        /// <summary>
        /// 查找下一位活着的玩家
        /// </summary>
        /// <param name="aChiefName">武将名称</param>
        /// <returns>武将对象</returns>
        public ChiefBase NextChief(string aChiefName)
        {
            int i = 0;
            foreach (Player p in PlayerList)
            {
                if (p.Chief.ChiefName == aChiefName)
                {
                    int j = PlayerList.Count;
                    while (j > 0)
                    {
                        if (!PlayerList[i = (++i) % PlayerList.Count].Dead)
                            return PlayerList[i].Chief;
                        j--;
                    }
                    return null;
                }
                i++;
            }
            return null;
        }

        /// <summary>
        /// 获取玩家数量
        /// </summary>
        public int PlayerCount
        {
            get
            {
                return PlayerList.Count;
            }
        }


        /// <summary>
        /// 通过序号获取或设置玩家,索引为-1表示返回一个场上活着的玩家
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Player this[int i]
        {
            get
            {
                if (i == -1)
                {
                    foreach (Player p in PlayerList)
                    {
                        if (!p.Dead) return p;
                    }
                    return null;
                }
                return PlayerList[i];
            }
            set
            {
                PlayerList[i] = value;
            }
        }

        /// <summary>
        /// 获取当前或者的玩家数量
        /// </summary>
        public int PeoplealiveCount
        {
            get
            {
                return PlayerList.Where(i => !i.Dead).Count();
            }
        }

        /// <summary>
        /// 通过玩家选择的武将来获取或设置玩家
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <returns>玩家对象</returns>
        public Player this[Player aPlayer]
        {
            get
            {
                return this[aPlayer.Chief.ChiefName];
            }
            set
            {
                this[aPlayer.Chief.ChiefName] = value;
            }
        }

        /// <summary>
        /// 通过玩家选择的武将来获取或设置玩家
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <returns>玩家对象</returns>
        public Player this[ChiefBase aChief]
        {
            get
            {
                return this[aChief.ChiefName];
            }
            set
            {
                this[aChief.ChiefName] = value;
            }
        }

        /// <summary>
        /// 通过玩家选择的武将来获取或设置玩家
        /// </summary>
        /// <param name="ChiefName">武将名称</param>
        /// <returns>玩家名称</returns>
        public Player this[string ChiefName]
        {
            get
            {
                foreach (Player p in PlayerList)
                    if (p.Chief.ChiefName == ChiefName)
                        return p;
                //这里我觉得，找不到武将对象就返回null是不明智的，因为同步层不会发生找不到的问题
                //但是在异步通信层中这一点却很重要，因为这个索引器要解决同步异步两种任务的查询，返回null并不稳妥
                return null;
            }
            set
            {
                for (int i = 0; i < PlayerList.Count; i++)
                {
                    if (PlayerList[i].Chief.ChiefName == ChiefName)
                    {
                        PlayerList[i] = value;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获得A武将到B武将的距离，不包括-1计算,但包括B武将的+1计算
        /// </summary>
        /// <param name="A">起点武将</param>
        /// <param name="B">目的武将</param>
        /// <returns>距离</returns>
        internal byte Distance(ChiefBase A, ChiefBase B)
        {
            if (A == null || B == null) return 0;
            if (this[A].Dead ||this[B].Dead || this[A].Chief.IsMe(B)) return 0;
            byte pa = 0;
            byte pb = 0;
            pa = Convert.ToByte(this.PlayerList.IndexOf(this[A]));
            pb = Convert.ToByte(this.PlayerList.IndexOf(this[B]));
            
            byte t1 = 0;
            byte t2 = 0;
            byte p = pa;
            while (p != pb && t1 < 100)
            {
                if(!PlayerList[p].Dead)
                    t1++;
                p = (byte)((p + 1) % PlayerList.Count);
            }
            p = pb;
            while (p != pa && t2 < 100)
            {
                if (!PlayerList[p].Dead)
                    t2++;
                p = (byte)((p + 1) % PlayerList.Count);
            }
            if (this[B].Jia1Ma != null)
            {
                t1++; t2++;
            }
            return Math.Min(t1, t2);
        }
    }
}
