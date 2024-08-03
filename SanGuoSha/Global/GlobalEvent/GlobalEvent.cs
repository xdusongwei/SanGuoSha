/*
 * GlobalEvent.cs
 * Namespace: SanGuoSha.ServerCore.Contest.Global
 * GlobalEvent类继承GameBase类,与GameBase类的数据和组件任务不同,GlobalEvent类作为一般效果的处理区域
 * 同时,GlobalEvent是一个巨大的方法集,也是三层框架中的方法层,所以描述每一个方法都显得很必要,
 * 它的很多方法被分割成其他文件放置在GlobalEvent文件夹内,但是同属GlobalEvent类
 * GlobalEvent中大部分方法是为其他组件部分提供处理行为的基本功能
 * 
 * 事件层的运行机制包含逻辑泵和事件链两种概念
 * 逻辑泵就是将游戏轮询武将并使其进入各个阶段的实现方法,并根据请求的情况配置事件链来进行处理
 * 事件链可以说是一个队列和一个链表组成,对于各个游戏阶段第一个返回的请求,将转换成单对单(武将)的子事件
 * 子事件按照队列结构进行存储,之后对每个子事件进行处理,处理子事件是一个链式过程,意味着互动操作可以使子事件的处理变成多重的或者递归的
 * 
 * 对于牌的处理，有垃圾桶容器和EventNode,Move两种操作来处理
 * 其他的,GlobalEvnet类提供了很多实用方法,用于进行技能层面的和执行层面操作
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Global
{
    /// <summary>
    /// 游戏对象的中间层基类,方法层
    /// 这个基类主要负责维护逻辑泵
    /// </summary>
    public partial class GlobalEvent : GameBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GlobalEvent()
        {
            
        }

        #region 游戏处理的主框架
        /// <summary>
        /// 角色求救过程
        /// </summary>
        /// <param name="aChiefSource">伤害的来源,可以是null</param>
        /// <param name="aPreDefunct">求救武将</param>
        /// <param name="aRescuePoint">求救血量</param>
        /// <returns>返回true表示该角色求救失败,false表示求救成功</returns>
        private bool Cry4HelpProc(ChiefBase aChiefSource, ChiefBase aPreDefunct, sbyte aRescuePoint)
        {
            if (aPreDefunct == null || aRescuePoint < 1) return true;
            ChiefBase start = aChiefSource != null ? aChiefSource : aPreDefunct;
            ChiefBase t = start;
            string msg = null;
            do
            {
                MessageCore.AskForResult res = null;
                if (t == aPreDefunct)
                {
                    msg = new Beaver("askfor.cry4help.taoorjiu", t.ChiefName, aPreDefunct.ChiefName, aRescuePoint.ToString()).ToString();
                        //new XElement("askfor.cry4help.taoorjiu",
                        //    new XElement("target", t.ChiefName),
                        //    new XElement("target2", aPreDefunct.ChiefName),
                        //    new XElement("rescuepoint", aRescuePoint)
                        //);
                    res = AsynchronousCore.AskForCards(t, MessageCore.AskForEnum.AskForTaoOrJiu,new AskForWrapper(msg , this) , gData);
                    AsynchronousCore.LeadingValid(t);
                }
                else
                {
                    msg = new Beaver("askfor.cry4help.tao", t.ChiefName, aPreDefunct.ChiefName, aRescuePoint.ToString()).ToString();
                        //new XElement("askfor.cry4help.tao",
                        //    new XElement("target", t.ChiefName),
                        //    new XElement("target2", aPreDefunct.ChiefName),
                        //    new XElement("rescuepoint", aRescuePoint)
                        //);
                    res = AsynchronousCore.AskForCards(t, MessageCore.AskForEnum.AskForTao, new AskForWrapper(msg, this), gData);
                    AsynchronousCore.LeadingValid(t);
                }
                ValidityResult(t , ref res);
                if (res.Effect != Card.Effect.None)
                {
                    if (res.Effect == Card.Effect.Tao)
                    {
                        AsynchronousCore.SendMessage(
                            new Beaver("tao" , t.ChiefName ,aChiefSource.ChiefName , res.SkillName , Card.Cards2Beaver("cards" , res.Cards )).ToString());
                        //    new XElement("tao",
                        //        new XElement("from", t.ChiefName),
                        //        new XElement("to", aChiefSource.ChiefName),
                        //        new XElement("skill" , res.SkillName ),
                        //        Card.Cards2XML("cards", res.Cards)
                        //    )
                        //);
                    }
                    else if (res.Effect == Card.Effect.Jiu)
                    {
                        AsynchronousCore.SendMessage( new Beaver("jiu" , t.ChiefName , res.SkillName , Card.Cards2Beaver("cards" , res.Cards )).ToString());
                        //new XElement("jiu",
                        //    new XElement("from", t.ChiefName),
                        //    new XElement("skill", res.SkillName),
                        //    Card.Cards2XML("cards", res.Cards)
                        //)
                        //);
                    }
                    if (res.Effect == Card.Effect.Tao || res.Effect == Card.Effect.Jiu)
                    {
                        sbyte cost = 1;
                        foreach (ASkill s in aPreDefunct.Skills)
                        {
                            cost = s.CalcRescuePoint(aPreDefunct, res.Leader, res.Effect, cost, gData);
                        }
                        aRescuePoint -= cost;
                        if (res.PlayerLead)
                            DropCards(true, CardFrom.HandAndEquipage, res.SkillName, res.Cards, res.Effect, t, aPreDefunct, null);
                        if (aRescuePoint < 1) break;
                        continue;
                    }
                }
                t = GamePlayers.NextChief(t);
            } while (!t.IsMe(start));
            --aRescuePoint;
            if (aRescuePoint < 0)
            {
                GamePlayers[aPreDefunct].Health = Math.Abs(aRescuePoint);
                AsynchronousCore.SendMessage( new Beaver("health" , aPreDefunct.ChiefName , GamePlayers[aPreDefunct].MaxHealth.ToString() , GamePlayers[aPreDefunct].Health.ToString()).ToString());
                    //new XElement("health",
                    //    new XElement("target", aPreDefunct.ChiefName),
                    //    new XElement("max", GamePlayers[aPreDefunct].MaxHealth),
                    //    new XElement("current", GamePlayers[aPreDefunct].Health)
                    //    ));
            }
            else
                GamePlayers[aPreDefunct].Health = 0;
            return aRescuePoint < 0 ? false : true;
        }

        /// <summary>
        /// 子事件处理方法
        /// </summary>
        private void EventProc()
        {
            if (queRecoard.Count < 1) return;
            //清除子事件处理列表
            lstRecoard.Clear();
            //取子事件队列的事件放置到子事件处理列表中
            EventRecoard r = queRecoard.Dequeue();
            lstRecoard.Add(r);

            //根据首事件的性质开始处理
            switch (r.Effect)
            {
                //...None效果一般是说,某些技能已经处理过了,也不想让这个事件走一般过程,于是有了None来跳过子事件的处理
                case Card.Effect.None:

                    break;
                //杀
                case Card.Effect.Sha:
                    r = ShaProc(r);
                    break;
                //决斗
                case Card.Effect.JueDou:
                    r = JueDouProc(r);
                    break;
                //桃
                case Card.Effect.Tao:
                    r = TaoProc(r);
                    break;
                //南蛮入侵
                case Card.Effect.NanManRuQin:
                    r = NanManRuQinProc(r);
                    break;
                //万箭齐发
                case Card.Effect.WanJianQiFa:
                    r = WanJianQiFaProc(r);
                    break;
                //桃园结义
                case Card.Effect.TaoYuanJieYi:
                    r = TaoYuanJieYi(r);
                    break;
                //无中生有
                case Card.Effect.WuZhongShengYou:
                    r = WuZhongShengYouProc(r);
                    break;
                //过河拆桥
                case Card.Effect.GuoHeChaiQiao:
                    r = GuoHeChaiQiaoProc(r);
                    break;
                //顺手牵羊
                case Card.Effect.ShunShouQianYang:
                    r = ShunShouQianYangProc(r);
                    break;
                //借刀杀人
                case Card.Effect.JieDaoShaRen:
                    r = JieDaoShaRenProc(r);
                    break;
                //乐不思蜀
                case Card.Effect.LeBuSiShu:
                    r = LeBuSiShuProc(r);
                    break;
                //闪电
                case Card.Effect.ShanDian:
                    r = ShanDianProc(r);
                    break;
                //五谷丰登
                case Card.Effect.WuGuFengDeng:
                    r = WuGuFengDengProc(r);
                    break;
                    //防具的过程统一用一个方法
                case Card.Effect.BaGuaZhen:
                case Card.Effect.TengJia:
                case Card.Effect.RenWangDun:
                case Card.Effect.BaiYinShiZi:
                    r = ArmorProc(r);
                    break;
                    //武器的过程统一用一个方法
                case Card.Effect.ZhangBaSheMao:
                case Card.Effect.ZhuGeLianNu:
                case Card.Effect.FangTianHuaJi:
                case Card.Effect.HanBingJian:
                case Card.Effect.GuDianDao:
                case Card.Effect.QiLinGong:
                case Card.Effect.GuanShiFu:
                case Card.Effect.QingLongYanYueDao:
                case Card.Effect.QingGangJian:
                case Card.Effect.CiXiongShuangGuJian:
                case Card.Effect.ZhuQueYuShan:
                    r = WeaponProc(r);
                    break;
                    //各种马的过程用一个方法
                case Card.Effect.Jian1:
                case Card.Effect.Jia1:
                    r = HorseProc(r);
                    break;
            }
        }

        /// <summary>
        /// 清除事件链的过程
        /// </summary>
        /// <remarks>该方法是将公共牌槽,武将可回收牌槽等剩余的牌放入垃圾桶中,并将发送事件结束的XML消息
        /// 清除事件链发生在每个游戏阶段的起始事件完成后</remarks>
        private void ClearEventProc()
        {
            //尝试回收游戏对象中牌槽容器的牌
            foreach(Slot s in CardsBuffer.Slots)
                if (s.Recyclable && s.Cards.Count != 0)
                {
                    DropCards(true, CardFrom.Slot, string.Empty, s.Cards.ToArray(), Card.Effect.None, null, null, null);
                    s.Cards.Clear();
                }
            //尝试回收玩家牌槽容器中的牌
            foreach (Player p in GamePlayers.All)
            {
                if (p.Chief != null)
                    foreach (Slot s in p.Chief.SlotsBuffer.Slots)
                        if (s.Recyclable && s.Cards.Count != 0)
                        {
                            DropCards(true, CardFrom.Slot, string.Empty, s.Cards.ToArray(), Card.Effect.None, null, null, null);
                            s.Cards.Clear();
                        }
            }
            //发送事件结束消息
            AsynchronousCore.SendClearMessage();
        }
        #endregion


        /// <summary>
        /// 问询出牌合法性检验,并回应武将所控制的玩家出牌合法性结果,对于判断为非法的出牌,将会设置用户返回结果
        /// </summary>
        /// <param name="aChief">出牌武将</param>
        /// <param name="aResult">问询结果,若出牌不合法,则问询会被修改为默认对象</param>
        /// <remarks>此方法用于通过回调通知调试客户端操作的合法性,回调仅适合用于测试类库逻辑</remarks>
        private void ValidityResult(ChiefBase aChief, ref MessageCore.AskForResult aResult)
        {
            if (!CheckValid(aResult, aChief))
            {
                AsynchronousCore.LeadingInvalid(aChief);
                aResult = new MessageCore.AskForResult(false, aResult.Leader, new ChiefBase[] { }, new Card[] { }, Card.Effect.None, false, true, string.Empty);
            }
            else
                AsynchronousCore.LeadingValid(aChief);
        }

        #region 玩家放弃对问询的回答,系统做出的默认处理
        /// <summary>
        /// 让玩家自动选择要弃的牌
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <returns>返回牌数组</returns>
        private Card[] AutoAbandonment(ChiefBase aChief)
        {
            if (GamePlayers[aChief].Hands.Count <= GamePlayers[aChief].Health) return new Card[0];
            return GamePlayers[aChief].Hands.GetRange(0, GamePlayers[aChief].Hands.Count - GamePlayers[aChief].Health).ToArray();
        }


        /// <summary>
        /// 自动从玩家牌中选择一张牌
        /// 若该玩家有手牌,则在手牌中随机选一张,否则选择武器,防具,+1马,-1马,最后放置的debuff
        /// </summary>
        /// <param name="aChief">需要选择的武将对象</param>
        /// <returns>一张系统选择的牌</returns>
        internal Card AutoSelect(ChiefBase aChief)
        {
            if (GamePlayers[aChief].Hands.Count > 0)
            {
                return GamePlayers[aChief].Hands[GetRandom(GamePlayers[aChief].Hands.Count)];
            }
            else if (GamePlayers[aChief].Weapon != null)
            {
                return GamePlayers[aChief].Weapon;
            }
            else if (GamePlayers[aChief].Armor != null)
            {
                return GamePlayers[aChief].Armor;
            }
            else if (GamePlayers[aChief].Jia1Ma != null)
            {
                return GamePlayers[aChief].Jia1Ma;
            }
            else if (GamePlayers[aChief].Jian1Ma != null)
            {
                return GamePlayers[aChief].Jian1Ma;
            }
            else if (GamePlayers[aChief].Debuff.Count > 0)
            {
                return GamePlayers[aChief].Debuff.Peek();
            }
            else
                return null;
        }
        #endregion

        #region 判定和判定区

        /// <summary>
        /// 从牌堆取出一张牌作为判定牌
        /// </summary>
        /// <param name="aChief">判定的武将对象</param>
        /// <param name="aEffect">要判定的效果</param>
        /// <returns>一张判定牌</returns>
        internal Card popJudgementCard(ChiefBase aChief,Card.Effect aEffect)
        {
            Card c = CardsHeap.Pop(); //取一张牌
            AsynchronousCore.SendMessage(
                new Beaver("judgmentcard", aChief.ChiefName, aEffect.ToString(), Card.Cards2Beaver("cards", new Card[] { c })).ToString());
                //new XElement("judgmentcard",
                //    new XElement("target", aChief.ChiefName),
                //    new XElement("effect", aEffect),
                //    Card.Cards2XML("cards", new Card[] { c })
                //)); //发送揭出判定牌的消息
            ChiefBase t = aChief; //现在从判定牌持有的玩家开始,轮询武将OnChiefJudgementCardShow_Turn方法
            do
            {
                foreach (ASkill s in t.Skills)
                    c = s.OnChiefJudgementCardShow_Turn(aChief, c , t , gData);
                t = t.Next;
            }
            while (t != aChief);
            c.CardEffect = aEffect;
 //最后这里设定判定牌的性质是考虑之前判定牌不确定的因素
            return c;
        }

        internal void RecoveryJudgementCard(Card aCard, ChiefBase aChief, Card.Effect aEffect)
        {
            bool EnableSengToBin = true;
            foreach (ASkill s in aChief.Skills)
            {
                s.OnChiefJudgementCardTakeEffect(aChief, aCard, ref EnableSengToBin, gData);
            }
            if (EnableSengToBin)
                lstCardBin.Add(aCard);
        }
        #endregion

        #region 小的方法
        /// <summary>
        /// 一般出牌情况下,检查出牌是否合法,即牌来自目标的手牌
        /// </summary>
        /// <param name="aResult">问询的回复</param>
        /// <param name="aChief">出牌的武将</param>
        /// <returns>若出牌都在手牌中,返回true</returns>
        private bool CheckValid(MessageCore.AskForResult aResult , ChiefBase aChief)
        {
            if (!aResult.PlayerLead || aResult.SkillName != string.Empty) return true;
            if (aResult.Cards.Count() == 0 && aResult.Effect != Card.Effect.None) return false;
            return GamePlayers[aChief].HasCardsInHand(aResult.Cards);
        }

        //这方法位置有问题
        private XElement Chiefs2XmlName(int aIndex)
        {
            XElement r = new XElement("chiefs");
            foreach (ChiefBase c in GamePlayers[aIndex].AvailableChiefs)
                r.Add(new XElement("chief", c.ChiefName));
            return r;
        }

        private Beaver Chiefs2Beaver(int aIndex)
        {
            Beaver ret = new Beaver();
            foreach (ChiefBase c in GamePlayers[aIndex].AvailableChiefs)
                ret.Add(string.Empty, c.ChiefName);
            ret.SetHeaderElementName("chiefs");
            return ret;
            //Beaver r = null;
            //foreach (ChiefBase c in GamePlayers[aIndex].AvailableChiefs)
            //    if (r == null)
            //        r = new Beaver("chiefs", c.ChiefName);
            //    else
            //        r.Add(string.Empty, c.ChiefName);
            //if (r == null)
            //    r = new Beaver("chiefs");
            //return r;
        }

        internal bool HasCardsInBin(Card[] aCards)
        {
            foreach (Card c in lstCardBin)
            {
                if (!lstCardBin.Contains(c)) return false;
            }
            return true;
        }

        internal int CalcMaxShaTargets(ChiefBase aChief , Card[] aCards)
        {
            int count = 1;
            if (GamePlayers[aChief].Weapon != null)
                Weapon.CalcShaTargetsCount(GamePlayers[aChief].Weapon.CardEffect, aCards, aChief, Card.Effect.Sha, gData, ref count);
            return count;
        }

        /// <summary>
        /// 一个用于武将之间移动牌的方法,默认将牌送至收到牌的武将的手牌中
        /// </summary>
        /// <param name="aChiefFrom">给出牌的武将</param>
        /// <param name="aChiefTo">收到牌的武将</param>
        /// <param name="aCards">牌数组</param>
        internal bool Move(ChiefBase aChiefFrom, ChiefBase aChiefTo, Card[] aCards)
        {
            if (GamePlayers[aChiefFrom].Dead || GamePlayers[aChiefTo].Dead) return false;
            if (aChiefTo == aChiefFrom) return false;
            if (!DropCards(false, CardFrom.HandAndEquipage, string.Empty, aCards, Card.Effect.None, aChiefFrom, null, null)) return false;
            foreach (Card c in aCards)
                GamePlayers[aChiefTo].Hands.Add(c.GetOriginalCard());
            return true;
        }

        private bool WithinKitRange(ChiefBase aChiefForm, ChiefBase aChiefTo)
        {
            byte dis = GamePlayers.Distance(aChiefTo, aChiefForm);
            return CalcKitDistance(aChiefForm) >= dis;
        }

        internal bool WithinShaRange(ChiefBase aChiefForm, ChiefBase aChiefTo)
        {
            byte dis = GamePlayers.Distance(aChiefTo, aChiefForm);
            return CalcShaDistance(aChiefForm) >= dis;
        }

        private byte CalcKitDistance(ChiefBase aChief)
        {
            byte ret = 1;
            if (GamePlayers[aChief].Jian1Ma != null)
                ret++;
            foreach (ASkill s in aChief.Skills)
                ret = s.CalcKitDistance(aChief, ret, gData);
            return ret;
        }

        private byte CalcShaDistance(ChiefBase aChief)
        {
            byte ret = 1;
            if (GamePlayers[aChief].Weapon != null)
                ret = Convert.ToByte(Weapon.WeaponRange(GamePlayers[aChief].Weapon.CardEffect));
            if (GamePlayers[aChief].Jian1Ma != null)
                ret++;
            foreach (ASkill s in aChief.Skills)
                ret = s.CalcShaDistance(aChief, ret, gData);
            return ret;
        }

        /// <summary>
        /// 要求武将消耗体力值
        /// </summary>
        /// <param name="aChief">受伤害的武将</param>
        /// <param name="aDamage">伤害量</param>
        /// <param name="aSource">伤害来源,非玩家操作置null</param>
        /// <param name="aSourceEvent">伤害来源事件</param>
        internal void DamageHealth(ChiefBase aChief, sbyte aDamage , ChiefBase aSource, EventRecoard aSourceEvent)
        {
            if (GamePlayers[aChief].Health - aDamage < 1)
            {
                AsynchronousCore.SendMessage(
                    new Beaver("health" , aChief.ChiefName , GamePlayers[aChief].MaxHealth.ToString() , "0" ).ToString());
                    //new XElement("health",
                    //new XElement("target", aChief.ChiefName),
                    //new XElement("max", GamePlayers[aChief].MaxHealth),
                    //new XElement("current", 0)
                    //));

                if (Cry4HelpProc(aSource, aChief, (sbyte)Math.Abs(GamePlayers[aChief].Health - aDamage - 1)))
                {
                    //GamePlayers[aChief].Health = 0;
                    GamePlayers[aChief].Dead = true;
                    //drop all cards
                    List<Card> lstDrop = new List<Card>();
                    lstDrop.AddRange(GamePlayers[aChief].Hands);
                    lstDrop.AddRange(GamePlayers[aChief].Debuff);
                    if (GamePlayers[aChief].Weapon != null)
                        lstDrop.Add(GamePlayers[aChief].Weapon);
                    if (GamePlayers[aChief].Armor != null)
                        lstDrop.Add(GamePlayers[aChief].Armor);
                    if (GamePlayers[aChief].Jia1Ma != null)
                        lstDrop.Add(GamePlayers[aChief].Jia1Ma);
                    if (GamePlayers[aChief].Jian1Ma != null)
                        lstDrop.Add(GamePlayers[aChief].Jian1Ma);
                    AsynchronousCore.SendMessage(
                        new Beaver("fall" , aChief.ChiefName , aChief.ChiefStatus).ToString());
                        //new XElement("fall",
                        //    new XElement("target", aChief.ChiefName),
                        //    new XElement("status", aChief.ChiefStatus)
                        //    )
                        //);
                    AsynchronousCore.SendMessage(MessageCore.MakeDropMessage(aChief, aChief, lstDrop.ToArray()));
                    DropCards(true, CardFrom.HandAndEquipageAndJudgement, string.Empty, lstDrop.ToArray(), Card.Effect.None, aChief, null, null);
                    RefereeProc();
                    if (aChief.ChiefStatus == ChiefBase.Status.Insurgent && aSource != null && !GamePlayers[aSource].Dead)
                    {
                        TakeingCards(aSource, 3);
                    }
                    return;
                }
            }
            else
            {
                AsynchronousCore.SendMessage(new Beaver("health" , aChief.ChiefName , GamePlayers[aChief].MaxHealth.ToString() , (GamePlayers[aChief].Health - aDamage).ToString()).ToString());
                    //new XElement("health",
                    //new XElement("target", aChief.ChiefName),
                    //new XElement("max", GamePlayers[aChief].MaxHealth),
                    //new XElement("current", GamePlayers[aChief].Health - aDamage)
                    //));
                GamePlayers[aChief].Health -= aDamage;
            }
            foreach (ASkill s in GamePlayers[aChief].Chief.Skills)
            {
                s.OnChiefHarmed(aSourceEvent, aSource, aChief, gData, aDamage);
            }
        }

        internal void RegainHealth(ChiefBase aChief, sbyte aRegain)
        {
            if (GamePlayers[aChief].MaxHealth != GamePlayers[aChief].Health)
            {
                GamePlayers[aChief].Health += aRegain;
                AsynchronousCore.SendMessage( new Beaver("health" ,aChief.ChiefName ,GamePlayers[aChief].MaxHealth.ToString() , GamePlayers[aChief].Health.ToString()).ToString());
                    //new XElement("health",
                    //new XElement("target", aChief.ChiefName),
                    //new XElement("max", GamePlayers[aChief].MaxHealth),
                    //new XElement("current", GamePlayers[aChief].Health)
                    //));
            }
        }
        

        /// <summary>
        /// 清除打牌堆
        /// </summary>
        private void FreeCardBin()
        {
            CardsHeap.AddCards(lstCardBin.ToArray());
            lstCardBin.Clear();
        }

        /// <summary>
        /// 移除武将的手牌,如果不能全部移除将不会改变玩家的手牌
        /// 注意,方法成功之后移除的手牌并不会放入弃牌堆
        /// </summary>
        /// <param name="aChief">武将</param>
        /// <param name="aCards">需要移除的手牌</param>
        /// <returns>移除正常返回true</returns>
        internal bool RemoveHand(ChiefBase aChief, Card[] aCards)
        {
            foreach (Card c in aCards)
            {
                if (!GamePlayers[aChief].Hands.Contains(c)) return false;
            }
            List<Card> old = GamePlayers[aChief].Hands.ToList();
            foreach (Card c in aCards)
            {
                if (!GamePlayers[aChief].RemoveHand(c))
                {
                    GamePlayers[aChief].Hands = old;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 移除武将的牌,即包含手牌,装备区和判定区,如果不能全部移除将不会改变玩家的牌,但是所执行的事件不能挽回
        /// 注意,方法成功之后移除的牌并不会放入弃牌堆
        /// </summary>
        /// <param name="aChief">武将</param>
        /// <param name="aCards">需要移除的牌</param>
        /// <returns>移除正常返回true</returns>
        private bool RemoveCard(ChiefBase aChief, Card[] aCards)
        {
            Card weapon = GamePlayers[aChief].Weapon;
            Card armor = GamePlayers[aChief].Armor;
            Card Jia1 = GamePlayers[aChief].Jia1Ma;
            Card Jian1 = GamePlayers[aChief].Jian1Ma;
            List<Card> oldHand = GamePlayers[aChief].Hands.ToList();
            Stack<Card> oldDebuff = new Stack<Card>(GamePlayers[aChief].Debuff);
            foreach (Card c in aCards)
            {
                if (!GamePlayers[aChief].RemoveHand(c) && !GamePlayers[aChief].RemoveBuff(c))
                {
                    if (GamePlayers[aChief].Weapon != null && GamePlayers[aChief].Weapon.IsSame(c))
                    {
                        GamePlayers[aChief].UnloadWeapon(this, gData);
                        foreach (ASkill s in aChief.Skills)
                            s.DropEquipage(aChief, gData);
                    }
                    else if (GamePlayers[aChief].Armor != null && GamePlayers[aChief].Armor.IsSame(c))
                    {
                        GamePlayers[aChief].UnloadArmor(this);
                        foreach (ASkill s in aChief.Skills)
                            s.DropEquipage(aChief, gData);
                    }
                    else if (GamePlayers[aChief].Jia1Ma != null && GamePlayers[aChief].Jia1Ma.IsSame(c))
                    {
                        GamePlayers[aChief].UnloadJia1(this);
                        foreach (ASkill s in aChief.Skills)
                            s.DropEquipage(aChief, gData);
                    }
                    else if (GamePlayers[aChief].Jian1Ma != null && GamePlayers[aChief].Jian1Ma.IsSame(c))
                    {
                        GamePlayers[aChief].UnloadJia1(this);
                        foreach (ASkill s in aChief.Skills)
                            s.DropEquipage(aChief, gData);
                    }
                    else
  //这里找不到牌在玩家的任何区域中，执行还原动作，并且返回false
                    {
                        GamePlayers[aChief].Weapon = weapon;
                        GamePlayers[aChief].Armor = armor;
                        GamePlayers[aChief].Jia1Ma = Jia1;
                        GamePlayers[aChief].Jian1Ma = Jian1;
                        GamePlayers[aChief].Hands = oldHand;
                        GamePlayers[aChief].Debuff = oldDebuff;
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 让玩家从牌堆拿n张牌,该方法会自动将牌放置到武将的手牌中,并且发送takecards消息(n=0方法无作为)
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="n">拿牌的数量</param>
        /// <returns>牌的数组</returns>
        internal Card[] TakeingCards(ChiefBase aChief, int n)
        {
            if (n <= 0) return new Card[] { };
            if (GamePlayers[aChief].Dead) return new Card[] { };
            Card[] ret = CardsHeap.Pop(n);
            GamePlayers[aChief].Hands.AddRange(ret);
            AsynchronousCore.SendPrivateMessageWithOpenMessage(aChief, new Beaver("takecards", aChief.ChiefName, Card.Cards2Beaver("cards", ret)).ToString(), new Beaver("takecards", aChief.ChiefName, n.ToString(), GamePlayers[aChief].Hands.Count.ToString()).ToString(), GamePlayers);
            //AsynchronousCore.SendMessage( GamePlayers.All.Where(c=>c.Chief != aChief ).ToArray(),
            //    new Beaver("takecards", aChief.ChiefName , n.ToString() , GamePlayers[aChief].Hands.Count.ToString()).ToString(),true);
            //    //new XElement("takecards",
            //    //    new XElement("target" , aChief.ChiefName),
            //    //    new XElement("takecount" , n.ToString()),
            //    //    new XElement("count" , GamePlayers[aChief].Hands.Count.ToString())
            //    //) , true);
            //AsynchronousCore.SendMessage(new ChiefBase[] { aChief },
            //    new Beaver("takecards", aChief.ChiefName, Card.Cards2Beaver("cards", ret)).ToString(), false);
            //    //new XElement("takecards",
            //    //    new XElement("target", aChief.ChiefName),
            //    //    Card.Cards2XML("cards", ret)
            //    //),false);
            return ret;
        }

        /// <summary>
        /// 此操作将从打牌的垃圾桶lstCardBin中捡出来一些牌,如果参数中有任何牌不能在垃圾桶中找到,方法将不会改变垃圾桶内容
        /// </summary>
        /// <param name="aCards">需要拾取出来的牌</param>
        /// <returns>如果所有的牌都能从垃圾桶中找到,那么返回true</returns>
        internal bool PickRubbish(Card[] aCards)
        {
            List<Card> copy = lstCardBin.ToList();
            foreach (Card c in aCards)
                if (!lstCardBin.Remove(c))
                {
                    lstCardBin = copy;
                    return false;
                }
            return true;
        }
        #endregion

        #region 类的数据区
        
        /// <summary>
        /// 存储每个子事件的头
        /// </summary>
        private Queue<EventRecoard> queRecoard = new Queue<EventRecoard>();

        /// <summary>
        /// 子事件的处理链表
        /// </summary>
        internal List<EventRecoard> lstRecoard = new List<EventRecoard>();

        /// <summary>
        /// 事件中需要回收的牌
        /// </summary>
        private List<Card> lstCardBin = new List<Card>();

        #endregion
    }
}
