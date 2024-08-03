/*
 * Card 和 CardHeap
 * Namespace SanGuoSha.Contest.Data
 * 定义牌和牌堆
*/
using System.Xml.Linq;
using SanGuoSha.Contest.Data.GameException;
using BeaverMarkupLanguage;


namespace SanGuoSha.Contest.Data
{
    /// <summary>
    /// 牌类
    /// </summary>
    /// <remarks>
    /// 创建一张牌
    /// </remarks>
    /// <param name="aID">牌的唯一编号</param>
    /// <param name="aSuit">花色</param>
    /// <param name="aIndex">牌号</param>
    /// <param name="aEffect">基本效果</param>
    /// <param name="aElement">牌的属性效果</param>
    public class Card(int aID, Card.Suit aSuit, int aIndex, Card.Effect aEffect, Card.ElementType aElement)
    {

        /// <summary>
        /// 创建一张没有属性的牌
        /// </summary>
        /// <param name="aID">牌的编号</param>
        /// <param name="aSuit">花色</param>
        /// <param name="aIndex">牌号</param>
        /// <param name="aEffect">基本效果</param>
        public Card(int aID, Suit aSuit, int aIndex, Effect aEffect)
            :this(aID  ,aSuit ,aIndex , aEffect , ElementType.None)
        {

        }

        /// <summary>
        /// 根据ID判断牌是否相同
        /// </summary>
        /// <param name="c">另一张牌的引用</param>
        /// <returns>若号码相同,返回true,其他情况包括null参数返回false</returns>
        public bool IsSame(Card? c)
        {
            if (c == null) return false;
            return c.ID == ID;
        }

        /// <summary>
        /// 花色枚举
        /// </summary>
        public enum Suit { 
            /// <summary>
            /// 红桃
            /// </summary>
            Heart,
            /// <summary>
            /// 草花
            /// </summary>
            Club, 
            /// <summary>
            /// 黑桃
            /// </summary>
            Spade, 
            /// <summary>
            /// 方片
            /// </summary>
            Diamond, 
        };

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
        public readonly int ID = aID;

        /// <summary>
        /// 这张牌的花色
        /// </summary>
        public Suit CardSuit = aSuit;

        /// <summary>
        /// 这张牌的牌号，只读
        /// </summary>
        public readonly int CardIndex = aIndex;
        /// <summary>
        /// 原效果，只读
        /// </summary>
        public readonly Effect OriginEffect = aEffect;
        /// <summary>
        /// 原花色，只读
        /// </summary>
        public readonly Suit OriginSuit = aSuit;
        /// <summary>
        /// 牌的效果
        /// </summary>
        public Effect CardEffect = aEffect;


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
        } = aElement;

        /// <summary>
        /// 牌原有的属性
        /// </summary>
        public readonly ElementType OriginElement = aElement;


        /// <summary>
        /// 获得这张牌的原效果副本（清除上面的多余特殊效果）
        /// </summary>
        /// <returns></returns>
        public Card GetOriginalCard()
        {
            return new Card(ID, OriginSuit, CardIndex, OriginEffect , OriginElement);
        }

        /// <summary>
        /// 将牌数组转化为牌号码的XML节
        /// </summary>
        /// <param name="aNodeName">节名称</param>
        /// <param name="aCards">牌数组</param>
        /// <returns>一个XML形式的牌号码对象</returns>
        internal static XElement Cards2XML(string aNodeName, Card[] aCards)
        {
            XElement xcards = new(aNodeName);
            foreach (Card c in aCards)
                xcards.Add(new XElement("card", c.ID));
            return xcards;
        }

