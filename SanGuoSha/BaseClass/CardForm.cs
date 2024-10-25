

namespace SanGuoSha.BaseClass
{
        /// <summary>
        /// 牌的来源区域种类枚举
        /// </summary>
        public enum CardFrom
        {
            /// <summary>
            /// 不在玩家区域中, 不会被系统移除
            /// </summary>
            None,
            /// <summary>
            /// 玩家的手牌中
            /// </summary>
            Hand,
            /// <summary>
            /// 玩家手牌, 装备区和判定区
            /// </summary>
            HandAndEquipageAndTrial,
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
            Sentence,
            /// <summary>
            /// 牌槽, 注意牌槽的牌不会被该方法从中删除
            /// </summary>
            Slot,
        };
}
