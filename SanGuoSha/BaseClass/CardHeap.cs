

namespace SanGuoSha.BaseClass
{
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

        public bool ShuffleTrashHeap
        {
            get;
            set;
        } = true;

        /// <summary>
        /// 未知牌
        /// </summary>
        public static readonly Card Unknown = new(0, Card.Suit.Spade, 1, CardEffect.None);

        /// <summary>
        /// 黑桃
        /// </summary>
        public static readonly Card Spade = new(-1, Card.Suit.Spade, 1, CardEffect.None);
        /// <summary>
        /// 梅花
        /// </summary>
        public static readonly Card Club = new(-2, Card.Suit.Club, 1, CardEffect.None);
        /// <summary>
        /// 红桃
        /// </summary>
        public static readonly Card Heart = new(-3, Card.Suit.Heart, 1, CardEffect.None);
        /// <summary>
        /// 方片
        /// </summary>
        public static readonly Card Diamond = new(-4, Card.Suit.Diamond, 1, CardEffect.None);

        public readonly Random Random = new();

        /// <summary>
        /// 构造牌堆
        /// </summary>
        public CardHeap()
        {
        }

        /// <summary>
        /// 释放对象的非托管资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                }
                // 释放非托管资源
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

        /// <summary>
        /// 将弃牌堆的牌洗好放入拿牌堆
        /// </summary>
        public void FillCards()
        {
            List<Card> lst = new(TrashCardsHeap);

            Card[] arr = [.. lst];
            if(ShuffleTrashHeap)
                Random.Shuffle(arr);
            lst = [.. arr];
            foreach (var c in lst)
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
            TrashCardsHeap.Enqueue(aCard);
        }

