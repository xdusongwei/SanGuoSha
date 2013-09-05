/*
 * Card 和 CardHeap
 * Namespace SGS.ServerCore.Contest.Data
 * 定义牌和牌堆
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Data.GameException;
using BeaverMarkupLanguage;


namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 牌类
    /// </summary>
    public class Card
    {
        /// <summary>
        /// 创建一张牌
        /// </summary>
        /// <param name="aID">牌的编号</param>
        /// <param name="aHuaSe">花色</param>
        /// <param name="aNumber">点数</param>
        /// <param name="aEffect">基本效果</param>
        /// <param name="aElement">牌的属性效果</param>
        public Card(int aID , Suit aHuaSe , int aNumber , Effect aEffect , ElementType aElement)
        {
            ID = aID;
            CardHuaSe = aHuaSe;
            CardNumber = aNumber;
            OriginEffect = aEffect;
            OriginHuaSe = aHuaSe;
            CardEffect = aEffect;
            OriginElement = aElement;
            Element = aElement;
        }

        /// <summary>
        /// 创建一张没有属性的牌
        /// </summary>
        /// <param name="aID">牌的编号</param>
        /// <param name="aHuaSe">花色</param>
        /// <param name="aNumber">点数</param>
        /// <param name="aEffect">基本效果</param>
        public Card(int aID, Suit aHuaSe, int aNumber, Effect aEffect)
            :this(aID  ,aHuaSe ,aNumber , aEffect , ElementType.None)
        {

        }

        /// <summary>
        /// 根据ID判断牌是否相同
        /// </summary>
        /// <param name="c">另一张牌的引用</param>
        /// <returns>若号码相同,返回true,其他情况包括null参数返回false</returns>
        public bool Same(Card c)
        {
            if (c == null) return false;
            return c.ID == ID ? true : false;
        }

        /// <summary>
        /// 花色枚举
        /// </summary>
        public enum Suit { 
            /// <summary>
            /// 红桃
            /// </summary>
            HongTao,
            /// <summary>
            /// 草花
            /// </summary>
            CaoHua, 
            /// <summary>
            /// 黑桃
            /// </summary>
            HeiTao, 
            /// <summary>
            /// 方片
            /// </summary>
            FangPian };

        /// <summary>
        /// 牌的效果
        /// </summary>
        public enum Effect
        {
            /// <summary>
            /// 没有效果
            /// </summary>
            None, 
            /// <summary>
            /// 杀
            /// </summary>
            Sha, 
            /// <summary>
            /// 闪
            /// </summary>
            Shan, 
            /// <summary>
            /// 桃
            /// </summary>
            Tao, 
            /// <summary>
            /// 无懈可击
            /// </summary>
            WuXieKeJi, 
            /// <summary>
            /// 决斗
            /// </summary>
            JueDou, 
            /// <summary>
            /// 南蛮入侵
            /// </summary>
            NanManRuQin, 
            /// <summary>
            /// 万箭齐发
            /// </summary>
            WanJianQiFa, 
            /// <summary>
            /// 桃园结义
            /// </summary>
            TaoYuanJieYi, 
            /// <summary>
            /// 无中生有
            /// </summary>
            WuZhongShengYou, 
            /// <summary>
            /// 过河拆桥
            /// </summary>
            GuoHeChaiQiao, 
            /// <summary>
            /// 顺手牵羊
            /// </summary>
            ShunShouQianYang, 
            /// <summary>
            /// 借刀杀人
            /// </summary>
            JieDaoShaRen, 
            /// <summary>
            /// 五谷丰登
            /// </summary>
            WuGuFengDeng,
            /// <summary>
            /// 闪电
            /// </summary>
            ShanDian, 
            /// <summary>
            /// 乐不思蜀
            /// </summary>
            LeBuSiShu,
            /// <summary>
            /// 八卦阵
            /// </summary>
            BaGuaZhen, 
            /// <summary>
            /// 仁王盾
            /// </summary>
            RenWangDun, 
            /// <summary>
            /// 藤甲
            /// </summary>
            TengJia, 
            /// <summary>
            /// 白银狮子
            /// </summary>
            BaiYinShiZi,
            /// <summary>
            /// 诸葛连弩
            /// </summary>
            ZhuGeLianNu, 
            /// <summary>
            /// 贯石斧
            /// </summary>
            GuanShiFu, 
            /// <summary>
            /// 麒麟弓
            /// </summary>
            QiLinGong, 
            /// <summary>
            /// 雌雄双股剑
            /// </summary>
            CiXiongShuangGuJian, 
            /// <summary>
            /// 古锭刀
            /// </summary>
            GuDianDao, 
            /// <summary>
            /// 青龙偃月刀
            /// </summary>
            QingLongYanYueDao, 
            /// <summary>
            /// 方天画戟
            /// </summary>
            FangTianHuaJi, 
            /// <summary>
            /// 朱雀羽扇
            /// </summary>
            ZhuQueYuShan, 
            /// <summary>
            /// 青钢剑
            /// </summary>
            QingGangJian, 
            /// <summary>
            /// 寒冰箭
            /// </summary>
            HanBingJian, 
            /// <summary>
            /// 丈八蛇矛
            /// </summary>
            ZhangBaSheMao,
            /// <summary>
            /// +1马
            /// </summary>
            Jia1 , 
            /// <summary>
            /// -1马
            /// </summary>
            Jian1,
            /// <summary>
            /// 酒
            /// </summary>
            Jiu , 
            /// <summary>
            /// 火攻
            /// </summary>
            HuoGong , 
            /// <summary>
            /// 铁索连环
            /// </summary>
            TieSuoLianHuan,
            /// <summary>
            /// 兵粮寸断
            /// </summary>
            BingLiangCunDuan,
            /// <summary>
            /// 回应出牌问询,技能已处理,不需要通过出牌的问询处理
            /// </summary>
            Skill,
            /// <summary>
            /// 刚烈
            /// </summary>
            GangLie , 
            /// <summary>
            /// 洛神
            /// </summary>
            LuoShen,
            /// <summary>
            /// 铁骑
            /// </summary>
            TieQi
        };

        /// <summary>
        /// 这张牌的编号
        /// </summary>
        public readonly int ID;

        /// <summary>
        /// 这张牌的花色
        /// </summary>
        public Suit CardHuaSe;

        /// <summary>
        /// 这张牌的点数，只读
        /// </summary>
        public readonly int CardNumber;
        /// <summary>
        /// 原效果，只读
        /// </summary>
        public readonly Effect OriginEffect;
        /// <summary>
        /// 原花色，只读
        /// </summary>
        public readonly Suit OriginHuaSe;
        /// <summary>
        /// 牌的效果
        /// </summary>
        public Effect CardEffect;


        /// <summary>
        /// 牌的属性
        /// </summary>
        public enum ElementType { 
            /// <summary>
            /// 没有属性
            /// </summary>
            None, 
            /// <summary>
            /// 雷属性
            /// </summary>
            Thunder, 
            /// <summary>
            /// 火属性
            /// </summary>
            Fire };

        /// <summary>
        /// 牌的属性
        /// </summary>
        public ElementType Element
        {
            get;
            set;
        }

        /// <summary>
        /// 牌原有的属性
        /// </summary>
        public readonly ElementType OriginElement;


        /// <summary>
        /// 获得这张牌的原效果副本（清除上面的多余特殊效果）
        /// </summary>
        /// <returns></returns>
        public Card GetOriginalCard()
        {
            return new Card(ID, OriginHuaSe, CardNumber, OriginEffect , OriginElement);
        }

        /// <summary>
        /// 将牌数组转化为牌号码的XML节
        /// </summary>
        /// <param name="aNodeName">节名称</param>
        /// <param name="aCards">牌数组</param>
        /// <returns>一个XML形式的牌号码对象</returns>
        internal static XElement Cards2XML(string aNodeName, Card[] aCards)
        {
            XElement xcards = new XElement(aNodeName);
            foreach (Card c in aCards)
                xcards.Add(new XElement("card", c.ID));
            return xcards;
        }

        internal static BeaverMarkupLanguage.Beaver Cards2Beaver(string aNodeName, Card[] aCards)
        {
            Beaver ret = new Beaver();
            foreach (Card c in aCards)
                ret.Add(string.Empty, c.ID);
            ret.SetHeaderElementName(aNodeName);
            return ret;
            //return new BeaverMarkupLanguage.Beaver(aNodeName, aCards.Select(i => i.CardNumber).ToArray());
        }
    }

    /// <summary>
    /// 牌堆类
    /// 牌堆类包含拿牌堆和弃牌堆
    /// </summary>
    public class CardHeap : IDisposable
    {
        //private RNGCryptoServiceProvider _random = null;

        /// <summary>
        /// 当前游戏中牌的总数
        /// </summary>
        public int TotalCards
        {
            get;
            set;
        }

        /// <summary>
        /// 未知牌
        /// </summary>
        public static readonly Card Unknown = new Card(0, Card.Suit.HeiTao, 1, Card.Effect.None);

        /// <summary>
        /// 黑桃
        /// </summary>
        public static readonly Card HeiTao = new Card(-1, Card.Suit.HeiTao, 1, Card.Effect.None);
        /// <summary>
        /// 草花
        /// </summary>
        public static readonly Card CaoHua = new Card(-2, Card.Suit.CaoHua, 1, Card.Effect.None);
        /// <summary>
        /// 红桃
        /// </summary>
        public static readonly Card HongTao = new Card(-3, Card.Suit.HongTao, 1, Card.Effect.None);
        /// <summary>
        /// 方片
        /// </summary>
        public static readonly Card FangPian = new Card(-4, Card.Suit.FangPian, 1, Card.Effect.None);

        /// <summary>
        /// 构造牌堆
        /// </summary>
        public CardHeap(SGS.ServerCore.Contest.Global.GameBase aGame)
        {
            //_random = new RNGCryptoServiceProvider();
            _game = aGame;
        }

        private SGS.ServerCore.Contest.Global.GameBase _game = null;

        /// <summary>
        /// 释放对象的非托管资源
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
                }
                disposed = true;
            }
        }

        /// <summary>
        /// 销毁牌堆
        /// </summary>
        ~CardHeap()
        {
            Dispose(false);
        }

        private int GetRandom(int mod)
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
                    mask = (mask << 8) | arr[i - 1];
                    mask = (mask << 8) | arr[i - 2];
                    mask = (mask << 8) | arr[i - 3];
                    ret = ret ^ mask;
                }
            }
            ret = Math.Abs(ret);
            return ret % mod;
        }
        /// <summary>
        /// 将弃牌堆的牌洗好放入拿牌堆
        /// </summary>
        public void FillCard()
        {
            List<Card> lst = new List<Card>();
            foreach (Card c in CardsHeap2)
            {
                lst.Add(c.GetOriginalCard());
            }

            Card[] arr = lst.ToArray();
            for (int i = 0; i < lst.Count; i++)
            {
                int a = GetRandom(lst.Count);
                int b = GetRandom(lst.Count);
                
                Card t = arr[a];
                arr[a] = arr[b];
                arr[b] = t;
            }
            lst = arr.ToList();
            foreach (Card c in lst)
            {
                CardsHeap.Enqueue(c);
            }
            CardsHeap2.Clear();
        }

        /// <summary>
        /// 将一张牌加入弃牌堆
        /// </summary>
        /// <param name="aCard">牌对象</param>
        public void AddCard(Card aCard)
        {
            CardsHeap2.Enqueue(aCard.GetOriginalCard());
        }

        /// <summary>
        /// 加入一批牌到弃牌堆
        /// </summary>
        /// <param name="aCards">被弃牌的数组</param>
        public void AddCards(Card[] aCards)
        {
            foreach (Card c in aCards)
                CardsHeap2.Enqueue(c);
        }

        /// <summary>
        /// 从牌堆中哪一张牌
        /// </summary>
        /// <returns>若无法给出一张牌,则返回null.否则返回牌对象</returns>
        public Card Pop()
        {
            if (CardsHeap.Count == 0)
            {
                FillCard();
            }
            if(CardsHeap.Count> 0 )
                return CardsHeap.Dequeue();
            else
                throw new NoMoreCard(_game.GamePlayers);
        }

        /// <summary>
        /// 把指定号码的牌从弃牌堆或者发牌堆中去除
        /// </summary>
        /// <param name="aID">牌的号码</param>
        /// <returns>成功返回true</returns>
        public bool PopCard(int aID)
        {
            if (CardsHeap.Where(c => c.ID == aID).Count() > 0)
            {
                CardsHeap = new Queue<Card>(CardsHeap.Where(c=>c.ID != aID));
                return true;
            }
            else if (CardsHeap2.Where(c => c.ID == aID).Count() > 0)
            {
                CardsHeap2 = new Queue<Card>(CardsHeap2.Where(c => c.ID != aID));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 将牌放置按照给定的顺序在新牌堆的顶端
        /// </summary>
        /// <param name="aCards">牌数组</param>
        /// <returns></returns>
        public bool PutOnTop(Card[] aCards)
        {
            if (aCards.Count() == 0) return true;
            List<Card> lst = new List<Card>(aCards);
            foreach (Card c in CardsHeap)
                lst.Add(c);
            CardsHeap = new Queue<Card>(lst);
            return true;
        }


        /// <summary>
        /// 将[牌按照指定的顺序放置在新牌堆的底部
        /// </summary>
        /// <param name="aCards">牌数组</param>
        /// <returns></returns>
        public bool PutOnBottom(Card[] aCards)
        {
            if (aCards.Count() == 0) return true;
            foreach (Card c in aCards)
                CardsHeap.Enqueue(c);
            return true;
        }

        /// <summary>
        /// 从牌堆中拿n张牌
        /// </summary>
        /// <param name="n">拿出的数量</param>
        /// <returns>牌的数组</returns>
        public Card[] Pop(int n)
        {
            List<Card> lst = new List<Card>();
            if (n < 1) return lst.ToArray();
            if (n > CardsHeap.Count + CardsHeap2.Count) throw new NoMoreCard(_game.GamePlayers);
            while (n > 0)
            {
                if (CardsHeap.Count == 0)
                    FillCard();
                lst.Add( CardsHeap.Dequeue());
                n--;
            }
            return lst.ToArray();
        }

        /// <summary>
        /// 判断牌是否在弃牌堆或者发牌堆
        /// </summary>
        /// <param name="aCard">牌对象</param>
        /// <returns>若存在则返回true</returns>
        public bool Exist(Card aCard)
        {
            if (CardsHeap.Contains(aCard) || CardsHeap2.Contains(aCard)) return true;
            return false;
        }

        /// <summary>
        /// 向弃牌堆加入原版的卡牌
        /// </summary>
        public void FillOriginCards()
        {
            AddCard(new Card(1, Card.Suit.CaoHua, 12, Card.Effect.ZhangBaSheMao));
            AddCard(new Card(2, Card.Suit.FangPian, 5, Card.Effect.GuanShiFu));
            AddCard(new Card(3, Card.Suit.HeiTao, 5, Card.Effect.QingLongYanYueDao));
            AddCard(new Card(4, Card.Suit.HeiTao, 2, Card.Effect.CiXiongShuangGuJian));
            AddCard(new Card(5, Card.Suit.HeiTao, 6, Card.Effect.QingGangJian));
            AddCard(new Card(6, Card.Suit.FangPian, 12, Card.Effect.FangTianHuaJi));
            AddCard(new Card(7, Card.Suit.FangPian, 1, Card.Effect.ZhuGeLianNu));
            AddCard(new Card(8, Card.Suit.HongTao, 5, Card.Effect.QiLinGong));
            AddCard(new Card(9, Card.Suit.CaoHua, 2, Card.Effect.BaGuaZhen));
            AddCard(new Card(10, Card.Suit.HeiTao, 12, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(11, Card.Suit.CaoHua, 1, Card.Effect.JueDou));
            AddCard(new Card(12 , Card.Suit.CaoHua , 6 , Card.Effect.Sha));
            AddCard(new Card(13 , Card.Suit.HongTao , 3 , Card.Effect.Tao));
            AddCard(new Card(14, Card.Suit.HongTao, 4, Card.Effect.WuGuFengDeng));
            AddCard(new Card(15, Card.Suit.FangPian, 4, Card.Effect.ShunShouQianYang));
            AddCard(new Card(16, Card.Suit.CaoHua, 4, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(17, Card.Suit.HeiTao, 4, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(18, Card.Suit.FangPian, 2, Card.Effect.Shan));
            AddCard(new Card(19, Card.Suit.HongTao, 8, Card.Effect.Tao));
            AddCard(new Card(20, Card.Suit.HeiTao, 3, Card.Effect.ShunShouQianYang));
            AddCard(new Card(21, Card.Suit.FangPian, 8, Card.Effect.Sha));
            AddCard(new Card(22, Card.Suit.HeiTao, 1, Card.Effect.JueDou));
            AddCard(new Card(23, Card.Suit.CaoHua, 13, Card.Effect.JieDaoShaRen));
            AddCard(new Card(24, Card.Suit.CaoHua, 7, Card.Effect.Sha));
            AddCard(new Card(25, Card.Suit.CaoHua, 13, Card.Effect.WuXieKeJi));
            AddCard(new Card(26, Card.Suit.HongTao, 1, Card.Effect.TaoYuanJieYi));
            AddCard(new Card(27, Card.Suit.CaoHua, 4, Card.Effect.Sha));
            AddCard(new Card(28, Card.Suit.FangPian, 7, Card.Effect.Sha));
            AddCard(new Card(29, Card.Suit.HongTao, 9, Card.Effect.Tao));
            AddCard(new Card(30, Card.Suit.HongTao, 13, Card.Effect.Jia1));
            AddCard(new Card(31, Card.Suit.HeiTao, 13, Card.Effect.NanManRuQin));
            AddCard(new Card(32, Card.Suit.CaoHua, 11, Card.Effect.Sha));
            AddCard(new Card(33, Card.Suit.HongTao, 1, Card.Effect.WanJianQiFa));
            AddCard(new Card(34, Card.Suit.HeiTao, 11, Card.Effect.ShunShouQianYang));
            AddCard(new Card(35, Card.Suit.FangPian, 13, Card.Effect.Jian1));
            AddCard(new Card(36, Card.Suit.HeiTao, 10, Card.Effect.Sha));
            AddCard(new Card(37, Card.Suit.HeiTao, 1, Card.Effect.ShanDian));
            AddCard(new Card(38, Card.Suit.HeiTao, 2, Card.Effect.BaGuaZhen));
            AddCard(new Card(39, Card.Suit.HeiTao, 7, Card.Effect.NanManRuQin));
            AddCard(new Card(40, Card.Suit.FangPian, 13, Card.Effect.Sha));
            AddCard(new Card(41, Card.Suit.CaoHua, 8, Card.Effect.Sha));
            AddCard(new Card(42, Card.Suit.CaoHua, 3, Card.Effect.Sha));
            AddCard(new Card(43, Card.Suit.FangPian, 4, Card.Effect.Shan));
            AddCard(new Card(44, Card.Suit.CaoHua, 3, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(45, Card.Suit.FangPian, 2, Card.Effect.Shan));
            AddCard(new Card(46, Card.Suit.HongTao, 4, Card.Effect.Tao));
            AddCard(new Card(47, Card.Suit.FangPian, 9, Card.Effect.Shan));
            AddCard(new Card(48, Card.Suit.HeiTao, 3, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(49, Card.Suit.HongTao, 8, Card.Effect.WuZhongShengYou));
            AddCard(new Card(50, Card.Suit.CaoHua, 2, Card.Effect.Sha));
            AddCard(new Card(51, Card.Suit.HeiTao, 4, Card.Effect.ShunShouQianYang));
            AddCard(new Card(52, Card.Suit.FangPian, 10, Card.Effect.Sha));
            AddCard(new Card(53, Card.Suit.HongTao, 13, Card.Effect.Shan));
            AddCard(new Card(54, Card.Suit.FangPian, 10, Card.Effect.Shan));
            AddCard(new Card(55, Card.Suit.FangPian, 7, Card.Effect.Shan));
            AddCard(new Card(56, Card.Suit.HongTao, 10, Card.Effect.Sha));
            AddCard(new Card(57, Card.Suit.HongTao, 7, Card.Effect.WuZhongShengYou));
            AddCard(new Card(58, Card.Suit.HongTao, 2, Card.Effect.Shan));
            AddCard(new Card(59, Card.Suit.HongTao, 7, Card.Effect.Tao));
            AddCard(new Card(60, Card.Suit.CaoHua, 8, Card.Effect.Sha));
            AddCard(new Card(61, Card.Suit.FangPian, 11, Card.Effect.Shan));
            AddCard(new Card(62, Card.Suit.CaoHua, 7, Card.Effect.NanManRuQin));
            AddCard(new Card(63, Card.Suit.FangPian, 6, Card.Effect.Sha));
            AddCard(new Card(64, Card.Suit.CaoHua, 9, Card.Effect.Sha));
            AddCard(new Card(65, Card.Suit.HongTao, 3, Card.Effect.WuGuFengDeng));
            AddCard(new Card(66, Card.Suit.FangPian, 6, Card.Effect.Shan));
            AddCard(new Card(67, Card.Suit.CaoHua, 6, Card.Effect.LeBuSiShu));
            AddCard(new Card(68, Card.Suit.FangPian, 3, Card.Effect.Shan));
            AddCard(new Card(69, Card.Suit.CaoHua, 5, Card.Effect.Sha));
            AddCard(new Card(70, Card.Suit.HongTao, 6, Card.Effect.LeBuSiShu));
            AddCard(new Card(71, Card.Suit.HeiTao, 13, Card.Effect.Jian1));
            AddCard(new Card(72, Card.Suit.CaoHua, 1, Card.Effect.ZhuGeLianNu));
            AddCard(new Card(73, Card.Suit.HongTao, 6, Card.Effect.Tao));
            AddCard(new Card(74, Card.Suit.CaoHua, 5, Card.Effect.Jia1));
            AddCard(new Card(75, Card.Suit.HeiTao, 5, Card.Effect.Jia1));
            AddCard(new Card(76, Card.Suit.CaoHua, 12, Card.Effect.JieDaoShaRen));
            AddCard(new Card(77, Card.Suit.FangPian, 12, Card.Effect.Tao));
            AddCard(new Card(78, Card.Suit.CaoHua, 12, Card.Effect.WuXieKeJi));
            AddCard(new Card(79, Card.Suit.HeiTao, 10, Card.Effect.Sha));
            AddCard(new Card(80, Card.Suit.CaoHua, 10, Card.Effect.Sha));
            AddCard(new Card(81, Card.Suit.HeiTao, 9, Card.Effect.Sha));
            AddCard(new Card(82, Card.Suit.FangPian, 1, Card.Effect.JueDou));
            AddCard(new Card(83, Card.Suit.FangPian, 5, Card.Effect.Shan));
            AddCard(new Card(84, Card.Suit.HeiTao, 6, Card.Effect.LeBuSiShu));
            AddCard(new Card(85, Card.Suit.HeiTao, 8, Card.Effect.Sha));
            AddCard(new Card(86, Card.Suit.FangPian, 8, Card.Effect.Shan));
            AddCard(new Card(87, Card.Suit.HongTao, 9, Card.Effect.WuZhongShengYou));
            AddCard(new Card(88, Card.Suit.CaoHua, 9, Card.Effect.Sha));
            AddCard(new Card(89, Card.Suit.HeiTao, 8, Card.Effect.Sha));
            AddCard(new Card(90, Card.Suit.HongTao, 11, Card.Effect.WuZhongShengYou));
            AddCard(new Card(91, Card.Suit.HeiTao, 9, Card.Effect.Sha));
            AddCard(new Card(92, Card.Suit.FangPian, 11, Card.Effect.Shan));
            AddCard(new Card(93, Card.Suit.HeiTao, 11, Card.Effect.WuXieKeJi));
            AddCard(new Card(94, Card.Suit.HongTao, 5, Card.Effect.Jian1));
            AddCard(new Card(95, Card.Suit.CaoHua, 11, Card.Effect.Sha));
            AddCard(new Card(96, Card.Suit.HongTao, 11, Card.Effect.Sha));
            AddCard(new Card(97, Card.Suit.FangPian, 9, Card.Effect.Sha));
            AddCard(new Card(98, Card.Suit.HongTao, 2, Card.Effect.Shan));
            AddCard(new Card(99, Card.Suit.HongTao, 10, Card.Effect.Sha));
            AddCard(new Card(100, Card.Suit.CaoHua, 10, Card.Effect.Sha));
            AddCard(new Card(101, Card.Suit.FangPian, 3, Card.Effect.ShunShouQianYang));
            AddCard(new Card(102, Card.Suit.HeiTao, 7, Card.Effect.Sha));
            AddCard(new Card(103, Card.Suit.HongTao, 12, Card.Effect.Tao));
            AddCard(new Card(104, Card.Suit.HongTao, 12, Card.Effect.GuoHeChaiQiao));
            CardsHeap2 = new Queue<Card>(CardsHeap2.Distinct().ToArray());
            TotalCards = CardsHeap2.Count;
        }

        /// <summary>
        /// 向弃牌堆添加ex卡牌
        /// </summary>
        public void FillExCards()
        {
            AddCard(new Card(105, Card.Suit.HeiTao, 2, Card.Effect.HanBingJian));
            AddCard(new Card(106, Card.Suit.FangPian, 12, Card.Effect.WuXieKeJi));
            AddCard(new Card(107, Card.Suit.CaoHua, 2, Card.Effect.RenWangDun));
            AddCard(new Card(108, Card.Suit.HongTao, 12, Card.Effect.ShanDian));
            CardsHeap2 = new Queue<Card>(CardsHeap2.Distinct().ToArray());
            TotalCards = CardsHeap2.Count;
        }

        /// <summary>
        /// 向弃牌堆添加神卡牌
        /// </summary>
        public void FillShenCards()
        {
            AddCard(new Card(109, Card.Suit.HongTao, 9, Card.Effect.Shan));
            AddCard(new Card(110, Card.Suit.HongTao, 11, Card.Effect.Shan));
            AddCard(new Card(111, Card.Suit.HeiTao, 2, Card.Effect.TengJia));
            AddCard(new Card(112, Card.Suit.HongTao, 10, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(113, Card.Suit.HongTao, 12, Card.Effect.Shan));
            AddCard(new Card(114, Card.Suit.FangPian, 13, Card.Effect.Jia1));
            AddCard(new Card(115, Card.Suit.FangPian, 3, Card.Effect.Tao));
            AddCard(new Card(116, Card.Suit.FangPian, 11, Card.Effect.Shan));
            AddCard(new Card(117, Card.Suit.HongTao, 8, Card.Effect.Shan));
            AddCard(new Card(118, Card.Suit.HeiTao, 4, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(119, Card.Suit.CaoHua, 2, Card.Effect.TengJia));
            AddCard(new Card(120, Card.Suit.CaoHua, 1, Card.Effect.BaiYinShiZi));
            AddCard(new Card(121, Card.Suit.HongTao, 1, Card.Effect.WuXieKeJi));
            AddCard(new Card(122, Card.Suit.FangPian, 8, Card.Effect.Shan));
            AddCard(new Card(123, Card.Suit.CaoHua, 5, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(124, Card.Suit.FangPian, 6, Card.Effect.Shan));
            AddCard(new Card(125, Card.Suit.CaoHua, 8, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(126, Card.Suit.HeiTao, 8, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(127, Card.Suit.FangPian, 2, Card.Effect.Tao));
            AddCard(new Card(128, Card.Suit.CaoHua, 6, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(129, Card.Suit.HongTao, 7, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(130, Card.Suit.HeiTao, 5, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(131, Card.Suit.FangPian, 7, Card.Effect.Shan));
            AddCard(new Card(132, Card.Suit.HongTao, 6, Card.Effect.Tao));
            AddCard(new Card(133, Card.Suit.HeiTao, 13, Card.Effect.WuXieKeJi));
            AddCard(new Card(134, Card.Suit.FangPian, 4, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(135, Card.Suit.HeiTao, 7, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(136, Card.Suit.HongTao, 5, Card.Effect.Tao));
            AddCard(new Card(137, Card.Suit.HeiTao, 6, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(138, Card.Suit.CaoHua, 7, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(139, Card.Suit.FangPian, 10, Card.Effect.Shan));
            AddCard(new Card(140, Card.Suit.HongTao, 13, Card.Effect.WuXieKeJi));
            AddCard(new Card(141, Card.Suit.HongTao, 4, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(142, Card.Suit.FangPian, 5, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(143, Card.Suit.HeiTao, 1, Card.Effect.GuDianDao));
            AddCard(new Card(144, Card.Suit.FangPian, 1, Card.Effect.ZhuQueYuShan));
            AddCard(new Card(145, Card.Suit.CaoHua, 11, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(146, Card.Suit.HongTao, 3, Card.Effect.HuoGong));
            AddCard(new Card(147, Card.Suit.CaoHua, 3, Card.Effect.Jiu));
            AddCard(new Card(148, Card.Suit.HeiTao, 11, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(149, Card.Suit.HeiTao, 9, Card.Effect.Jiu));
            AddCard(new Card(150, Card.Suit.FangPian, 9, Card.Effect.Jiu));
            AddCard(new Card(151, Card.Suit.CaoHua, 10, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(152, Card.Suit.CaoHua, 13, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(153, Card.Suit.FangPian, 12, Card.Effect.HuoGong));
            AddCard(new Card(154, Card.Suit.HeiTao, 12, Card.Effect.HuoGong));
            AddCard(new Card(155, Card.Suit.HeiTao, 3, Card.Effect.Jiu));
            AddCard(new Card(156, Card.Suit.CaoHua, 12, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(157, Card.Suit.CaoHua, 9, Card.Effect.Jiu));
            AddCard(new Card(158, Card.Suit.HongTao, 2, Card.Effect.HuoGong));
            AddCard(new Card(159, Card.Suit.CaoHua, 4, Card.Effect.BingLiangCunDuan));
            AddCard(new Card(160, Card.Suit.HeiTao, 10, Card.Effect.BingLiangCunDuan));
            CardsHeap2 = new Queue<Card>(CardsHeap2.Distinct().ToArray());
            TotalCards = CardsHeap2.Count;
        }

        /// <summary>
        /// 检查游戏中的牌信息,确定游戏中有哪些丢失的或者重复的牌
        /// </summary>
        /// <param name="aGame">游戏对象</param>
        /// <returns>XML报告</returns>
        public string CardsChecker(SGS.ServerCore.Contest.Global.GameBase aGame)
        {
            List<Card> lst = new List<Card>(CardsHeap);
            lst.AddRange(CardsHeap2);
            ChiefBase s = aGame.GamePlayers[-1].Chief;
            if (s != null)
            {
                ChiefBase t = s;
                do
                {
                    lst.AddRange(aGame.GamePlayers[t].Hands);
                    if (aGame.GamePlayers[t].Armor != null)
                        lst.Add(aGame.GamePlayers[t].Armor);
                    if (aGame.GamePlayers[t].Weapon != null)
                        lst.Add(aGame.GamePlayers[t].Weapon);
                    if (aGame.GamePlayers[t].Jia1Ma != null)
                        lst.Add(aGame.GamePlayers[t].Jia1Ma);
                    if(aGame.GamePlayers[t].Jian1Ma!= null )
                        lst.Add(aGame.GamePlayers[t].Jian1Ma);
                    lst.AddRange(aGame.GamePlayers[t].Debuff);
                    t = t.Next;
                } while (t != s);
            }
            int[] Buff = new int[TotalCards +1];
            foreach (Card c in lst)
            {
                Buff[c.ID]++;
            }
            Beaver lost = new Beaver();
            Beaver excess = new Beaver();
            bool normal = true;
            for (int i = 1; i <= TotalCards; i++)
            {
                if (Buff[i] > 1)
                {
                    excess.Add(string.Empty, new Beaver(string.Empty, i, Buff[i] - 1));
                    normal = false;
                }
                //new XElement("card",
                //    new XElement("id", i),
                //    new XElement("excess_count", Buff[i] - 1)
                //));
                else if (Buff[i] == 0)
                {
                    lost.Add(string.Empty, i);
                    normal = false;
                }
                        //new XElement("card" ,
                        //    new XElement("id" , i)
                        //    )
                        //);
            }
            if (!normal)
                return new Beaver("cardschecker.wrong_total", lost, excess).ToString();
            return new Beaver("cardschecker").ToString();
        }
        private Queue<Card> CardsHeap = new Queue<Card>(); //牌堆
        private Queue<Card> CardsHeap2 = new Queue<Card>(); //弃牌堆
    }

}