        internal static BeaverMarkupLanguage.Beaver Cards2Beaver(string aNodeName, Card[] aCards)
        {
            Beaver ret = [];
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
        public static readonly Card Unknown = new(0, Card.Suit.Spade, 1, Card.Effect.None);

        /// <summary>
        /// 黑桃
        /// </summary>
        public static readonly Card Spade = new(-1, Card.Suit.Spade, 1, Card.Effect.None);
        /// <summary>
        /// 草花
        /// </summary>
        public static readonly Card Club = new(-2, Card.Suit.Club, 1, Card.Effect.None);
        /// <summary>
        /// 红桃
        /// </summary>
        public static readonly Card Heart = new(-3, Card.Suit.Heart, 1, Card.Effect.None);
        /// <summary>
        /// 方片
        /// </summary>
        public static readonly Card Diamond = new(-4, Card.Suit.Diamond, 1, Card.Effect.None);

        /// <summary>
        /// 构造牌堆
        /// </summary>
        public CardHeap(SanGuoSha.Contest.Global.GameBase aGame)
        {
            //_random = new RNGCryptoServiceProvider();
            _game = aGame;
        }

        private SanGuoSha.Contest.Global.GameBase _game = null;

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
        public void FillCards()
        {
            List<Card> lst = [];
            foreach (Card c in TrashCardsHeap)
            {
                lst.Add(c.GetOriginalCard());
            }

            Card[] arr = [.. lst];
            for (int i = 0; i < lst.Count; i++)
            {
                int a = GetRandom(lst.Count);
                int b = GetRandom(lst.Count);

                (arr[b], arr[a]) = (arr[a], arr[b]);
            }
            lst = [.. arr];
            foreach (Card c in lst)
            {
                TakingCardsHeap.Enqueue(c);
            }
            TrashCardsHeap.Clear();
        }

        /// <summary>
        /// 将一张牌加入弃牌堆
        /// </summary>
        /// <param name="aCard">牌对象</param>
        public void AddCard(Card aCard)
        {
            TrashCardsHeap.Enqueue(aCard.GetOriginalCard());
        }

        /// <summary>
        /// 加入一批牌到弃牌堆
        /// </summary>
        /// <param name="aCards">被弃牌的数组</param>
        public void AddCards(Card[] aCards)
        {
            foreach (Card c in aCards)
                TrashCardsHeap.Enqueue(c);
        }

        /// <summary>
        /// 从牌堆中哪一张牌
        /// </summary>
        /// <returns>若无法给出一张牌,则返回null.否则返回牌对象</returns>
        public Card Pop()
        {
            if (TakingCardsHeap.Count == 0)
            {
                FillCards();
            }
            if(TakingCardsHeap.Count > 0)
                return TakingCardsHeap.Dequeue();
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
            if (TakingCardsHeap.Where(c => c.ID == aID).Any())
            {
                TakingCardsHeap = new Queue<Card>(TakingCardsHeap.Where(c=>c.ID != aID));
                return true;
            }
            else if (TrashCardsHeap.Where(c => c.ID == aID).Any())
            {
                TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Where(c => c.ID != aID));
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
            if (aCards.Length == 0) return true;
            List<Card> lst = new(aCards);
            foreach (Card c in TakingCardsHeap)
                lst.Add(c);
            TakingCardsHeap = new Queue<Card>(lst);
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
                TakingCardsHeap.Enqueue(c);
            return true;
        }

        /// <summary>
        /// 从牌堆中拿n张牌
        /// </summary>
        /// <param name="n">拿出的数量</param>
        /// <returns>牌的数组</returns>
        public Card[] Pop(int n)
        {
            List<Card> lst = [];
            if (n < 1) return [.. lst];
            if (n > TakingCardsHeap.Count + TrashCardsHeap.Count) throw new NoMoreCard(_game.GamePlayers);
            while (n > 0)
            {
                if (TakingCardsHeap.Count == 0)
                    FillCards();
                lst.Add( TakingCardsHeap.Dequeue());
                n--;
            }
            return [.. lst];
        }

        /// <summary>
        /// 判断牌是否在弃牌堆或者发牌堆
        /// </summary>
        /// <param name="aCard">牌对象</param>
        /// <returns>若存在则返回true</returns>
        public bool Exist(Card aCard)
        {
            if (TakingCardsHeap.Contains(aCard) || TrashCardsHeap.Contains(aCard)) return true;
            return false;
        }

        /// <summary>
        /// 向弃牌堆加入原版的卡牌
        /// </summary>
        public void FillOriginCards()
        {
            AddCard(new Card(1, Card.Suit.Club, 12, Card.Effect.ZhangBaSheMao));
            AddCard(new Card(2, Card.Suit.Diamond, 5, Card.Effect.GuanShiFu));
            AddCard(new Card(3, Card.Suit.Spade, 5, Card.Effect.QingLongYanYueDao));
            AddCard(new Card(4, Card.Suit.Spade, 2, Card.Effect.CiXiongShuangGuJian));
            AddCard(new Card(5, Card.Suit.Spade, 6, Card.Effect.QingGangJian));
            AddCard(new Card(6, Card.Suit.Diamond, 12, Card.Effect.FangTianHuaJi));
            AddCard(new Card(7, Card.Suit.Diamond, 1, Card.Effect.ZhuGeLianNu));
            AddCard(new Card(8, Card.Suit.Heart, 5, Card.Effect.QiLinGong));
            AddCard(new Card(9, Card.Suit.Club, 2, Card.Effect.BaGuaZhen));
            AddCard(new Card(10, Card.Suit.Spade, 12, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(11, Card.Suit.Club, 1, Card.Effect.JueDou));
            AddCard(new Card(12 , Card.Suit.Club , 6 , Card.Effect.Sha));
            AddCard(new Card(13 , Card.Suit.Heart , 3 , Card.Effect.Tao));
            AddCard(new Card(14, Card.Suit.Heart, 4, Card.Effect.WuGuFengDeng));
            AddCard(new Card(15, Card.Suit.Diamond, 4, Card.Effect.ShunShouQianYang));
            AddCard(new Card(16, Card.Suit.Club, 4, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(17, Card.Suit.Spade, 4, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(18, Card.Suit.Diamond, 2, Card.Effect.Shan));
            AddCard(new Card(19, Card.Suit.Heart, 8, Card.Effect.Tao));
            AddCard(new Card(20, Card.Suit.Spade, 3, Card.Effect.ShunShouQianYang));
            AddCard(new Card(21, Card.Suit.Diamond, 8, Card.Effect.Sha));
            AddCard(new Card(22, Card.Suit.Spade, 1, Card.Effect.JueDou));
            AddCard(new Card(23, Card.Suit.Club, 13, Card.Effect.JieDaoShaRen));
            AddCard(new Card(24, Card.Suit.Club, 7, Card.Effect.Sha));
            AddCard(new Card(25, Card.Suit.Club, 13, Card.Effect.WuXieKeJi));
            AddCard(new Card(26, Card.Suit.Heart, 1, Card.Effect.TaoYuanJieYi));
            AddCard(new Card(27, Card.Suit.Club, 4, Card.Effect.Sha));
            AddCard(new Card(28, Card.Suit.Diamond, 7, Card.Effect.Sha));
            AddCard(new Card(29, Card.Suit.Heart, 9, Card.Effect.Tao));
            AddCard(new Card(30, Card.Suit.Heart, 13, Card.Effect.Jia1));
            AddCard(new Card(31, Card.Suit.Spade, 13, Card.Effect.NanManRuQin));
            AddCard(new Card(32, Card.Suit.Club, 11, Card.Effect.Sha));
            AddCard(new Card(33, Card.Suit.Heart, 1, Card.Effect.WanJianQiFa));
            AddCard(new Card(34, Card.Suit.Spade, 11, Card.Effect.ShunShouQianYang));
            AddCard(new Card(35, Card.Suit.Diamond, 13, Card.Effect.Jian1));
            AddCard(new Card(36, Card.Suit.Spade, 10, Card.Effect.Sha));
            AddCard(new Card(37, Card.Suit.Spade, 1, Card.Effect.ShanDian));
            AddCard(new Card(38, Card.Suit.Spade, 2, Card.Effect.BaGuaZhen));
            AddCard(new Card(39, Card.Suit.Spade, 7, Card.Effect.NanManRuQin));
            AddCard(new Card(40, Card.Suit.Diamond, 13, Card.Effect.Sha));
            AddCard(new Card(41, Card.Suit.Club, 8, Card.Effect.Sha));
            AddCard(new Card(42, Card.Suit.Club, 3, Card.Effect.Sha));
            AddCard(new Card(43, Card.Suit.Diamond, 4, Card.Effect.Shan));
            AddCard(new Card(44, Card.Suit.Club, 3, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(45, Card.Suit.Diamond, 2, Card.Effect.Shan));
            AddCard(new Card(46, Card.Suit.Heart, 4, Card.Effect.Tao));
            AddCard(new Card(47, Card.Suit.Diamond, 9, Card.Effect.Shan));
            AddCard(new Card(48, Card.Suit.Spade, 3, Card.Effect.GuoHeChaiQiao));
            AddCard(new Card(49, Card.Suit.Heart, 8, Card.Effect.WuZhongShengYou));
            AddCard(new Card(50, Card.Suit.Club, 2, Card.Effect.Sha));
            AddCard(new Card(51, Card.Suit.Spade, 4, Card.Effect.ShunShouQianYang));
            AddCard(new Card(52, Card.Suit.Diamond, 10, Card.Effect.Sha));
            AddCard(new Card(53, Card.Suit.Heart, 13, Card.Effect.Shan));
            AddCard(new Card(54, Card.Suit.Diamond, 10, Card.Effect.Shan));
            AddCard(new Card(55, Card.Suit.Diamond, 7, Card.Effect.Shan));
            AddCard(new Card(56, Card.Suit.Heart, 10, Card.Effect.Sha));
            AddCard(new Card(57, Card.Suit.Heart, 7, Card.Effect.WuZhongShengYou));
            AddCard(new Card(58, Card.Suit.Heart, 2, Card.Effect.Shan));
            AddCard(new Card(59, Card.Suit.Heart, 7, Card.Effect.Tao));
            AddCard(new Card(60, Card.Suit.Club, 8, Card.Effect.Sha));
            AddCard(new Card(61, Card.Suit.Diamond, 11, Card.Effect.Shan));
            AddCard(new Card(62, Card.Suit.Club, 7, Card.Effect.NanManRuQin));
            AddCard(new Card(63, Card.Suit.Diamond, 6, Card.Effect.Sha));
            AddCard(new Card(64, Card.Suit.Club, 9, Card.Effect.Sha));
            AddCard(new Card(65, Card.Suit.Heart, 3, Card.Effect.WuGuFengDeng));
            AddCard(new Card(66, Card.Suit.Diamond, 6, Card.Effect.Shan));
            AddCard(new Card(67, Card.Suit.Club, 6, Card.Effect.LeBuSiShu));
            AddCard(new Card(68, Card.Suit.Diamond, 3, Card.Effect.Shan));
            AddCard(new Card(69, Card.Suit.Club, 5, Card.Effect.Sha));
            AddCard(new Card(70, Card.Suit.Heart, 6, Card.Effect.LeBuSiShu));
            AddCard(new Card(71, Card.Suit.Spade, 13, Card.Effect.Jian1));
            AddCard(new Card(72, Card.Suit.Club, 1, Card.Effect.ZhuGeLianNu));
            AddCard(new Card(73, Card.Suit.Heart, 6, Card.Effect.Tao));
            AddCard(new Card(74, Card.Suit.Club, 5, Card.Effect.Jia1));
            AddCard(new Card(75, Card.Suit.Spade, 5, Card.Effect.Jia1));
            AddCard(new Card(76, Card.Suit.Club, 12, Card.Effect.JieDaoShaRen));
            AddCard(new Card(77, Card.Suit.Diamond, 12, Card.Effect.Tao));
            AddCard(new Card(78, Card.Suit.Club, 12, Card.Effect.WuXieKeJi));
            AddCard(new Card(79, Card.Suit.Spade, 10, Card.Effect.Sha));
            AddCard(new Card(80, Card.Suit.Club, 10, Card.Effect.Sha));
            AddCard(new Card(81, Card.Suit.Spade, 9, Card.Effect.Sha));
            AddCard(new Card(82, Card.Suit.Diamond, 1, Card.Effect.JueDou));
            AddCard(new Card(83, Card.Suit.Diamond, 5, Card.Effect.Shan));
            AddCard(new Card(84, Card.Suit.Spade, 6, Card.Effect.LeBuSiShu));
            AddCard(new Card(85, Card.Suit.Spade, 8, Card.Effect.Sha));
            AddCard(new Card(86, Card.Suit.Diamond, 8, Card.Effect.Shan));
            AddCard(new Card(87, Card.Suit.Heart, 9, Card.Effect.WuZhongShengYou));
            AddCard(new Card(88, Card.Suit.Club, 9, Card.Effect.Sha));
            AddCard(new Card(89, Card.Suit.Spade, 8, Card.Effect.Sha));
            AddCard(new Card(90, Card.Suit.Heart, 11, Card.Effect.WuZhongShengYou));
            AddCard(new Card(91, Card.Suit.Spade, 9, Card.Effect.Sha));
            AddCard(new Card(92, Card.Suit.Diamond, 11, Card.Effect.Shan));
            AddCard(new Card(93, Card.Suit.Spade, 11, Card.Effect.WuXieKeJi));
            AddCard(new Card(94, Card.Suit.Heart, 5, Card.Effect.Jian1));
            AddCard(new Card(95, Card.Suit.Club, 11, Card.Effect.Sha));
            AddCard(new Card(96, Card.Suit.Heart, 11, Card.Effect.Sha));
            AddCard(new Card(97, Card.Suit.Diamond, 9, Card.Effect.Sha));
            AddCard(new Card(98, Card.Suit.Heart, 2, Card.Effect.Shan));
            AddCard(new Card(99, Card.Suit.Heart, 10, Card.Effect.Sha));
            AddCard(new Card(100, Card.Suit.Club, 10, Card.Effect.Sha));
            AddCard(new Card(101, Card.Suit.Diamond, 3, Card.Effect.ShunShouQianYang));
            AddCard(new Card(102, Card.Suit.Spade, 7, Card.Effect.Sha));
            AddCard(new Card(103, Card.Suit.Heart, 12, Card.Effect.Tao));
            AddCard(new Card(104, Card.Suit.Heart, 12, Card.Effect.GuoHeChaiQiao));
            TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Distinct().ToArray());
            TotalCards = TrashCardsHeap.Count;
        }

        /// <summary>
        /// 向弃牌堆添加ex卡牌
        /// </summary>
        public void FillExCards()
        {
            AddCard(new Card(105, Card.Suit.Spade, 2, Card.Effect.HanBingJian));
            AddCard(new Card(106, Card.Suit.Diamond, 12, Card.Effect.WuXieKeJi));
            AddCard(new Card(107, Card.Suit.Club, 2, Card.Effect.RenWangDun));
            AddCard(new Card(108, Card.Suit.Heart, 12, Card.Effect.ShanDian));
            TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Distinct().ToArray());
            TotalCards = TrashCardsHeap.Count;
        }

        /// <summary>
        /// 向弃牌堆添加神卡牌
        /// </summary>
        public void FillShenCards()
        {
            AddCard(new Card(109, Card.Suit.Heart, 9, Card.Effect.Shan));
            AddCard(new Card(110, Card.Suit.Heart, 11, Card.Effect.Shan));
            AddCard(new Card(111, Card.Suit.Spade, 2, Card.Effect.TengJia));
            AddCard(new Card(112, Card.Suit.Heart, 10, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(113, Card.Suit.Heart, 12, Card.Effect.Shan));
            AddCard(new Card(114, Card.Suit.Diamond, 13, Card.Effect.Jia1));
            AddCard(new Card(115, Card.Suit.Diamond, 3, Card.Effect.Tao));
            AddCard(new Card(116, Card.Suit.Diamond, 11, Card.Effect.Shan));
            AddCard(new Card(117, Card.Suit.Heart, 8, Card.Effect.Shan));
            AddCard(new Card(118, Card.Suit.Spade, 4, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(119, Card.Suit.Club, 2, Card.Effect.TengJia));
            AddCard(new Card(120, Card.Suit.Club, 1, Card.Effect.BaiYinShiZi));
            AddCard(new Card(121, Card.Suit.Heart, 1, Card.Effect.WuXieKeJi));
            AddCard(new Card(122, Card.Suit.Diamond, 8, Card.Effect.Shan));
            AddCard(new Card(123, Card.Suit.Club, 5, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(124, Card.Suit.Diamond, 6, Card.Effect.Shan));
            AddCard(new Card(125, Card.Suit.Club, 8, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(126, Card.Suit.Spade, 8, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(127, Card.Suit.Diamond, 2, Card.Effect.Tao));
            AddCard(new Card(128, Card.Suit.Club, 6, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(129, Card.Suit.Heart, 7, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(130, Card.Suit.Spade, 5, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(131, Card.Suit.Diamond, 7, Card.Effect.Shan));
            AddCard(new Card(132, Card.Suit.Heart, 6, Card.Effect.Tao));
            AddCard(new Card(133, Card.Suit.Spade, 13, Card.Effect.WuXieKeJi));
            AddCard(new Card(134, Card.Suit.Diamond, 4, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(135, Card.Suit.Spade, 7, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(136, Card.Suit.Heart, 5, Card.Effect.Tao));
            AddCard(new Card(137, Card.Suit.Spade, 6, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(138, Card.Suit.Club, 7, Card.Effect.Sha, Card.ElementType.Thunder));
            AddCard(new Card(139, Card.Suit.Diamond, 10, Card.Effect.Shan));
            AddCard(new Card(140, Card.Suit.Heart, 13, Card.Effect.WuXieKeJi));
            AddCard(new Card(141, Card.Suit.Heart, 4, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(142, Card.Suit.Diamond, 5, Card.Effect.Sha, Card.ElementType.Fire));
            AddCard(new Card(143, Card.Suit.Spade, 1, Card.Effect.GuDianDao));
            AddCard(new Card(144, Card.Suit.Diamond, 1, Card.Effect.ZhuQueYuShan));
            AddCard(new Card(145, Card.Suit.Club, 11, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(146, Card.Suit.Heart, 3, Card.Effect.HuoGong));
            AddCard(new Card(147, Card.Suit.Club, 3, Card.Effect.Jiu));
            AddCard(new Card(148, Card.Suit.Spade, 11, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(149, Card.Suit.Spade, 9, Card.Effect.Jiu));
            AddCard(new Card(150, Card.Suit.Diamond, 9, Card.Effect.Jiu));
            AddCard(new Card(151, Card.Suit.Club, 10, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(152, Card.Suit.Club, 13, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(153, Card.Suit.Diamond, 12, Card.Effect.HuoGong));
            AddCard(new Card(154, Card.Suit.Spade, 12, Card.Effect.HuoGong));
            AddCard(new Card(155, Card.Suit.Spade, 3, Card.Effect.Jiu));
            AddCard(new Card(156, Card.Suit.Club, 12, Card.Effect.TieSuoLianHuan));
            AddCard(new Card(157, Card.Suit.Club, 9, Card.Effect.Jiu));
            AddCard(new Card(158, Card.Suit.Heart, 2, Card.Effect.HuoGong));
            AddCard(new Card(159, Card.Suit.Club, 4, Card.Effect.BingLiangCunDuan));
            AddCard(new Card(160, Card.Suit.Spade, 10, Card.Effect.BingLiangCunDuan));
            TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Distinct().ToArray());
            TotalCards = TrashCardsHeap.Count;
        }

        /// <summary>
        /// 检查游戏中的牌信息,确定游戏中有哪些丢失的或者重复的牌
        /// </summary>
        /// <param name="aGame">游戏对象</param>
        /// <returns>XML报告</returns>
        public string CardsChecker(SanGuoSha.Contest.Global.GameBase aGame)
        {
            List<Card> lst = new(TakingCardsHeap);
            lst.AddRange(TrashCardsHeap);
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
            Beaver lost = [];
            Beaver excess = [];
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
        private Queue<Card> TakingCardsHeap = new(); //牌堆
        private Queue<Card> TrashCardsHeap = new(); //弃牌堆
    }

}