        /// <summary>
        /// 加入一批牌到弃牌堆
        /// </summary>
        /// <param name="aCards">被弃牌的数组</param>
        public void AddCards(Card[] aCards)
        {
            foreach (var c in aCards)
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
            if (TakingCardsHeap.Count > 0)
                return TakingCardsHeap.Dequeue();
            else
                throw new NoMoreCard();
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
            if (n > TakingCardsHeap.Count + TrashCardsHeap.Count) throw new NoMoreCard();
            while (n > 0)
            {
                if (TakingCardsHeap.Count == 0)
                    FillCards();
                lst.Add(TakingCardsHeap.Dequeue());
                n--;
            }
            return [.. lst];
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
                TakingCardsHeap = new Queue<Card>(TakingCardsHeap.Where(c => c.ID != aID));
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
            foreach (var c in TakingCardsHeap)
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
            if (aCards.Length == 0) return true;
            foreach (var c in aCards)
                TakingCardsHeap.Enqueue(c);
            return true;
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
            AddCard(new Card(1, Card.Suit.Club, 12, CardEffect.丈八蛇矛));
            AddCard(new Card(2, Card.Suit.Diamond, 5, CardEffect.贯石斧));
            AddCard(new Card(3, Card.Suit.Spade, 5, CardEffect.青龙偃月刀));
            AddCard(new Card(4, Card.Suit.Spade, 2, CardEffect.雌雄双股剑));
            AddCard(new Card(5, Card.Suit.Spade, 6, CardEffect.青钢剑));
            AddCard(new Card(6, Card.Suit.Diamond, 12, CardEffect.方天画戟));
            AddCard(new Card(7, Card.Suit.Diamond, 1, CardEffect.诸葛连弩));
            AddCard(new Card(8, Card.Suit.Heart, 5, CardEffect.麒麟弓));
            AddCard(new Card(9, Card.Suit.Club, 2, CardEffect.八卦阵));
            AddCard(new Card(10, Card.Suit.Spade, 12, CardEffect.过河拆桥));
            AddCard(new Card(11, Card.Suit.Club, 1, CardEffect.决斗));
            AddCard(new Card(12, Card.Suit.Club, 6, CardEffect.杀));
            AddCard(new Card(13, Card.Suit.Heart, 3, CardEffect.桃));
            AddCard(new Card(14, Card.Suit.Heart, 4, CardEffect.五谷丰登));
            AddCard(new Card(15, Card.Suit.Diamond, 4, CardEffect.顺手牵羊));
            AddCard(new Card(16, Card.Suit.Club, 4, CardEffect.过河拆桥));
            AddCard(new Card(17, Card.Suit.Spade, 4, CardEffect.过河拆桥));
            AddCard(new Card(18, Card.Suit.Diamond, 2, CardEffect.闪));
            AddCard(new Card(19, Card.Suit.Heart, 8, CardEffect.桃));
            AddCard(new Card(20, Card.Suit.Spade, 3, CardEffect.顺手牵羊));
            AddCard(new Card(21, Card.Suit.Diamond, 8, CardEffect.杀));
            AddCard(new Card(22, Card.Suit.Spade, 1, CardEffect.决斗));
            AddCard(new Card(23, Card.Suit.Club, 13, CardEffect.借刀杀人));
            AddCard(new Card(24, Card.Suit.Club, 7, CardEffect.杀));
            AddCard(new Card(25, Card.Suit.Club, 13, CardEffect.无懈可击));
            AddCard(new Card(26, Card.Suit.Heart, 1, CardEffect.桃园结义));
            AddCard(new Card(27, Card.Suit.Club, 4, CardEffect.杀));
            AddCard(new Card(28, Card.Suit.Diamond, 7, CardEffect.杀));
            AddCard(new Card(29, Card.Suit.Heart, 9, CardEffect.桃));
            AddCard(new Card(30, Card.Suit.Heart, 13, CardEffect.加1马, Card.ElementType.None, "爪黄飞电"));
            AddCard(new Card(31, Card.Suit.Spade, 13, CardEffect.南蛮入侵));
            AddCard(new Card(32, Card.Suit.Club, 11, CardEffect.杀));
            AddCard(new Card(33, Card.Suit.Heart, 1, CardEffect.万箭齐发));
            AddCard(new Card(34, Card.Suit.Spade, 11, CardEffect.顺手牵羊));
            AddCard(new Card(35, Card.Suit.Diamond, 13, CardEffect.减1马, Card.ElementType.None, "紫骍"));
            AddCard(new Card(36, Card.Suit.Spade, 10, CardEffect.杀));
            AddCard(new Card(37, Card.Suit.Spade, 1, CardEffect.闪电));
            AddCard(new Card(38, Card.Suit.Spade, 2, CardEffect.八卦阵));
            AddCard(new Card(39, Card.Suit.Spade, 7, CardEffect.南蛮入侵));
            AddCard(new Card(40, Card.Suit.Diamond, 13, CardEffect.杀));
            AddCard(new Card(41, Card.Suit.Club, 8, CardEffect.杀));
            AddCard(new Card(42, Card.Suit.Club, 3, CardEffect.杀));
            AddCard(new Card(43, Card.Suit.Diamond, 4, CardEffect.闪));
            AddCard(new Card(44, Card.Suit.Club, 3, CardEffect.过河拆桥));
            AddCard(new Card(45, Card.Suit.Diamond, 2, CardEffect.闪));
            AddCard(new Card(46, Card.Suit.Heart, 4, CardEffect.桃));
            AddCard(new Card(47, Card.Suit.Diamond, 9, CardEffect.闪));
            AddCard(new Card(48, Card.Suit.Spade, 3, CardEffect.过河拆桥));
            AddCard(new Card(49, Card.Suit.Heart, 8, CardEffect.无中生有));
            AddCard(new Card(50, Card.Suit.Club, 2, CardEffect.杀));
            AddCard(new Card(51, Card.Suit.Spade, 4, CardEffect.顺手牵羊));
            AddCard(new Card(52, Card.Suit.Diamond, 10, CardEffect.杀));
            AddCard(new Card(53, Card.Suit.Heart, 13, CardEffect.闪));
            AddCard(new Card(54, Card.Suit.Diamond, 10, CardEffect.闪));
            AddCard(new Card(55, Card.Suit.Diamond, 7, CardEffect.闪));
            AddCard(new Card(56, Card.Suit.Heart, 10, CardEffect.杀));
            AddCard(new Card(57, Card.Suit.Heart, 7, CardEffect.无中生有));
            AddCard(new Card(58, Card.Suit.Heart, 2, CardEffect.闪));
            AddCard(new Card(59, Card.Suit.Heart, 7, CardEffect.桃));
            AddCard(new Card(60, Card.Suit.Club, 8, CardEffect.杀));
            AddCard(new Card(61, Card.Suit.Diamond, 11, CardEffect.闪));
            AddCard(new Card(62, Card.Suit.Club, 7, CardEffect.南蛮入侵));
            AddCard(new Card(63, Card.Suit.Diamond, 6, CardEffect.杀));
            AddCard(new Card(64, Card.Suit.Club, 9, CardEffect.杀));
            AddCard(new Card(65, Card.Suit.Heart, 3, CardEffect.五谷丰登));
            AddCard(new Card(66, Card.Suit.Diamond, 6, CardEffect.闪));
            AddCard(new Card(67, Card.Suit.Club, 6, CardEffect.乐不思蜀));
            AddCard(new Card(68, Card.Suit.Diamond, 3, CardEffect.闪));
            AddCard(new Card(69, Card.Suit.Club, 5, CardEffect.杀));
            AddCard(new Card(70, Card.Suit.Heart, 6, CardEffect.乐不思蜀));
            AddCard(new Card(71, Card.Suit.Spade, 13, CardEffect.减1马, Card.ElementType.None, "大宛"));
            AddCard(new Card(72, Card.Suit.Club, 1, CardEffect.诸葛连弩));
            AddCard(new Card(73, Card.Suit.Heart, 6, CardEffect.桃));
            AddCard(new Card(74, Card.Suit.Club, 5, CardEffect.加1马, Card.ElementType.None, "的卢"));
            AddCard(new Card(75, Card.Suit.Spade, 5, CardEffect.加1马, Card.ElementType.None, "绝影"));
            AddCard(new Card(76, Card.Suit.Club, 12, CardEffect.借刀杀人));
            AddCard(new Card(77, Card.Suit.Diamond, 12, CardEffect.桃));
            AddCard(new Card(78, Card.Suit.Club, 12, CardEffect.无懈可击));
            AddCard(new Card(79, Card.Suit.Spade, 10, CardEffect.杀));
            AddCard(new Card(80, Card.Suit.Club, 10, CardEffect.杀));
            AddCard(new Card(81, Card.Suit.Spade, 9, CardEffect.杀));
            AddCard(new Card(82, Card.Suit.Diamond, 1, CardEffect.决斗));
            AddCard(new Card(83, Card.Suit.Diamond, 5, CardEffect.闪));
            AddCard(new Card(84, Card.Suit.Spade, 6, CardEffect.乐不思蜀));
            AddCard(new Card(85, Card.Suit.Spade, 8, CardEffect.杀));
            AddCard(new Card(86, Card.Suit.Diamond, 8, CardEffect.闪));
            AddCard(new Card(87, Card.Suit.Heart, 9, CardEffect.无中生有));
            AddCard(new Card(88, Card.Suit.Club, 9, CardEffect.杀));
            AddCard(new Card(89, Card.Suit.Spade, 8, CardEffect.杀));
            AddCard(new Card(90, Card.Suit.Heart, 11, CardEffect.无中生有));
            AddCard(new Card(91, Card.Suit.Spade, 9, CardEffect.杀));
            AddCard(new Card(92, Card.Suit.Diamond, 11, CardEffect.闪));
            AddCard(new Card(93, Card.Suit.Spade, 11, CardEffect.无懈可击));
            AddCard(new Card(94, Card.Suit.Heart, 5, CardEffect.减1马, Card.ElementType.None, "赤兔"));
            AddCard(new Card(95, Card.Suit.Club, 11, CardEffect.杀));
            AddCard(new Card(96, Card.Suit.Heart, 11, CardEffect.杀));
            AddCard(new Card(97, Card.Suit.Diamond, 9, CardEffect.杀));
            AddCard(new Card(98, Card.Suit.Heart, 2, CardEffect.闪));
            AddCard(new Card(99, Card.Suit.Heart, 10, CardEffect.杀));
            AddCard(new Card(100, Card.Suit.Club, 10, CardEffect.杀));
            AddCard(new Card(101, Card.Suit.Diamond, 3, CardEffect.顺手牵羊));
            AddCard(new Card(102, Card.Suit.Spade, 7, CardEffect.杀));
            AddCard(new Card(103, Card.Suit.Heart, 12, CardEffect.桃));
            AddCard(new Card(104, Card.Suit.Heart, 12, CardEffect.过河拆桥));
            TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Distinct().ToArray());
            TotalCards = TrashCardsHeap.Count;
        }

        /// <summary>
        /// 向弃牌堆添加ex卡牌
        /// </summary>
        public void FillExCards()
        {
            AddCard(new Card(105, Card.Suit.Spade, 2, CardEffect.寒冰箭));
            AddCard(new Card(106, Card.Suit.Diamond, 12, CardEffect.无懈可击));
            AddCard(new Card(107, Card.Suit.Club, 2, CardEffect.仁王盾));
            AddCard(new Card(108, Card.Suit.Heart, 12, CardEffect.闪电));
            TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Distinct().ToArray());
            TotalCards = TrashCardsHeap.Count;
        }

        /// <summary>
        /// 向弃牌堆添加神卡牌
        /// </summary>
        public void FillShenCards()
        {
            AddCard(new Card(109, Card.Suit.Heart, 9, CardEffect.闪));
            AddCard(new Card(110, Card.Suit.Heart, 11, CardEffect.闪));
            AddCard(new Card(111, Card.Suit.Spade, 2, CardEffect.藤甲));
            AddCard(new Card(112, Card.Suit.Heart, 10, CardEffect.杀, Card.ElementType.Fire));
            AddCard(new Card(113, Card.Suit.Heart, 12, CardEffect.闪));
            AddCard(new Card(114, Card.Suit.Diamond, 13, CardEffect.加1马, Card.ElementType.None, "骅骝"));
            AddCard(new Card(115, Card.Suit.Diamond, 3, CardEffect.桃));
            AddCard(new Card(116, Card.Suit.Diamond, 11, CardEffect.闪));
            AddCard(new Card(117, Card.Suit.Heart, 8, CardEffect.闪));
            AddCard(new Card(118, Card.Suit.Spade, 4, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(119, Card.Suit.Club, 2, CardEffect.藤甲));
            AddCard(new Card(120, Card.Suit.Club, 1, CardEffect.白银狮子));
            AddCard(new Card(121, Card.Suit.Heart, 1, CardEffect.无懈可击));
            AddCard(new Card(122, Card.Suit.Diamond, 8, CardEffect.闪));
            AddCard(new Card(123, Card.Suit.Club, 5, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(124, Card.Suit.Diamond, 6, CardEffect.闪));
            AddCard(new Card(125, Card.Suit.Club, 8, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(126, Card.Suit.Spade, 8, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(127, Card.Suit.Diamond, 2, CardEffect.桃));
            AddCard(new Card(128, Card.Suit.Club, 6, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(129, Card.Suit.Heart, 7, CardEffect.杀, Card.ElementType.Fire));
            AddCard(new Card(130, Card.Suit.Spade, 5, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(131, Card.Suit.Diamond, 7, CardEffect.闪));
            AddCard(new Card(132, Card.Suit.Heart, 6, CardEffect.桃));
            AddCard(new Card(133, Card.Suit.Spade, 13, CardEffect.无懈可击));
            AddCard(new Card(134, Card.Suit.Diamond, 4, CardEffect.杀, Card.ElementType.Fire));
            AddCard(new Card(135, Card.Suit.Spade, 7, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(136, Card.Suit.Heart, 5, CardEffect.桃));
            AddCard(new Card(137, Card.Suit.Spade, 6, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(138, Card.Suit.Club, 7, CardEffect.杀, Card.ElementType.Thunder));
            AddCard(new Card(139, Card.Suit.Diamond, 10, CardEffect.闪));
            AddCard(new Card(140, Card.Suit.Heart, 13, CardEffect.无懈可击));
            AddCard(new Card(141, Card.Suit.Heart, 4, CardEffect.杀, Card.ElementType.Fire));
            AddCard(new Card(142, Card.Suit.Diamond, 5, CardEffect.杀, Card.ElementType.Fire));
            AddCard(new Card(143, Card.Suit.Spade, 1, CardEffect.古锭刀));
            AddCard(new Card(144, Card.Suit.Diamond, 1, CardEffect.朱雀羽扇));
            AddCard(new Card(145, Card.Suit.Club, 11, CardEffect.铁索连环));
            AddCard(new Card(146, Card.Suit.Heart, 3, CardEffect.火攻));
            AddCard(new Card(147, Card.Suit.Club, 3, CardEffect.酒));
            AddCard(new Card(148, Card.Suit.Spade, 11, CardEffect.铁索连环));
            AddCard(new Card(149, Card.Suit.Spade, 9, CardEffect.酒));
            AddCard(new Card(150, Card.Suit.Diamond, 9, CardEffect.酒));
            AddCard(new Card(151, Card.Suit.Club, 10, CardEffect.铁索连环));
            AddCard(new Card(152, Card.Suit.Club, 13, CardEffect.铁索连环));
            AddCard(new Card(153, Card.Suit.Diamond, 12, CardEffect.火攻));
            AddCard(new Card(154, Card.Suit.Spade, 12, CardEffect.火攻));
            AddCard(new Card(155, Card.Suit.Spade, 3, CardEffect.酒));
            AddCard(new Card(156, Card.Suit.Club, 12, CardEffect.铁索连环));
            AddCard(new Card(157, Card.Suit.Club, 9, CardEffect.酒));
            AddCard(new Card(158, Card.Suit.Heart, 2, CardEffect.火攻));
            AddCard(new Card(159, Card.Suit.Club, 4, CardEffect.兵粮寸断));
            AddCard(new Card(160, Card.Suit.Spade, 10, CardEffect.兵粮寸断));
            TrashCardsHeap = new Queue<Card>(TrashCardsHeap.Distinct().ToArray());
            TotalCards = TrashCardsHeap.Count;
        }
        
        private Queue<Card> TakingCardsHeap = new(); //拿牌堆
        private Queue<Card> TrashCardsHeap = new(); //弃牌堆

        public int TakingCardCount
        {
            get{
                return TakingCardsHeap.Count;
            }
        }

        public int TrashCardCount
        {
            get{
                return TrashCardsHeap.Count;
            }
        }
    }
}
