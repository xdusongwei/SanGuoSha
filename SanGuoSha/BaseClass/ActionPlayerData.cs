

namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 影响行动玩家的状态数据
    /// </summary>
    public class ActionPlayerData
    {
        /// <summary>
        /// 进入回合的玩家对象
        /// </summary>
        public volatile required PlayerBase CurrentPlayer;

        /// <summary>
        /// 指示该轮武将是否可以进入拿牌阶段
        /// </summary>
        public bool Take = true;

        /// <summary>
        /// 指示进入回合的武将是否能进入出牌阶段
        /// </summary>
        public bool Lead = true;

        /// <summary>
        /// 指示进入回合的武将是否能进入弃牌阶段
        /// </summary>
        public bool Abandonment = true;

        /// <summary>
        /// 指示进入回合的玩家能不能无限使用‘杀’
        /// </summary>
        public bool ShaNoLimit
        {
            get
            {
                return ShaNoLimitFlags.Count > 0;
            }
        }

        public HashSet<object> ShaNoLimitFlags = [];

        /// <summary>
        /// 将事件内的经常改动的数据复位操作
        /// </summary>
        public void Reset()
        {
            //EnableArmor = true;
            MaxShaTarget = 1;
        }
        /// <summary>
        /// 剩余可以杀的次数
        /// </summary>
        public int ShaRemain = 1;

        public int ShaTimes = 0;

        /// <summary>
        /// 进入回合的武将杀的目标数量最大值
        /// </summary>
        public int MaxShaTarget = 1;

        /// <summary>
        /// 进入回合的武将在拿牌阶段可以拿的牌数量
        /// </summary>
        public int TakeCardsCount = 2;

        /// <summary>
        /// 当前武将的执行阶段
        /// </summary>
        public volatile PlayerStageEnum PlayerStage = PlayerStageEnum.TurnStart;
    }
}
