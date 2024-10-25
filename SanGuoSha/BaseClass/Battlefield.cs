

using SanGuoSha.Battlefield;

namespace SanGuoSha.BaseClass
{
    public abstract class BattlefieldBase{
        public abstract CardCollector NewCollector();

        /// <summary>
        /// 让玩家从牌堆拿n张牌,该方法会自动将牌放置到玩家的手牌中(n=0方法无作为)
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="n">拿牌的数量</param>
        /// <returns>牌的数组</returns>
        public abstract Card[] TakingCards(PlayerBase aPlayer, int n);

        /// <summary>
        /// 从牌堆取出一张牌作为判定牌
        /// </summary>
        /// <param name="aPlayer">判定的玩家对象</param>
        /// <returns>一张判定牌</returns>
        public abstract Card PopSentenceCard(PlayerBase aPlayer);

        /// <summary>
        /// 移除玩家的牌,即包含手牌,装备区和判定区,如果不能全部移除将不会改变玩家的牌,但是所执行的事件不能挽回
        /// </summary>
        /// <param name="aPlayer">玩家</param>
        /// <param name="aCards">需要移除的牌</param>
        /// <returns>移除正常返回true</returns>
        public abstract bool RemoveCard(PlayerBase aPlayer, Card[] aCards);

        /// <summary>
        /// 一个用于武将之间移动牌的方法,默认将牌送至收到牌的武将的手牌中
        /// </summary>
        /// <param name="aFrom">给出牌的武将</param>
        /// <param name="afTo">收到牌的武将</param>
        /// <param name="aCards">牌数组</param>
        public abstract bool Move(PlayerBase aFrom, PlayerBase afTo, Card[] aCards);

        internal abstract void RegainHealth(PlayerBase aPlayer, sbyte aRegain);

        /// <summary>
        /// 要求武将消耗体力值
        /// </summary>
        /// <param name="aPlayer">受伤害的武将</param>
        /// <param name="aDamage">伤害量</param>
        /// <param name="aSource">伤害来源,非玩家操作置null</param>
        /// <param name="aSourceEvent">伤害来源事件</param>
        internal abstract void DamageHealth(PlayerBase aPlayer, sbyte aDamage, PlayerBase? aSource, EventRecord aSourceEvent, Card.ElementType aElement = Card.ElementType.None);

        /// <summary>
        /// 无懈可击子事件
        /// </summary>
        /// <param name="aTarget">需要无懈可击的目标</param>
        /// <param name="aEffect">需要被无懈可击的效果</param>
        /// <returns>true表示无懈可击成立,反之不成立</returns>
        internal abstract bool WuXieProc(PlayerBase aTarget, CardEffect aEffect, EventRecord aEvent);

        internal abstract bool WuXieProc(PlayerBase aTarget, CardEffect aEffect);


        public abstract AskAnswer NewAsk();

        private ActionPlayerData? _actionPlayerData;
        private int _answerCooldown = 0;

        public ActionPlayerData ActionPlayerData{
            get{
                return _actionPlayerData!;
            }

            set{
                _actionPlayerData = value;
            }
        }

        public PlayersBase Players = new Players();

        public int AnswerTimeout
        {
            get;
            set;
        } = 5000;
        
        public int AnswerCooldown
        {
            get
            {
                return _answerCooldown;
            }
            set
            {
                _answerCooldown = int.Max(0, int.Min(10_000, value));
            }
        }

        /// <summary>
        /// 五谷丰登牌槽名称
        /// </summary>
        public static readonly string WGFDSlotName = "五谷丰登";

        public abstract void NewEventNode(EventRecord aEvent);

        public abstract bool CheckLeadingAnswer(Type aType, AskForResult aAnswer);

        public abstract void CreateActionNode(ActionNode aNode);

        public abstract void UpdateSnapshot(AskAnswer? aAsk = null);

        public GameMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏事件所需的牌槽容器
        /// </summary>
        public abstract CardSlotContainer Slots
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置弃牌堆
        /// </summary>
        public abstract CardHeap CardsHeap
        {
            get;
            set;
        }

        public readonly List<ActionNode> ActionLog = [];

        internal readonly Dictionary<CardEffect, Type> AggressiveCards = [];

        protected readonly Dictionary<CardEffect, Type> TrialProcs = [];

        protected readonly Dictionary<AskForEnum, Type> AnswerCheck = [];

        protected readonly Dictionary<string, Type> AggressiveSkillCheck = [];

        protected readonly Dictionary<string, Type> TransformSkillCheck = [];

        protected readonly Random Random = new();

        /// <summary>
        /// 一个对列表对象进行乱序排列的方法
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="aList">列表对象</param>
        /// <returns>乱序列表</returns>
        internal List<T> ShuffleList<T>(List<T> aList)
        {
            var arr = aList.ToArray();
            ShuffleArray(arr);
            return [.. arr];
        }

        internal T[] ShuffleArray<T>(T[] aArray)
        {
            Random.Shuffle(aArray);
            return aArray;
        }

        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <param name="mod">模</param>
        /// <returns>[ 0 , mod ) 内任意整数</returns>
        internal int GetRandom(int mod)
        {
            return Random.Next(mod);
        }
    }
}
