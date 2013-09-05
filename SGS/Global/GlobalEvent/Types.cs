using SGS.ServerCore.Contest.Data;
using SGS.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;

namespace SGS.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        /// <summary>
        /// 牌的来源区域种类枚举
        /// </summary>
        public enum CardFrom
        {
            /// <summary>
            /// 不在玩家区域中,不会被系统移除
            /// </summary>
            None,
            /// <summary>
            /// 玩家的手牌中
            /// </summary>
            Hand,
            /// <summary>
            /// 玩家手牌,装备区和判定区
            /// </summary>
            HandAndEquipageAndJudgement,
            /// <summary>
            /// 玩家的装备区
            /// </summary>
            Equipage,
            /// <summary>
            /// 玩家的手牌及装备区
            /// </summary>
            HandAndEquipage,
            /// <summary>
            /// 最终执行的判定牌，意为触发相关武将的回收判定牌技能
            /// </summary>
            JudgementCard,
            /// <summary>
            /// 牌槽,注意牌槽的牌不会被该方法从中删除
            /// </summary>
            Slot
        };

        /// <summary>
        /// 子事件的节点
        /// </summary>
        internal struct EventRecoard
        {
            /// <summary>
            /// 事件的源,多指"出牌者"
            /// </summary>
            public ChiefBase Source;
            /// <summary>
            /// 事件的目标
            /// </summary>
            public ChiefBase Target;
            /// <summary>
            /// 事件的第二个目标
            /// </summary>
            public ChiefBase Target2;
            //事件使用的牌
            public Card[] Cards;
            //事件产生的效果
            public Card.Effect Effect;

            public string SkillName;

            /// <summary>
            /// 创建事件节点
            /// </summary>
            /// <param name="aSource">事件源</param>
            /// <param name="aTarget">事件目标</param>
            /// <param name="aTarget2">事件的第二个目标</param>
            /// <param name="aCards">源出的牌</param>
            /// <param name="aEffect">事件的效果</param>
            /// <param name="aSkillName">发动的技能名称</param>
            public EventRecoard(ChiefBase aSource, ChiefBase aTarget, ChiefBase aTarget2, Card[] aCards, Card.Effect aEffect, string aSkillName)
            {
                Source = aSource;
                Target = aTarget;
                Cards = aCards;
                Effect = aEffect;
                Target2 = aTarget2;
                SkillName = aSkillName;
            }

            /// <summary>
            /// 创建事件的节点
            /// </summary>
            /// <param name="aSource">事件源</param>
            /// <param name="aTarget">事件目标</param>
            /// <param name="aCards">源出的牌</param>
            /// <param name="aEffect">事件的效果</param>
            /// <param name="aSkillName">所发动的技能名称</param>
            public EventRecoard(ChiefBase aSource, ChiefBase aTarget, Card[] aCards, Card.Effect aEffect, string aSkillName)
                : this(aSource, aTarget, null, aCards, aEffect, aSkillName)
            {

            }
        }

    }
}
