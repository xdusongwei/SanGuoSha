using System.Runtime.InteropServices;


namespace SanGuoSha.BaseClass
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
    /// <param name="aCustomName">自定义名称</param>
    public class Card(int aID, Card.Suit aSuit, int aIndex, CardEffect aEffect, Card.ElementType aElement, string aCustomName): IComparable
    {

        /// <summary>
        /// 创建一张没有属性的牌
        /// </summary>
        /// <param name="aID">牌的编号</param>
        /// <param name="aSuit">花色</param>
        /// <param name="aIndex">牌号</param>
        /// <param name="aEffect">基本效果</param>
        public Card(int aID, Suit aSuit, int aIndex, CardEffect aEffect)
            :this(aID, aSuit, aIndex, aEffect, ElementType.None, string.Empty)
        {

        }

        public Card(int aID, Suit aSuit, int aIndex, CardEffect aEffect, ElementType aElement)
            :this(aID, aSuit, aIndex, aEffect, aElement, string.Empty)
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
            /// 梅花
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

        public enum CardColor { 
            Unknown,
            Black,
            Red,
        }

        /// <summary>
        /// 这张牌的编号
        /// </summary>
        public readonly int ID = aID >= -4 ? aID : throw new Exception($"生成牌ID错误: {aID}");

        /// <summary>
        /// 这张牌的花色
        /// </summary>
        public Suit CardSuit = aSuit;

        public CardColor Color
        {
            get
            {
                return CardSuit == Suit.Spade || CardSuit == Suit.Club ? CardColor.Black : CardColor.Red;
            }
        }

        /// <summary>
        /// 这张牌的牌号，只读
        /// </summary>
        public readonly int CardIndex = aIndex;
        /// <summary>
        /// 牌的效果
        /// </summary>
        public CardEffect CardEffect = aEffect;
        /// <summary>
        /// 需要指定名称的牌, 比如各种马
        /// </summary>
        public readonly string CustomName = aCustomName;
        /// <summary>
        /// 是否是非延时锦囊
        /// </summary>
        public static bool IsKit(CardEffect aEffect)
        {
            switch(aEffect)
            {
                case CardEffect.无懈可击:
                case CardEffect.决斗: 
                case CardEffect.南蛮入侵:
                case CardEffect.万箭齐发:
                case CardEffect.桃园结义:
                case CardEffect.无中生有:
                case CardEffect.过河拆桥:
                case CardEffect.顺手牵羊:
                case CardEffect.借刀杀人:
                case CardEffect.五谷丰登:
                case CardEffect.火攻:
                case CardEffect.铁索连环:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 是否延时锦囊
        /// </summary>
        public static bool IsDelayKit(CardEffect aEffect)
        {
            return aEffect switch
            {
                CardEffect.闪电 or CardEffect.乐不思蜀 or CardEffect.兵粮寸断 => true,
                _ => false,
            };
        }


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
        /// 获得这张牌的原效果副本（清除上面的多余特殊效果）
        /// </summary>
        /// <returns></returns>
        public Card GetOriginalCard()
        {
            return new Card(ID, CardSuit, CardIndex, CardEffect, Element, CustomName);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Card);
        }

        public bool Equals(Card? other)
        {
            if (other == null)
                return false;

            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(Card? left, Card? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Card? left, Card? right)
        {
            return !(left == right);
        }

        public static Suit[] Suits
        {
            get;
        } = [Suit.Spade, Suit.Club, Suit.Heart, Suit.Diamond, ];

        public static readonly Dictionary<CardEffect, sbyte> Weapons = new([
            new KeyValuePair<CardEffect, sbyte>(CardEffect.丈八蛇矛, 3),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.贯石斧, 3),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.青龙偃月刀, 3),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.雌雄双股剑, 2),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.青钢剑, 2),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.朱雀羽扇, 4),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.方天画戟, 4),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.诸葛连弩, 1),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.麒麟弓, 5),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.古锭刀, 2),
            new KeyValuePair<CardEffect, sbyte>(CardEffect.寒冰箭, 2),
        ]);

        public static readonly CardEffect[] Armors = [
            CardEffect.八卦阵,
            CardEffect.藤甲,
            CardEffect.仁王盾,
            CardEffect.白银狮子,
        ];

        public override string ToString()
        {
            var padding = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                padding = " ";
            }
            var suitEmoji = "?";
            if(CardSuit == Suit.Spade)
            {
                suitEmoji = "♠️";
            }
            if(CardSuit == Suit.Club)
            {
                suitEmoji = "♣️";
            }
            if(CardSuit == Suit.Heart)
            {
                suitEmoji = "♥️";
            }
            if(CardSuit == Suit.Diamond)
            {
                suitEmoji = "♦️";
            }
            return $"{suitEmoji}{padding}{CardIndex}:{CardEffect}";
        }

        public int CompareTo(object? obj)
        {
            return ID.CompareTo((obj as Card)!.ID);
        }
    }
}
