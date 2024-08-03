/*
 * 这个文件描述了游戏对象给用户的接口和回调接口
 * 并且实现了两个接口的类
 * ICallback是服务向用户传送的接口
 * IService是客户向服务传送的接口
 * 作为用户交互的唯一通道，这两个接口是非常重要的
 * CallbackCore类继承了ICallback接口，但没有任何意义
 * ServiceCore类继承了IService接口，实例化的ServiceCore是系统收到用户相应的重要对象
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Global;
using SanGuoSha.ServerCore.Contest.Data;
using BeaverMarkupLanguage;

//这些接口被划归在中间件命名空间中
namespace SanGuoSha.ServerCore.Contest.Apusic
{
    /// <summary>
    /// ICallback接口是WCF对象提供给服务的回调接口，即信息发向客户的通道
    /// 基本上，客户端的大多数功能由Message方法完成
    /// 系统的客户端必须重写此接口，已完成客户端对客户的响应
    /// </summary>
    public interface ICallback
    {
        /// <summary>
        /// 回调游戏消息和问询方法
        /// </summary>
        /// <param name="aText">返回XML消息的参数</param>
        void Message(string aText);

        /// <summary>
        /// 出牌有效
        /// </summary>
        void LeadingValid();

        /// <summary>
        /// 出牌无效，返回自己的牌数据
        /// </summary>
        /// <param name="aHand">当前手牌</param>
        /// <param name="Weapon">当前武器</param>
        /// <param name="Armor">当前装备</param>
        /// <param name="Jia1">当前+1</param>
        /// <param name="Jian1">当前-1</param>
        void LeadingInvalid(int[] aHand, int Weapon, int Armor, int Jia1, int Jian1);
    }

    /// <summary>
    /// WCF服务对外的接口
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 获取游戏的XML事件链，系统返回到此时为止所发生的所有事件
        /// </summary>
        /// <returns>一个XML对象</returns>
        string GetGameEventChain();

        /// <summary>
        /// 回应给系统的玩家动作，这是一个在玩家游戏中使用的操作
        /// </summary>
        /// <param name="aSkillName">若使用了技能，这里应写入技能名称，否则必须使用空字符串,主参</param>
        /// <param name="aCards">打出或选择的牌号码,主参</param>
        /// <param name="aTargets">若需要回应目标，在此回应玩家的武将名称,选参</param>
        /// <param name="aYN">若需回应是否，在此设置具体回应结果,选参</param>
        void Response(string aSkillName, int[] aCards, string[] aTargets, bool aYN);

        /// <summary>
        /// 选择武将的服务接口
        /// </summary>
        /// <param name="aChiefName">武将的名称</param>
        void SelectChief(string aChiefName);

        /// <summary>
        /// 聊天的接口
        /// </summary>
        /// <param name="aMessage">消息字符串</param>
        void Chat(string aChannle , string aMessage);

        /// <summary>
        /// 玩家离线
        /// </summary>
        /// <param name="aCallback">新的回调接口,一般这个接口是不处理任何回调的</param>
        void PlayerOffline(ICallback aCallback);

        /// <summary>
        /// 玩家离开
        /// </summary>
        /// <param name="aCallback">新的回调接口,一般这个接口是不处理任何回调的</param>
        void PlayerLeft(ICallback aCallback);

        /// <summary>
        /// 玩家恢复上线
        /// </summary>
        /// <param name="aCallback"></param>
        IService PlayerOnline(ICallback aCallback);
    }

#if !_TEST
    /// <summary>
    /// 一个简单的回调接口的实现，不完成任何回应操作，永远保持沉默
    /// </summary>
    public class CallbackCore : ICallback
    {
        private string ThisPlayer
        {
            get;
            set;
        }

        private readonly GameBase Game;

        /// <summary>
        /// 回调类的构造函数
        /// </summary>
        /// <param name="aPlayer">该玩家的武将名</param>
        /// <param name="aGame">游戏系统对象</param>
        public CallbackCore(string aPlayer ,GameBase aGame)
        {
            ThisPlayer = aPlayer;
            Game = aGame;
        }

        /// <summary>
        /// 出牌有效，调试用API，运行时意义不大
        /// </summary>
        public void LeadingValid()
        {

        }

        /// <summary>
        /// 出牌无效，参数返回玩家当前的牌，在玩家错误出牌时纠正当前牌的数据
        /// </summary>
        /// <param name="aHand">当前手牌</param>
        /// <param name="Weapon">当前武器</param>
        /// <param name="Armor">当前装备</param>
        /// <param name="Jia1">当前+1</param>
        /// <param name="Jian1">当前-1</param>
        public void LeadingInvalid(int[] aHand, int Weapon, int Armor, int Jia1, int Jian1)
        {

        }

        /// <summary>
        /// 简单问询接口实现
        /// </summary>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aPlayer">系统给出的目标玩家，常用语配合过河拆桥，顺手牵羊等</param>
        internal void AskFor(MessageCore.AskForEnum aAskFor, string aPlayer)
        {

        }

        /// <summary>
        /// 消息接口的实现
        /// </summary>
        /// <param name="sText">消息内容</param>
        public void Message(string sText)
        {
            
        }
    }
#endif

    /// <summary>
    /// WCF服务的实现
    /// </summary>
    public class ServiceCore : IService
    {
        /// <summary>
        /// 该实例的玩家对象
        /// </summary>
        public Player ThisPlayer
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏的对象
        /// </summary>
        public GameBase Game
        {
            get;
            set;
        }

        /// <summary>
        /// 服务实例的构造函数
        /// </summary>
        /// <param name="aPlayer">对应的玩家对象</param>
        /// <param name="aGameBase">游戏对象</param>
        public ServiceCore(Player aPlayer ,GameBase aGameBase)
        {
            ThisPlayer = aPlayer;
            Game = aGameBase;
        }



        /// <summary>
        /// 提供了将牌ID号码转换成牌对象的方法，注意号码仅限于该玩家相关区域（或对手区域）或五谷丰登选择区中，若ID不存在则无视该ID的转换
        /// </summary>
        /// <param name="aIDs">ID号码数组</param>
        /// <param name="aPlayer"></param>
        /// <returns></returns>
        private Card[] IDs2Cards(int[] aIDs, Player aPlayer)
        {
            //去重复项
            aIDs = aIDs.Distinct().ToArray();
            List<Card> ret = new List<Card>();
            //若target没有引用，那么选择在该玩家手牌，装备区，判定区,牌槽及五谷丰登选择区寻找响应ID
            if (Game.WaittingData.Target == null)
            {
                foreach (int i in aIDs)
                {
                    if (i == 0)
                    {
                        ret.Add(CardHeap.Unknown);
                        continue;
                    }
                    else if (i == -1)
                    {
                        ret.Add(CardHeap.Spade);
                        continue;
                    }
                    else if (i == -2)
                    {
                        ret.Add(CardHeap.Club);
                        continue;
                    }
                    else if (i == -3)
                    {
                        ret.Add(CardHeap.Heart);
                        continue;
                    }
                    else if (i == -4)
                    {
                        ret.Add(CardHeap.Diamond);
                        continue;
                    }

                    if (ThisPlayer.Weapon != null && ThisPlayer.Weapon.ID == i)
                    {
                        ret.Add(ThisPlayer.Weapon);
                        continue;
                    }
                    else if (ThisPlayer.Armor != null && ThisPlayer.Armor.ID == i)
                    {
                        ret.Add(ThisPlayer.Weapon);
                        continue;
                    }
                    else if (ThisPlayer.Jia1Ma != null && ThisPlayer.Jia1Ma.ID == i)
                    {
                        ret.Add(ThisPlayer.Weapon);
                        continue;
                    }
                    else if (ThisPlayer.Jian1Ma != null && ThisPlayer.Jian1Ma.ID == i)
                    {
                        ret.Add(ThisPlayer.Weapon);
                        continue;
                    }
                    else
                    {
                        foreach (Card c in ThisPlayer.Hands)
                            if (c.ID == i)
                            {
                                ret.Add(c);
                                break;
                            }
                        foreach (Card c in ThisPlayer.Debuff.Where(c => c.ID == i))
                        {
                            ret.Add(c);
                        }
                        foreach(Slot s in ThisPlayer.Chief.SlotsBuffer.Slots)
                            foreach (Card c in s.Cards.Where(c => c.ID == i))
                            {
                                ret.Add(c);
                            }
                        foreach (Card c in Game.CardsBuffer[Game.WGFDSlotName].Cards)
                            if (c.ID == i)
                            {
                                ret.Add(c);
                                break;
                            }
                    }
                }
            }
            else
            {
                foreach (int i in aIDs)
                {
                    if (i == 0)
                    {
                        ret.Add(CardHeap.Unknown);
                        continue;
                    }
                    else if (i == -1)
                    {
                        ret.Add(CardHeap.Spade);
                        continue;
                    }
                    else if (i == -2)
                    {
                        ret.Add(CardHeap.Club);
                        continue;
                    }
                    else if (i == -3)
                    {
                        ret.Add(CardHeap.Heart);
                        continue;
                    }
                    else if (i == -4)
                    {
                        ret.Add(CardHeap.Diamond);
                        continue;
                    }
                    if (Game.WaittingData.Target.Weapon != null && Game.WaittingData.Target.Weapon.ID == i)
                    {
                        ret.Add(Game.WaittingData.Target.Weapon);
                        continue;
                    }
                    else if (Game.WaittingData.Target.Armor != null && Game.WaittingData.Target.Armor.ID == i)
                    {
                        ret.Add(Game.WaittingData.Target.Weapon);
                        continue;
                    }
                    else if (Game.WaittingData.Target.Jia1Ma != null && Game.WaittingData.Target.Jia1Ma.ID == i)
                    {
                        ret.Add(Game.WaittingData.Target.Weapon);
                        continue;
                    }
                    else if (Game.WaittingData.Target.Jian1Ma != null && Game.WaittingData.Target.Jian1Ma.ID == i)
                    {
                        ret.Add(Game.WaittingData.Target.Weapon);
                        continue;
                    }
                    else
                    {
                        foreach (Card c in Game.WaittingData.Target.Hands)
                            if (c.ID == i)
                            {
                                ret.Add(c);
                                break;
                            }
                        foreach (Slot s in ThisPlayer.Chief.SlotsBuffer.Slots)
                            foreach (Card c in s.Cards.Where(c => c.ID == i))
                            {
                                ret.Add(c);
                            }
                        foreach (Card c in Game.WaittingData.Target.Debuff.Where(c => c.ID == i))
                        {
                            ret.Add(c);
                        }
                    }
                }
            }
            return ret.Distinct().ToArray();
        }

        /// <summary>
        /// 提供武将名称转换成武将对象的方法，转换不成功的名称会被忽略
        /// </summary>
        /// <param name="aNames">武将名称组</param>
        /// <param name="aPlayers">游戏中的玩家集合</param>
        /// <returns>武将对象数组</returns>
        private Player[] Names2Players(string[] aNames , Players aPlayers)
        {
            List<Player> ret = new List<Player>();
            foreach (string n in aNames)
            {
                foreach (Player p in aPlayers.All)
                    if (p.Chief.ChiefName == n && !p.Dead)
                        ret.Add(p);
            }
            return ret.Distinct().ToArray();
        }

        /// <summary>
        /// 选择武将的请求
        /// </summary>
        /// <param name="aChiefName">武将名称</param>
        public void SelectChief(string aChiefName)
        {
            if (Game == null || ThisPlayer == null) return;
            //上锁
            lock (Game.WaittingData.Achievelock)
            {
                //判断能否执行具体操作
                //判断包括 问询是否开启 且 问询线程是否拿到互斥信号 且 (该玩家是否选择了武将 或 可选武将组存在且该玩家在玩家集合中) 且 ThisPlayer 和 Game非null
                if (Game.WaittingData.Enable && Game.WaittingData.SynchronizationReady && (ThisPlayer.Chief == null || ThisPlayer.AvailableChiefs.Count() >0  && Game.GamePlayers.All.Contains(ThisPlayer)))
                {
                    //在可选武将组中找不到武将名称,那么返回请求
                    if (ThisPlayer.AvailableChiefs.Where(c => c.ChiefName == aChiefName).Count() != 1) return;
                    //将武将作为玩家所选武将
                    foreach (ChiefBase c in ThisPlayer.AvailableChiefs.Where(c => c.ChiefName == aChiefName))
                        ThisPlayer.Chief = c;

                    //置回应的数据结构,结果与复位
                    Game.WaittingData.Enable = false;
                    Game.WaittingData.SynchronizationReady = false;
                    Game.Response.HasResponse = true;
                    Game.Response.IsTimeout = false;
                    //表决字典置该玩家已表决
                    Game.WaittingData.Abstention[ThisPlayer] = true;
                    //不存在没有表决的玩家,则释放互斥信号,使主线程继续运行
                    if(!Game.WaittingData.Abstention.ContainsValue(false))
                        try
                        {
                            //终止线程,产生异常引号
                            if (Game.WaittingData.WaittingThread != null)
                            {
                                Game.WaittingData.WaittingThread.Abort();
                                Game.WaittingData.WaittingThread = null;
                            }
                        }
                        catch
                        {

                        }
                }
            }

        }

        /// <summary>
        /// 游戏事件链
        /// </summary>
        /// <returns></returns>
        public string GetGameEventChain()
        {
            if(ThisPlayer == null || !Game.GamePlayers.All.Contains(ThisPlayer))
            {
                return Game.AsynchronousCore.MakeEnvironmentXMLReport(null);
            }
            return Game.AsynchronousCore.MakeEnvironmentXMLReport(ThisPlayer.Chief);
        }

        /// <summary>
        /// 聊天请求
        /// </summary>
        /// <param name="aMessage"></param>
        public void Chat(string aChannle ,string aMessage)
        {
            GlobalEvent ge = Game as GlobalEvent;
            ge.AsynchronousCore.SendMessage(
                new Beaver("chat" , ThisPlayer.UID , aMessage).ToString());
                //new XElement("chat",
                //    new XElement("from" , ThisPlayer.UID),
                //    new XElement("message"  , aMessage )
                //));
        }

        /// <summary>
        /// 玩家发送的游戏回应请求
        /// </summary>
        /// <param name="aSkillName">发送的技能名称，若需要</param>
        /// <param name="aCardIDs">牌的号码组成的数组，若需要</param>
        /// <param name="aTargetNames">目标武将名称组成的数组，若需要</param>
        /// <param name="aYN">回应的是否，若需要</param>
        public void Response(string aSkillName, int[] aCardIDs, string[] aTargetNames, bool aYN)
        {
            if(!ResponseProc(aSkillName , aCardIDs , aTargetNames , aYN) && Game.WaittingData.AskTo!= null && Game.WaittingData.AskTo.Chief != null)
            {
                //上锁
                lock (Game.WaittingData.Achievelock)
                {
                    if (Game.WaittingData.AskTo != null)
                    {
                        Game.AsynchronousCore.LeadingInvalid(Game.WaittingData.AskTo.Chief);
                    }
                }
            }
        }
        //这个方法似乎仍存在频繁提交请求造成锁失效的问题
        /// <summary>
        /// 玩家发送的游戏回应请求过程
        /// </summary>
        /// <param name="aSkillName">发送的技能名称，若需要</param>
        /// <param name="aCardIDs">牌的号码组成的数组，若需要</param>
        /// <param name="aTargetNames">目标武将名称组成的数组，若需要</param>
        /// <param name="aYN">回应的是否，若需要</param>
        /// <returns>返回true表示低层次验证通过这些请求或者出错但忽略告知客户回应非法,如果返回false,那么需要告知客户端出牌无效</returns>
        public bool ResponseProc(string aSkillName, int[] aCardIDs , string[] aTargetNames , bool aYN)
        {
            if (Game == null || ThisPlayer == null) return false;
            //上锁
            lock (Game.WaittingData.Achievelock)
            {
                //判断该操作是否可以进行
                if ( Game.WaittingData.AskTo != null && Game.WaittingData.AskTo != ThisPlayer) return true; //如果问询的目标和回应的目标不是同一玩家，返回true且忽略这则消息的出牌错误回调

                if (ThisPlayer != null && Game.WaittingData.Enable && Game.WaittingData.SynchronizationReady &&  //ThisPlayer 和 Game非null 且 允许问询且等待线程获得互斥信号
                    // 【游戏问询所有玩家 且 该玩家在玩家集合中】 或 问询的是该玩家        最后，该玩家必须选择了武将！
                    ((Game.WaittingData.ToEvery && Game.GamePlayers.All.Contains(ThisPlayer)) || Game.WaittingData.AskTo == ThisPlayer) && ThisPlayer.Chief != null)
                {
                    //牌号码转换称牌对象
                    Card[] aCards = IDs2Cards(aCardIDs, ThisPlayer);
                    //目标武将名称转换成目标武将对象 
                    Player[] Targets = Names2Players(aTargetNames , Game.GamePlayers);

                    //若使用了技能名称,激活该角色武将的所有技能
                    if (aSkillName != string.Empty)
                    {
                        foreach (ASkill s in ThisPlayer.Chief.Skills)
                            if (!s.ActiveSkill(aSkillName, aCards, ThisPlayer.Chief, Player.Players2Chiefs(Targets), Game.WaittingData.AskFor, ref Game.Response.Effect, Game.gData)) return false;
                    }
                    else 
                    //根据问询来匹配相应的处理过程
                    switch (Game.WaittingData.AskFor)
                    {
                        case MessageCore.AskForEnum.SlotCards:
                            Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            break;
                        case MessageCore.AskForEnum.AskForTao:
                            if (aCards.Count() == 1 && aCards[0].CardEffect == Card.Effect.Tao)
                            {
                                bool found = false;
                                if (ThisPlayer.HasCardsInHand(aCards))
                                {
                                    found = true;
                                }
                                if (!found) return false;
                                Game.Response.Effect = aCards[0].CardEffect;
                            }
                            else if (aCards.Count() > 1) return false;  //出牌数量超过1视为对系统的攻击
                            
                            break;
                        case MessageCore.AskForEnum.AskForTaoOrJiu:
                            if (aCards.Count() == 1 && (aCards[0].CardEffect == Card.Effect.Tao || aCards[0].CardEffect == Card.Effect.Jiu))
                            {
                                bool found = false;
                                if (ThisPlayer.HasCardsInHand(aCards))
                                {
                                    found = true;
                                }
                                if (!found) return false;
                                Game.Response.Effect = aCards[0].CardEffect;
                            }
                            else if (aCards.Count() > 1) return false;  //出牌数量超过1视为对系统的攻击
                            
                            break;
                        case MessageCore.AskForEnum.YN:  //关于是否的问询
                            if (aCards.Count() > 0) return false;
                            Game.Response.Effect = Card.Effect.None;
                            Game.Response.Answer = aYN;
                            break;
                        case MessageCore.AskForEnum.WuGuFengDeng: //关于五谷丰登的问询
                            if (aCards.Count() == 1)
                            {
                                foreach (Card c in aCards)
                                {
                                    bool found = false;
                                    foreach (Card c2 in Game.CardsBuffer[Game.WGFDSlotName].Cards)
                                        if (c.Same(c2))
                                        {
                                            found = true;
                                            break;
                                        }
                                    if (!found) return false;
                                }
                            }
                            else if (aCards.Count() > 1) return false;  //出牌数量超过1视为对系统的攻击
                            Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            break;
                        case MessageCore.AskForEnum.TwoHandCards:
                            
                            if (aCards.Count() == 2)
                            {
                                foreach (Card c in aCards)
                                {
                                    bool found = false;
                                    foreach (Card c2 in Game.WaittingData.Target.Hands)
                                        if (c.Same(c2))
                                        {
                                            found = true;
                                            break;
                                        }
                                    if (!found)
                                    {
                                        Game.Response.Effect = Card.Effect.None;
                                        aCards = new Card[] { };
                                    }
                                }
                            }
                            else if (aCards.Count() != 0) return false;
                            Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            
                            break;
                        case MessageCore.AskForEnum.TargetHands:
                            if (Game.WaittingData.AskTo != Game.WaittingData.Target)
                                return false;
                            if (aCards.Count() == Game.WaittingData.CardsCount || Game.WaittingData.CardsCount == -1)
                            {
                                foreach (Card c in aCards)
                                {
                                    bool found = false;
                                    foreach (Card c2 in Game.WaittingData.Target.Hands)
                                        if (c.Same(c2))
                                        {
                                            found = true;
                                            break;
                                        }
                                    if (!found)
                                    {
                                        Game.Response.Effect = Card.Effect.None;
                                        aCards = new Card[] { };
                                    }
                                }
                            }
                            else 
                                return false;
                            Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            break;
                        case MessageCore.AskForEnum.TargetCardWithJudgementArea:
                            if (aCards.Count() == 1)
                            {
                                if (Game.WaittingData.Target.Armor == null || Game.WaittingData.Target.Armor != aCards[0])
                                    if (Game.WaittingData.Target.Weapon == null || Game.WaittingData.Target.Weapon != aCards[0])
                                        if (Game.WaittingData.Target.Jia1Ma == null || Game.WaittingData.Target.Jia1Ma != aCards[0])
                                            if (Game.WaittingData.Target.Jian1Ma == null || Game.WaittingData.Target.Jian1Ma != aCards[0])
                                            {
                                                bool ret = false;
                                                foreach (Card c in Game.WaittingData.Target.Hands)
                                                    if (c == aCards[0])
                                                    {
                                                        ret = true;
                                                        break;
                                                    }
                                                if (!ret)
                                                {
                                                    foreach (Card c in Game.WaittingData.Target.Debuff)
                                                        if (c == aCards[0])
                                                        {
                                                            ret = true;
                                                            break;
                                                        }
                                                    if (!ret) return false;
                                                }
                                            }
                                Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            }
                            else
                                Game.Response.Effect = Card.Effect.None;
                            break;
                        case MessageCore.AskForEnum.TargetHorse:
                            if (aCards.Count() == 1)
                            {
                                if (Targets.Count() != 1) return false;
                                if (Targets[0] != Game.WaittingData.Target) return false;
                                if (Game.WaittingData.Target.Jia1Ma != aCards[0] && Game.WaittingData.Target.Jian1Ma != aCards[0])
                                {
                                    return false;
                                }
                                Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            }
                            else
                                Game.Response.Effect = Card.Effect.None;
                            break;
                        case MessageCore.AskForEnum.TargetCard:
                            if (aCards.Count() == 1)
                            {
                                if (Targets.Count() != 1) return false;
                                if (Targets[0] != Game.WaittingData.Target) return false;
                                if (Game.WaittingData.Target.Armor != aCards[0] && Game.WaittingData.Target.Weapon != aCards[0] && Game.WaittingData.Target.Jia1Ma != aCards[0] && Game.WaittingData.Target.Jian1Ma != aCards[0])
                                {
                                    return false;
                                }
                                Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            }
                            else
                                Game.Response.Effect = Card.Effect.None;
                            break;
                        case MessageCore.AskForEnum.TargetHand://选择对方一张手牌
                            if (Game.WaittingData.AskTo == Game.WaittingData.Target)
                            {
                                if (aCards.Count() == 1)
                                {
                                    foreach (Card c in aCards)
                                    {
                                        bool found = false;
                                        foreach (Card c2 in Game.WaittingData.Target.Hands)
                                            if (c.Same(c2))
                                            {
                                                found = true;
                                                break;
                                            }
                                        if (!found) return false;
                                    }
                                }
                                else if (aCards.Count() > 1) return false;  //出牌数量超过1视为对系统的攻击
                            }
                            else
                            {
                                if (aCards.Count() > 0) return false;
                            }
                            Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            break;
                        case MessageCore.AskForEnum.TargetTwoCardsWithoutWeaponAndJudgement:
                            if (aCards.Count() == 2)
                            {
                                foreach (Card c in aCards)
                                {
                                    if (!Game.GamePlayers[Game.WaittingData.AskTo].Hands.Contains(c))
                                    {
                                        if (Game.GamePlayers[Game.WaittingData.AskTo].Armor != c)
                                            if (Game.GamePlayers[Game.WaittingData.AskTo].Jia1Ma != c)
                                                if (Game.GamePlayers[Game.WaittingData.AskTo].Jian1Ma != c)
                                                {
                                                    return false;
                                                }
                                    }
                                }
                                Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            }
                            break;
                        case MessageCore.AskForEnum.Suit:
                            if (aCards.Count() != 1) return false;
                            if (aCards[0].ID > -1 || aCards[0].ID < -4) return false;
                            Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                            break;
                        case MessageCore.AskForEnum.Abandonment:

                            break;
                        case MessageCore.AskForEnum.Aggressive:
                            if (aCards.Count() == 0)
                            {
                                Game.Response.Effect = Card.Effect.None;
                            }
                            else if (aCards.Count() != 1)
                            {
                                if (ThisPlayer.Weapon != null)
                                {
                                    SanGuoSha.ServerCore.Contest.Equipage.Weapon.LeadCards(ThisPlayer.Weapon.CardEffect, aCards, ThisPlayer.Chief, Player.Players2Chiefs(Targets) , MessageCore.AskForEnum.Aggressive, ref Game.Response.Effect, Game.gData);
                                }
                            }
                            //这是最基本的情况,没有使用任何技能且出一张牌
                            else if (aCards.Count() == 1)
                            {
                                switch (aCards[0].CardEffect)
                                {
                                    case Card.Effect.Shan:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Shan) return false;
                                        Game.Response.Effect = Card.Effect.Shan;
                                        break;
                                    case Card.Effect.Sha:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive && Game.WaittingData.AskFor != MessageCore.AskForEnum.Sha) return false;
                                        Game.Response.Effect = Card.Effect.Sha;
                                        break;
                                    case Card.Effect.JueDou:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.JueDou;
                                        break;
                                    case Card.Effect.Tao:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.Tao;
                                        break;
                                    case Card.Effect.NanManRuQin:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.NanManRuQin;
                                        break;
                                    case Card.Effect.WanJianQiFa:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.WanJianQiFa;
                                        break;
                                    case Card.Effect.TaoYuanJieYi:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.TaoYuanJieYi;
                                        break;
                                    case Card.Effect.WuZhongShengYou:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.WuZhongShengYou;
                                        break;
                                    case Card.Effect.GuoHeChaiQiao:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.GuoHeChaiQiao;
                                        break;
                                    case Card.Effect.ShunShouQianYang:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.ShunShouQianYang;
                                        break;
                                    case Card.Effect.JieDaoShaRen:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.JieDaoShaRen;
                                        break;
                                    case Card.Effect.LeBuSiShu:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.LeBuSiShu;
                                        break;
                                    case Card.Effect.ShanDian:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.ShanDian;
                                        break;
                                    case Card.Effect.WuGuFengDeng:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.WuGuFengDeng;
                                        break;
                                    case Card.Effect.WuXieKeJi:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.WuXieKeJi) return false;
                                        Game.Response.Effect = Card.Effect.WuXieKeJi;
                                        break;
                                    case Card.Effect.BaGuaZhen:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.BaGuaZhen;
                                        break;
                                    case Card.Effect.TengJia:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.TengJia;
                                        break;
                                    case Card.Effect.RenWangDun:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.RenWangDun;
                                        break;
                                    case Card.Effect.BaiYinShiZi:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.BaiYinShiZi;
                                        break;
                                    case Card.Effect.ZhangBaSheMao:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.ZhangBaSheMao;
                                        break;
                                    case Card.Effect.ZhuGeLianNu:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.ZhuGeLianNu;
                                        break;
                                    case Card.Effect.FangTianHuaJi:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.FangTianHuaJi;
                                        break;
                                    case Card.Effect.HanBingJian:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.HanBingJian;
                                        break;
                                    case Card.Effect.GuDianDao:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.GuDianDao;
                                        break;
                                    case Card.Effect.QiLinGong:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.QiLinGong;
                                        break;
                                    case Card.Effect.GuanShiFu:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.GuanShiFu;
                                        break;
                                    case Card.Effect.QingLongYanYueDao:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.QingLongYanYueDao;
                                        break;
                                    case Card.Effect.QingGangJian:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.QingGangJian;
                                        break;
                                    case Card.Effect.CiXiongShuangGuJian:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.CiXiongShuangGuJian;
                                        break;
                                    case Card.Effect.ZhuQueYuShan:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.ZhuQueYuShan;
                                        break;
                                    case Card.Effect.Jia1:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.Jia1;
                                        break;
                                    case Card.Effect.Jian1:
                                        if (Game.WaittingData.AskFor != MessageCore.AskForEnum.Aggressive) return false;
                                        Game.Response.Effect = Card.Effect.Jian1;
                                        break;
                                    default:
                                        Game.Response.Effect = Card.Effect.None;
                                        break;
                                }
                            }
                            else
                            {
                                Game.Response.Effect = Card.Effect.None;

                            }
                            break;
                        default:
                            if (aCards.Count() == 1)
                            {
                                Game.Response.Effect = aCards[0].CardEffect;
                            }
                            else if(ThisPlayer.Weapon != null )
                            {
                                SanGuoSha.ServerCore.Contest.Equipage.Weapon.LeadCards(ThisPlayer.Weapon.CardEffect, aCards, ThisPlayer.Chief, Player.Players2Chiefs(Targets), Game.WaittingData.AskFor, ref Game.Response.Effect, Game.gData);
                            }
                            break;
                    } //switch
                    
                    if(Game.WaittingData.AskFor != MessageCore.AskForEnum.Abandonment && Game.WaittingData.AskFor != MessageCore.AskForEnum.SlotCards && Game.WaittingData.AskFor != MessageCore.AskForEnum.TargetHand && Game.WaittingData.AskFor != MessageCore.AskForEnum.TargetCardWithJudgementArea && Game.WaittingData.AskFor != MessageCore.AskForEnum.TargetCard && Game.WaittingData.AskFor != MessageCore.AskForEnum.TargetHorse)
                        if (Game.Response.Effect == Card.Effect.None && aCards.Count() != 0 && aSkillName == string.Empty) return false;
                    if (Game.Response.Effect == Card.Effect.None && Game.WaittingData.AskTo == null)
                    {
                        if (Game.WaittingData.Abstention[ThisPlayer] == false)
                        {
                            Game.WaittingData.Abstention[ThisPlayer] = true;
                            if (Game.WaittingData.Abstention.Where(c => c.Value == false).Count() == 0)
                            {
                                Game.Response.Effect = Card.Effect.None;
                            }
                            else
                                return true;
                        }
                        else
                            return true;
                    }

                    Game.WaittingData.Enable = false;
                    Game.WaittingData.SynchronizationReady = false;
                    Game.Response.Cards = aCards;
                    Game.Response.HasResponse = true;
                    Game.Response.IsTimeout = false;
                    Game.Response.Targets = Targets;
                    Game.Response.Source = ThisPlayer;
                    Game.Response.SkillName = aSkillName;
                    try
                    {
                        
                        if (Game.WaittingData.WaittingThread != null)
                        {
                            Game.WaittingData.WaittingThread.Abort();
                            Game.WaittingData.WaittingThread = null;
                        }
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aCallback"></param>
        public void PlayerOffline(ICallback aCallback)
        {
            if (ThisPlayer.IsOffline || ThisPlayer.IsEscaped) return;
            ThisPlayer.IsOffline = true;
            ThisPlayer.Callback = aCallback;
            Game.AsynchronousCore.SendMessage(new Beaver("offline", ThisPlayer.UID).ToString()); //new XElement("offline", new XElement("uid", ThisPlayer.UID)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aCallback"></param>
        public void PlayerLeft(ICallback aCallback)
        {
            if (ThisPlayer.IsOffline || ThisPlayer.IsEscaped) return;
            ThisPlayer.IsEscaped = true;
            ThisPlayer.Callback = aCallback;
            Game.AsynchronousCore.SendMessage(new Beaver("left" , ThisPlayer.UID).ToString()); //new XElement("left", new XElement("uid", ThisPlayer.UID)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aCallback"></param>
        public IService PlayerOnline(ICallback aCallback)
        {
            if (ThisPlayer.IsOffline || ThisPlayer.IsEscaped)
            {
                ThisPlayer.IsEscaped = ThisPlayer.IsOffline = false;
                ThisPlayer.Callback = aCallback;
                Game.AsynchronousCore.SendMessage(new Beaver("online" , ThisPlayer.UID).ToString()); // new XElement("online", new XElement("uid", ThisPlayer.UID)));
            }
            return ThisPlayer.Service;
        }
    }

}
