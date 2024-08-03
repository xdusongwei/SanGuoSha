/*
 * AskForCore.cs
 * Namespace: SanGuoSha.Contest.Global
 * 提供问询系统和消息系统复合的类
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using SanGuoSha.Contest.Data;
using SanGuoSha.Contest.Equipage;
using System.IO;
using SanGuoSha.Contest.Data.GameException;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Global
{
    /// <summary>
    /// 问询和消息系统          
    /// </summary>
    public class MessageCore
    {
        /// <summary>
        /// 游戏基类
        /// </summary>
        private GameBase GB;

        /// <summary>
        /// 对象创建时间
        /// </summary>
        private DateTime _dtCreate = DateTime.Now;

        /// <summary>
        /// 获取修改消息链所需要的锁
        /// </summary>
        internal object MessageLocker = new object();

        /// <summary>
        /// 构造问询对象
        /// </summary>
        /// <param name="aGB">游戏的基类</param>
        public MessageCore(GameBase aGB)
        {
            GB = aGB;
        }

        /// <summary>
        /// 问询的内容枚举
        /// </summary>
        public enum AskForEnum
        { 
            /// <summary>
            /// 玩家选择自己的武将
            /// </summary>
            PlayerChief,
            /// <summary>
            /// 问询玩家是否
            /// </summary>
            YN, 
            /// <summary>
            /// 问询玩家闪
            /// </summary>
            Shan, 
            /// <summary>
            /// 问询玩家杀
            /// </summary>
            Sha,
            /// <summary>
            /// 问询玩家弃牌
            /// </summary>
            Abandonment,
            /// <summary>
            /// 问询玩家继续弃牌，AbandonmentNext会保持上次的问询时间，但在通信时会转化成问询 Abandonment
            /// </summary>
            AbandonmentNext, 
            /// <summary>
            /// 问询玩家任何适合在出牌阶段使用的牌
            /// </summary>
            Aggressive, 
            /// <summary>
            /// 问询玩家一张目标玩家的牌,包含判定区
            /// </summary>
            TargetCardWithJudgementArea, 
            /// <summary>
            /// 问询玩家一张目标玩家的牌，不包含判定区
            /// </summary>
            TargetCard,
            /// <summary>
            /// 问询玩家一张目标玩家的手牌
            /// </summary>
            TargetHand , 
            /// <summary>
            /// 问询玩家n张手牌,用于自己选择自己的一定量手牌的一种问询.数量为-1时表示任意张
            /// </summary>
            TargetHands,
            /// <summary>
            /// 从目标中选两张牌,除了武器和判定区
            /// </summary>
            TargetTwoCardsWithoutWeaponAndJudgement,
            /// <summary>
            /// 选择目标的马装备
            /// </summary>
            TargetHorse,
            /// <summary>
            /// 问询玩家无懈可击
            /// </summary>
            WuXieKeJi,
            /// <summary>
            /// 问询玩家的两张手牌
            /// </summary>
            TwoHandCards,
            /// <summary>
            /// 问询玩家从五谷丰登牌堆中选择一张牌
            /// </summary>
            WuGuFengDeng,
            /// <summary>
            /// 问询求一个桃
            /// </summary>
            AskForTao,
            /// <summary>
            /// 问询求一个桃或者酒
            /// </summary>
            AskForTaoOrJiu,
            /// <summary>
            /// 牌槽中的牌,除了五谷丰登牌槽,牌槽都用于技能操作,所以牌槽处理不属于异步的默认检查,请在ActiveSkill事件中自行检查
            /// </summary>
            SlotCards,
            /// <summary>
            /// 花色
            /// </summary>
            Suit
        };
        
        /// <summary>
        /// 玩家的阶段状态
        /// </summary>
        internal enum PlayerStatus
        { 
            /// <summary>
            /// 回合开始
            /// </summary>
            Start , 
            /// <summary>
            /// 判定阶段
            /// </summary>
            Judgment, 
            /// <summary>
            /// 拿牌阶段
            /// </summary>
            Take , 
            /// <summary>
            /// 出牌阶段
            /// </summary>
            Lead , 
            /// <summary>
            /// 弃牌阶段
            /// </summary>
            Abandoment};


        /// <summary>
        /// 问询玩家是否
        /// </summary>
        /// <param name="aChief">玩家的角色</param>
        /// <returns>返回问询结果</returns>
        internal AskForResult AskForYN(ChiefBase aChief)
        {
            GB.CommService.AskForCards(GB, GB.GamePlayers[aChief], AskForEnum.YN);
            return new AskForResult(GB.Response.IsTimeout , GB.Response.Source == null ? null : GB.Response.Source.Chief, Player.Players2Chiefs(GB.Response.Targets), GB.Response.Cards, GB.Response.Effect , GB.Response.Answer ,false , string.Empty);
        }

        /// <summary>
        /// 向所有玩家发送带有表决的出牌问询
        /// </summary>
        /// <param name="aAskFor">问询内容</param>
        /// <param name="aAbstention">表决字典</param>
        /// <returns>返回问询结果</returns>
        internal AskForResult AskForCards(AskForEnum aAskFor, Dictionary<Player, bool> aAbstention)
        {
            GB.CommService.AskForCards(GB, null, aAskFor, null, aAbstention ,0);
            return new AskForResult(GB.Response.IsTimeout, GB.Response.Source.Chief, Player.Players2Chiefs(GB.Response.Targets), GB.Response.Cards, GB.Response.Effect, GB.Response.Answer , false, string.Empty);
        }

        /// <summary>
        /// 问询玩家一个玩家出牌
        /// </summary>
        /// <param name="aChief">被问询的玩家角色</param>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aChiefTo">目标</param>
        /// <returns>返回问询结果</returns>
        internal AskForResult AskForCards(ChiefBase aChief, AskForEnum aAskFor, ChiefBase aChiefTo)
        {
            GB.CommService.AskForCards(GB, GB.GamePlayers[aChief], aAskFor, GB.GamePlayers[aChiefTo]);
            return new AskForResult(GB.Response.IsTimeout, GB.Response.Source.Chief, Player.Players2Chiefs(GB.Response.Targets), GB.Response.Cards, GB.Response.Effect, GB.Response.Answer, true, GB.Response.SkillName);
        }

        /// <summary>
        /// 问询玩家一个玩家出牌
        /// </summary>
        /// <param name="aChief">被问询的玩家角色</param>
        /// <param name="aCount">问询的牌数量</param>
        /// <returns>返回问询结果</returns>
        internal AskForResult AskForCardsWithCount(ChiefBase aChief, int aCount)
        {
            GB.CommService.AskForCards(GB, GB.GamePlayers[aChief], AskForEnum.TargetHands, GB.GamePlayers[aChief], null, aCount);
            return new AskForResult(GB.Response.IsTimeout, GB.Response.Source.Chief, Player.Players2Chiefs(GB.Response.Targets), GB.Response.Cards, GB.Response.Effect, GB.Response.Answer, true, GB.Response.SkillName);
        }

        /// <summary>
        /// 问询选将
        /// </summary>
        /// <param name="aAbstention">表决字典，需要选将的玩家值置false，其他置true</param>
        internal void AskForChief(Dictionary<Player, bool> aAbstention)
        {
            GB.CommService.AskForCards(GB, null, AskForEnum.PlayerChief, null, aAbstention ,0);
        }

        /// <summary>
        /// 问询玩家出牌
        /// </summary>
        /// <param name="aChief">被问询的玩家角色</param>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aWrapper"></param>
        /// <param name="aData"></param>
        /// <returns>返回问询结果</returns>
        internal AskForResult AskForCards(ChiefBase aChief, AskForEnum aAskFor, AskForWrapper aWrapper,  GlobalData aData)
        {
            return AskForCards(aChief, aAskFor, aWrapper , true, aData);
        }

        /// <summary>
        /// 问询玩家出牌
        /// </summary>
        /// <param name="aChief">被问询的玩家角色</param>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aActiveArmor">指示是否让防具来参与问询的回应</param>
        /// <param name="aWrapper"></param>
        /// <param name="aData"></param>
        /// <returns>返回问询结果</returns>
        internal AskForResult AskForCards(ChiefBase aChief, AskForEnum aAskFor, AskForWrapper aWrapper ,bool aActiveArmor  ,GlobalData aData)
        {
            AskForResult ret = null;
            
            if (aActiveArmor && aData.Game.GamePlayers[aChief].Armor != null)
            {
                if (aAskFor == AskForEnum.Shan)
                {
                    ret = Armor.AskFor(Card.Effect.Shan, aData.Game.GamePlayers[aChief].Armor.CardEffect, aChief, aData);
                    if (ret != null && ret.Effect != Card.Effect.None) return ret;
                }
            }
            
            //通知武将技能问询开始
            foreach (ASkill s in aChief.Skills)
            {
                s.BeforeAskfor(aChief, aAskFor, aData);
            }
            while (true)
            {
                aWrapper.SendAskFor(this);
                //使用通信方法问询玩家选择一张问询结果对应的牌
                GB.CommService.AskForCards(GB, GB.GamePlayers[aChief], aAskFor);
                if (aData.Game.Response.SkillName != string.Empty)
                {
                    //将问询通知该武将的技能，若回应非None效果，则将此Result作为结果返回
                    foreach (ASkill s in aChief.Skills)
                    {
                        try
                        {
                            AskForResult res = s.AskFor(aChief, aAskFor, aData);
                            if (res != null && res.Effect != Card.Effect.None)
                            {
                                ret = res;
                                goto FINISH;
                            }
                        }
                        catch (SkillInvalid e)
                        {
                            //return new AskForResult(false, aChief, new ChiefBase[] { }, new Card[] { }, Card.Effect.None, false, false, string.Empty);
                            continue;
                        }
                        catch (SkillFatalError e)
                        {
                            return new AskForResult(false, aChief, [], [], Card.Effect.None, false, false, string.Empty);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
                else
                    break;
            }
            FINISH:
            //通知武将技能问询结束
            foreach (ASkill s in aChief.Skills)
            {
                s.FinishAskfor(aChief, aAskFor, aData);
            }
            //返回问询结果
            if (ret != null && ret.Effect != Card.Effect.None)
                return ret;
            else
                return new AskForResult(GB.Response.IsTimeout, aChief, Player.Players2Chiefs(GB.Response.Targets), GB.Response.Cards, GB.Response.Effect, GB.Response.Answer, true, GB.Response.SkillName);
        }

        /// <summary>
        /// 通知玩家状态改变
        /// </summary>
        /// <param name="aChief"></param>
        /// <param name="aStatus"></param>
        internal void SendChangeStatusMessage(ChiefBase aChief, PlayerStatus aStatus)
        {
            SendMessage(new Beaver("status", aChief.ChiefName, aStatus.ToString()).ToString()
                //new XElement("status",
                //    new XElement("target" , aChief.ChiefName),
                //    new XElement("status" , aStatus)
                //    )
                );
            Thread.Sleep(250);
        }

        /// <summary>
        /// 通知武将所在的玩家出牌有效
        /// </summary>
        /// <param name="aChief">武将对象</param>
        internal void LeadingValid(ChiefBase aChief)
        {
            GB.GamePlayers[aChief].Callback.LeadingValid();
        }

        /// <summary>
        /// 指示武将出牌无效,恢复其原有的手牌和装备
        /// </summary>
        /// <param name="aChief">武将对象</param>
        internal void LeadingInvalid(ChiefBase aChief)
        {
            List<int> IDs = [];
            foreach(Card c in GB.GamePlayers[aChief].Hands)
                IDs.Add(c.ID);
            GB.GamePlayers[aChief].Callback.LeadingInvalid([.. IDs], GB.GamePlayers[aChief].Weapon == null ? 0 : GB.GamePlayers[aChief].Weapon.ID, GB.GamePlayers[aChief].Armor == null ? 0 : GB.GamePlayers[aChief].Armor.ID,
                GB.GamePlayers[aChief].Jia1Ma == null ? 0 : GB.GamePlayers[aChief].Jia1Ma.ID, GB.GamePlayers[aChief].Jian1Ma == null ? 0 : GB.GamePlayers[aChief].Jian1Ma.ID);
        }

        /// <summary>
        /// 发送对所有武将和观看者的消息
        /// </summary>
        /// <param name="aMessage">消息内容</param>
        public void SendMessage(string aMessage)
        {
            SendMessage(GB.GamePlayers.All.ToArray(), aMessage, true);
        }

        /// <summary>
        /// 发送对于某些武将的消息
        /// </summary>
        /// <param name="aChiefs">武将</param>
        /// <param name="aMessage">消息</param>
        /// <param name="aCommon">是否需要将消息放置到公共事件链中，放置到公共事件链中意味着观看者也能收到</param>
        public void SendMessage(ChiefBase[] aChiefs, string aMessage, bool aCommon)
        {
            SendMessage(ChiefBase.Chiefs2Players(aChiefs , GB.GamePlayers), aMessage, aCommon);
        }

        /// <summary>
        /// 发送对于某些玩家的消息
        /// </summary>
        /// <param name="aPlayers">玩家</param>
        /// <param name="aMessage">消息</param>
        /// <param name="aCommon">是否需要将消息放置到公共事件链中，放置到公共事件链中意味着观看者也能收到</param>
        public void SendMessage(Player[] aPlayers, string aMessage, bool aCommon)
        {
            lock (MessageLocker)
            {
                //将这个消息放置到各个玩家的事件链中
                foreach (Player p in aPlayers)
                {
                    p.Messages.Add(aMessage);
                }
                //需要添加进公共事件，这里将消息放置到公共事件链中
                if (aCommon)
                {
                    GB.EventChain.Add(aMessage);
                }
            }
            Thread.Sleep(10);
            //通过通信对象发送消息
            GB.CommService.SendMessage(aPlayers, aMessage);
            //写日志
            string file = System.AppDomain.CurrentDomain.BaseDirectory + string.Format("TID{0}_{1:D2}H{2:D2}M{3:D2}S.txt", Thread.CurrentThread.ManagedThreadId	
.ToString(), _dtCreate.Hour, _dtCreate.Minute, _dtCreate.Second);
            File.AppendAllText(file, aMessage.ToString() + "\r\n\r\n");
        }

        /// <summary>
        /// 发送转移牌的消息
        /// </summary>
        /// <param name="aFrom">牌的来源武将</param>
        /// <param name="aTo">牌的目标武将</param>
        /// <param name="aCards">牌</param>
        /// <param name="aPlayers">玩家集合</param>
        internal void SendStealMessage(ChiefBase aFrom, ChiefBase aTo, Card[] aCards , Players  aPlayers)
        {
            List<Card> vir = [];
            foreach (Card c in aCards)
            {
                if (aPlayers[aFrom].Hands.Contains(c))
                    vir.Add(CardHeap.Unknown);
                else
                    vir.Add(c);
            }
            SendMessage(aPlayers.All.Where((i) => i.Chief != aFrom && i.Chief != aTo).ToArray(), MessageCore.MakeStealMessage(aFrom, aTo, [.. vir]), true);

            SendMessage(new ChiefBase[] { aFrom, aTo }, MessageCore.MakeStealMessage(aFrom, aTo, aCards), false);
        }

        internal void SendPickMessage(ChiefBase aChief, Card[] aCards)
        {
            SendMessage(MessageCore.MakePickMessage(aChief, aCards));
        }

        internal void SendPrivateMessageWithOpenMessage(ChiefBase aChief, string aPrivateMessage, string aOpenMessage, Players aPlayers)
        {
            SendMessage(aPlayers.All.Where((i) => i.Chief != aChief).ToArray(), aOpenMessage, true);

            SendMessage(new ChiefBase[] { aChief }, aPrivateMessage, false);
        }

        internal void SendPrivateMessageWithOpenMessage(ChiefBase aChief, ChiefBase aChief2, string aPrivateMessage, string aOpenMessage, Players aPlayers)
        {
            SendMessage(aPlayers.All.Where((i) => i.Chief != aChief && i.Chief != aChief2).ToArray(), aOpenMessage, true);

            SendMessage(new ChiefBase[] { aChief }, aPrivateMessage, false);
        }

        /// <summary>
        /// 发送转移牌的消息
        /// </summary>
        /// <param name="aFrom">牌的来源武将</param>
        /// <param name="aTo">牌的目标武将</param>
        /// <param name="aCards">牌</param>
        /// <param name="aPlayers">玩家集合</param>
        internal void SendGiveMessage(ChiefBase aFrom, ChiefBase aTo, Card[] aCards, Players aPlayers)
        {
            List<Card> vir = [];
            foreach (Card c in aCards)
            {
                if (aPlayers[aFrom].Hands.Contains(c))
                    vir.Add(CardHeap.Unknown);
                else
                    vir.Add(c);
            }
            SendMessage(aPlayers.All.Where((i) => i.Chief != aFrom && i.Chief != aTo).ToArray(), MessageCore.MakeGiveMessage(aFrom, aTo, [.. vir]), true);

            SendMessage(new ChiefBase[] { aFrom, aTo }, MessageCore.MakeGiveMessage(aFrom, aTo, aCards), false);
        }

        /// <summary>
        /// 产生所有玩家状态的XML消息
        /// </summary>
        /// <param name="aChief">产生信息的武将对象,对于请求公共信息,这里使用null</param>
        /// <returns>XML消息</returns>
        internal string MakeEnvironmentXMLReport(ChiefBase aChief)
        {
            Beaver env = [];
            Beaver players = [];
            Beaver chain = [];
            Beaver hands = null;
            foreach (Player p in GB.GamePlayers.All)
            {
                hands = new Beaver("hands");
                Beaver debuffs = [];
                Beaver skills = [];
                foreach (Card c in p.Debuff)
                    debuffs.Add(string.Empty, new Beaver(c.CardEffect.ToString(), c.ID));
                        //new XElement("effect", c.CardEffect),
                        //new XElement("card", c.ID)
                        //));
                debuffs.SetHeaderElementName("debuffs");
                if (p.Chief != null)
                {
                    foreach (SkillBase s in p.Chief.Skills)
                    {
                        skills.Add(string.Empty , new Beaver(string.Empty , s.SkillName, s.SkillStatus.ToString()));// new XElement("skill", new XElement("name", s.SkillName), new XElement("status", s.SkillStatus)));
                    }
                    if (p.Chief == aChief)
                    {
                        lock (GB.AsynchronousCore.MessageLocker)
                        {
                            foreach (string item in GB.EventChain)
                            {
                                chain.Add(item);
                            }
                        }
                        hands = MakeHandMessage(p);
                    }
                    skills.SetHeaderElementName("skills");
                }
                if (p.Chief != null && p.Chief == aChief)
                {
                    Beaver player = new Beaver
                    {
                        { string.Empty, p.UID, p.PlayerName, p.Dead.ToString(), p.MaxHealth.ToString(), p.Health.ToString() },
                        {
                            string.Empty,
                            hands,
                            debuffs,
                            skills,
                            p.Chief == null ? string.Empty : p.Chief.ChiefName,
                            p.Chief != null && p.Chief.ChiefStatus == ChiefBase.Status.Majesty ? ChiefBase.Status.Majesty.ToString() : p.Chief != null && p.Dead ? p.Chief.ChiefStatus.ToString() : ChiefBase.Status.Unknown.ToString()
                        }
                    };
                    if(p.Weapon != null ) 
                        player.Add("weapon" , p.Weapon.ID);
                    if(p.Armor != null )
                        player.Add("armor" , p.Armor.ID);
                    if(p.Jia1Ma != null )
                        player.Add("jia1" , p.Jia1Ma.ID);
                    if(p.Jian1Ma != null) 
                        player.Add("jian1" , p.Jian1Ma.ID);
                    players.Add(string.Empty, player);
                        //new XElement("player",
                        //    new XElement("UID", p.UID),
                        //    new XElement("name", p.PlayerName),
                        //    new XElement("dead", p.Dead),
                        //    new XElement("max_health", p.MaxHealth),
                        //    new XElement("health", p.Health),
                        //    p.Weapon == null ? new XElement("weapon") :new XElement("weapon", p.Weapon.ID),
                        //    p.Armor == null ?new XElement("armor") :new XElement("armor" , p.Armor.ID),
                        //    p.Jia1Ma == null ?  new XElement("jia1"):new XElement("jia1",p.Jia1Ma.ID),
                        //    p.Jian1Ma == null ? new XElement("jian1") : new XElement("jian1", p.Jian1Ma.ID),
                        //    hands,
                        //    debuffs,
                        //    new XElement("chief_name", p.Chief == null ? string.Empty : p.Chief.ChiefName),
                        //    new XElement("status", p.Chief != null && p.Chief.ChiefStatus == ChiefBase.Status.Majesty ? ChiefBase.Status.Majesty : p.Chief != null && p.Dead ? p.Chief.ChiefStatus : ChiefBase.Status.Unknown),
                        //    skills
                        //));
                }
                else
                {
                    Beaver player = new Beaver
                    {
                        { string.Empty, p.UID, p.PlayerName, p.Dead.ToString(), p.MaxHealth.ToString(), p.Health.ToString() },
                        {
                            string.Empty,
                            p.Hands.Count,
                            debuffs,
                            skills,
                            p.Chief == null ? string.Empty : p.Chief.ChiefName,
                            p.Chief != null && p.Chief.ChiefStatus == ChiefBase.Status.Majesty ? ChiefBase.Status.Majesty.ToString() : p.Chief != null && p.Dead ? p.Chief.ChiefStatus.ToString() : ChiefBase.Status.Unknown.ToString()
                        }
                    };
                    if (p.Weapon != null)
                        player.Add("weapon", p.Weapon.ID);
                    if (p.Armor != null)
                        player.Add("armor", p.Armor.ID);
                    if (p.Jia1Ma != null)
                        player.Add("jia1", p.Jia1Ma.ID);
                    if (p.Jian1Ma != null)
                        player.Add("jian1", p.Jian1Ma.ID);
                    players.Add(string.Empty, player);
                    //players.Add(
                    //    new XElement("player",
                    //        new XElement("UID", p.UID),
                    //        new XElement("name", p.PlayerName),
                    //        new XElement("dead", p.Dead),
                    //        new XElement("max_health", p.MaxHealth),
                    //        new XElement("health", p.Health),
                    //        p.Weapon == null ? new XElement("weapon") : new XElement("weapon", p.Weapon.ID),
                    //        p.Armor == null ? new XElement("armor") : new XElement("armor", p.Armor.ID),
                    //        p.Jia1Ma == null ? new XElement("jia1") : new XElement("jia1", p.Jia1Ma.ID),
                    //        p.Jian1Ma == null ? new XElement("jian1") : new XElement("jian1", p.Jian1Ma.ID),
                    //        new XElement("hands_count", p.Hands.Count),
                    //        debuffs,
                    //        new XElement("chief_name", p.Chief == null ? string.Empty : p.Chief.ChiefName),
                    //        new XElement("status", p.Chief != null && p.Chief.ChiefStatus == ChiefBase.Status.Majesty ? ChiefBase.Status.Majesty : p.Chief != null && p.Dead ? p.Chief.ChiefStatus : ChiefBase.Status.Unknown),
                    //        skills
                    //    ));
                }
            }
            if (aChief == null)
            {
                lock (GB.EventChainLocker)
                {
                    foreach (string item in GB.EventChain)
                    {
                        chain.Add(string.Empty, item);
                    }
                }
            }
            
            //env.Add(players);
            //env.Add(new XElement("latency", GB.WaittingData.Latency));
            //env.Add(new XElement("mode", GB.Mode));
            //env.Add(chain);
            //env.Add(new XElement("active" , GB.gData.Active == null ? string.Empty : GB.gData.Active.ChiefName));
            //env.Add(new XElement("status" , GB.gData.ChiefStatus));

            env.Add(string.Empty, players);
            env.Add("common", GB.WaittingData.Latency, GB.Mode.ToString(), chain, GB.gData.Active == null ? string.Empty : GB.gData.Active.ChiefName , GB.gData.ChiefStatus);
            env.SetHeaderElementName("environment");
            return env.ToString();
        }

        /// <summary>
        /// 产生某个玩家手牌序列Beaver消息
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <returns>Beaver对象</returns>
        private Beaver MakeHandMessage(Player aPlayer)
        {
            Beaver ret = [];
            foreach (Card c in aPlayer.Hands)
            {
                ret.Add(string.Empty, c.ID);
            }
            ret.SetHeaderElementName("hands");
            return ret;
        }

        /// <summary>
        /// 发送所有玩家情况的消息
        /// </summary>
        public void SendEnvironmentMessage()
        {
            
            foreach (Player p in GB.GamePlayers.All)
            {
                SendMessage(new Player[] { p }, MakeEnvironmentXMLReport(p.Chief), false);
                //SendMessage(new Player[]{ p } , MakeHandMessage(p), false);
            }
        }

        /// <summary>
        /// 发送事件完毕的消息，同时清除所有玩家的事件链
        /// </summary>
        public void SendClearMessage()
        {
            lock (MessageLocker)
            {
                foreach (Player p in GB.GamePlayers.All)
                {
                    p.Messages.Clear();
                }
                GB.EventChain.Clear();
            }
            SendMessage(new Beaver("clear" , string.Empty).ToString());// new XElement("clear"));
        }

        /// <summary>
        /// 问询结果
        /// </summary>
        public class AskForResult
        {
            /// <summary>
            /// 事件的产生者
            /// </summary>
            public readonly ChiefBase Leader;
            /// <summary>
            /// 事件的目标
            /// </summary>
            public readonly ChiefBase[] Targets;
            /// <summary>
            /// 事件目标回应的牌
            /// </summary>
            public readonly Card[] Cards;
            /// <summary>
            /// 回应的牌的效果
            /// </summary>
            public readonly Card.Effect Effect;
            /// <summary>
            /// 是否超时
            /// </summary>
            public readonly bool TimeOut;
            /// <summary>
            /// 回复的是否结果
            /// </summary>
            public readonly bool YN;
            /// <summary>
            /// 判断这个回应是玩家出牌造成的,如果是,框架处理的时候会顺带将牌从手牌中移除
            /// </summary>
            public readonly bool PlayerLead;
            /// <summary>
            /// 若回应中武将技能参与，这里保存发动的武将技能
            /// </summary>
            public readonly string SkillName;

            /// <summary>
            /// 问询结果
            /// </summary>
            /// <param name="aTimeout">是否超时</param>
            /// <param name="aLeader">问询目标</param>
            /// <param name="aTargets">目标的目标</param>
            /// <param name="aCards">问询目标出牌</param>
            /// <param name="aEffect">出牌的效果</param>
            /// <param name="aYN">问询的是否结果</param>
            /// <param name="aPlayerLead">问询是否是玩家所选出牌，针对八卦阵</param>
            /// <param name="aSkillName">问询引发的技能名称</param>
            public AskForResult(bool aTimeout , ChiefBase aLeader, ChiefBase[] aTargets, Card[] aCards, Card.Effect aEffect , bool aYN , bool aPlayerLead , string aSkillName)
            {
                TimeOut = aTimeout;
                Leader = aLeader;
                Targets = aTargets;
                Cards = aCards;
                Effect = aEffect;
                YN = aYN;
                PlayerLead = aPlayerLead;
                SkillName = aSkillName;
            }
        }

        /// <summary>
        /// 产生一条触发技能的问询
        /// </summary>
        /// <param name="aChief">需要触发技能的武将</param>
        /// <param name="aSkill">需要触发的技能</param>
        /// <returns>一条问询消息</returns>
        internal static string MakeAskForSkillMessage(ChiefBase aChief, SkillBase aSkill)
        {
            return new Beaver("askfor.skill", aChief.ChiefName, aSkill.SkillName).ToString();
            //return new XElement("askfor.skill",
            //            new XElement("target", aChief.ChiefName),
            //            new XElement("skill", aSkill.SkillName));
        }


        /// <summary>
        /// 产生触发技能的消息
        /// </summary>
        /// <param name="aChief">触发技能的武将</param>
        /// <param name="aSkill">激发的技能</param>
        /// <param name="aTargets">目标</param>
        /// <param name="aCards">牌</param>
        /// <returns>一条技能触发的消息</returns>
        internal static string MakeTriggerSkillMesssage(ChiefBase aChief, SkillBase aSkill, ChiefBase[] aTargets, Card[] aCards)
        {
            return new Beaver("skill", aChief.ChiefName, aSkill.SkillName, ChiefBase.Chiefs2Beaver("targets", aTargets), Card.Cards2Beaver("cards", aCards)).ToString();
            //return new XElement("skill",
            //                new XElement("target", aChief.ChiefName),
            //                new XElement("name", aSkill.SkillName),
            //                ChiefBase.Chiefs2XML("targets" , aTargets),
            //                Card.Cards2XML("cards", aCards)
            //            );
        }

        /// <summary>
        /// 产生一条drop消息
        /// </summary>
        /// <param name="aFrom">选择牌的玩家</param>
        /// <param name="aTo">被选择牌的玩家</param>
        /// <param name="aCards">弃掉的牌</param>
        /// <returns>drop消息</returns>
        internal static string MakeDropMessage(ChiefBase aFrom, ChiefBase aTo, Card[] aCards)
        {
            return new Beaver("drop", aFrom.ChiefName, aTo.ChiefName, Card.Cards2Beaver("cards", aCards)).ToString();
            //return new XElement("drop",
            //                new XElement("from", aFrom.ChiefName),
            //                new XElement("to", aTo.ChiefName),
            //                Card.Cards2XML("cards", aCards)
            //            );
        }

        /// <summary>
        /// 一条从打出的牌中拾取牌的消息
        /// </summary>
        /// <param name="aChief">拾取牌的武将</param>
        /// <param name="aCards">拾取的牌</param>
        /// <returns></returns>
        internal static string MakePickMessage(ChiefBase aChief, Card[] aCards)
        {
            return new Beaver("pick", aChief.ChiefName, Card.Cards2Beaver("cards", aCards)).ToString();
            //return new XElement("pick",
            //                new XElement("to", aChief.ChiefName),
            //                Card.Cards2XML("cards", aCards)
            //            );
        }

        /// <summary>
        /// 产生一条steal消息
        /// </summary>
        /// <param name="aFrom">牌原先的拥有者</param>
        /// <param name="aTo">牌现在的拥有者</param>
        /// <param name="aCards">牌</param>
        /// <returns>steal消息</returns>
        internal static string MakeStealMessage(ChiefBase aFrom, ChiefBase aTo, Card[] aCards)
        {
            return new Beaver("steal", aFrom.ChiefName, aTo.ChiefName, Card.Cards2Beaver("cards", aCards)).ToString();
            //return new XElement
            //                (
            //                "steal",
            //                    new XElement("from", aFrom.ChiefName),
            //                    new XElement("to", aTo.ChiefName),
            //                    Card.Cards2XML("cards", aCards)
            //                );
        }

        /// <summary>
        /// 产生一条give消息
        /// </summary>
        /// <param name="aFrom">牌原先的拥有者</param>
        /// <param name="aTo">牌现在的拥有者</param>
        /// <param name="aCards">牌</param>
        /// <returns>steal消息</returns>
        internal static string MakeGiveMessage(ChiefBase aFrom, ChiefBase aTo, Card[] aCards)
        {
            return new Beaver("give", aFrom.ChiefName, aTo.ChiefName, Card.Cards2Beaver("cards", aCards)).ToString();
            //return new XElement
            //                (
            //                "give",
            //                    new XElement("from", aFrom.ChiefName),
            //                    new XElement("to", aTo.ChiefName),
            //                    Card.Cards2XML("cards", aCards)
            //                );
        }

    }

    

    /// <summary>
    /// 问询消息的包装器
    /// </summary>
    internal class AskForWrapper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aChiefs">需要接收问询的武将</param>
        /// <param name="aMessage">问询内容</param>
        /// <param name="aCommon">是否将消息放入公共事件链中</param>
        /// <param name="aGame">游戏对象</param>
        public AskForWrapper(ChiefBase[] aChiefs, string aMessage, bool aCommon , GameBase aGame)
            :this(ChiefBase.Chiefs2Players(aChiefs , aGame.GamePlayers), aMessage , aCommon )
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aPlayers">需要接收问询的玩家</param>
        /// <param name="aMessage">问询内容</param>
        /// <param name="aCommon">是否将消息放入公共事件链中</param>
        public AskForWrapper(Player[] aPlayers, string aMessage, bool aCommon)
        {
            Targets = aPlayers;
            Message = aMessage;
            Common = aCommon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aMessage">问询内容</param>
        /// <param name="aGame">游戏对象</param>
        public AskForWrapper(string aMessage, GameBase aGame)
            :this(aGame.GamePlayers.All , aMessage , true)
        {

        }

        public Player[] Targets
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public bool Common
        {
            get;
            private set;
        }

        /// <summary>
        /// 发送问询
        /// </summary>
        public void SendAskFor(MessageCore aCore)
        {
            aCore.SendMessage(Targets, Message, Common);
        }
    }
}
