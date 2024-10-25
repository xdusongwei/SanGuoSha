

namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 牌槽类
    /// </summary>
    /// <remarks>
    /// 牌槽的构造函数
    /// </remarks>
    /// <param name="aSlotName">牌槽的名称</param>
    /// <param name="aCommon">指示XML消息产生方法,决定牌槽的内容是否所有玩家可见</param>
    /// <param name="aRecyclable">设置该牌槽是否在每次事件结束时回收其中的牌</param>
    public class CardSlot(string aSlotName, bool aIsCommon, bool aRecyclable)
    {
        /// <summary>
        /// 指示XML消息产生方法,决定牌槽的内容是否所有玩家可见
        /// 若设置false,公共事件链和非该玩家的事件链中的首元素中不包含这个牌槽的内容描述
        /// </summary>
        internal bool IsCommon
        {
            get;
            set;
        } = aIsCommon;

        /// <summary>
        /// 牌槽的内容
        /// </summary>
        public List<Card> Cards
        {
            get;
            private set;
        } = [];

        public List<Tuple<PlayerBase, Card>> PlayerChoice = [];

        public void Reset()
        {
            Cards.Clear();
            PlayerChoice.Clear();
        }

        public bool CardInSlotAndNoOneChoose(Card aCard)
        {
            if(!Cards.Contains(aCard)) return false;
            if(PlayerChoice.Any(i => i.Item2 == aCard)) return false;
            return true;
        }

        public bool MarkPlayerChoose(PlayerBase aPlayer, Card aCard)
        {
            if(!CardInSlotAndNoOneChoose(aCard)) return false;
            PlayerChoice.Add(new Tuple<PlayerBase, Card>(aPlayer, aCard));
            return true;
        }

        /// <summary>
        /// 牌槽的名称
        /// </summary>
        public string SlotName
        {
            get;
            set;
        } = aSlotName;

        /// <summary>
        /// 获取或者设置该牌槽是否在每次事件结束时回收其中的牌
        /// </summary>
        internal bool Recyclable
        {
            set;
            get;
        } = aRecyclable;

        /// <summary>
        /// 向牌槽添加牌对象
        /// </summary>
        /// <param name="aCards">牌数组</param>
        internal void AddCards(Card[] aCards)
        {
            Cards.AddRange(aCards);
            Cards = Cards.Distinct().ToList();
        }
    }
}
