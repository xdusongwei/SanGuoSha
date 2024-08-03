/*
 * Namespace SanGuoSha.ServerCore.Contest.Data
 * 牌槽与牌槽容器 
 * 牌槽是一个可以包含一组牌的容器
 * 用来容纳手牌\判定区的牌\武将某些技能用来放置牌\系统过程中特殊事件的牌
 * 而牌槽容器则是将一些牌槽进行管理的方式
*/
using System.Collections.Generic;
using System.Linq;

namespace SanGuoSha.ServerCore.Contest.Data
{
    /// <summary>
    /// 牌槽容器类
    /// </summary>
    internal class SlotContainer
    {
        internal List<Slot> Slots
        {
            get;
            private set;
        }

        public SlotContainer()
        {
            Slots = new List<Slot>();
        }

        internal Slot this[string aName]
        {
            get
            {
                return Slots.Find(c => c.SlotName == aName);
            }
        }
    }

    /// <summary>
    /// 牌槽类
    /// </summary>
    internal class Slot
    {
        /// <summary>
        /// 指示XML消息产生方法,决定牌槽的内容是否所有玩家可见
        /// 若设置false,公共事件链和非该玩家的事件链中的首元素中不包含这个牌槽的内容描述
        /// </summary>
        internal bool Common
        {
            get;
            set;
        }

        /// <summary>
        /// 牌槽的内容
        /// </summary>
        internal List<Card> Cards
        {
            get;
            private set;
        }

        /// <summary>
        /// 牌槽的名称
        /// </summary>
        internal string SlotName
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或者设置该牌槽是否在每次事件结束时回收其中的牌
        /// </summary>
        internal bool Recyclable
        {
            set;
            get;
        }

        /// <summary>
        /// 向牌槽添加牌对象
        /// </summary>
        /// <param name="aCards">牌数组</param>
        internal void AddCards(Card[] aCards)
        {
            Cards.AddRange(aCards);
            Cards.Remove(null);
            Cards = Cards.Distinct().ToList();
        }

        /// <summary>
        /// 牌槽的构造函数
        /// </summary>
        /// <param name="aSlotName">牌槽的名称</param>
        /// <param name="aCommon">指示XML消息产生方法,决定牌槽的内容是否所有玩家可见</param>
        /// <param name="aRecyclable">设置该牌槽是否在每次事件结束时回收其中的牌</param>
        public Slot(string aSlotName, bool aCommon, bool aRecyclable)
        {
            Cards = new List<Card>();
            SlotName = aSlotName;
            Common = false;
            Recyclable = aRecyclable;
        }
    }
}
