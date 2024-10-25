using System.Collections;


namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 武将的定义
    /// </summary>
    public abstract class PlayersBase: IEnumerable<PlayerBase>
    {   
        public abstract int AliveCount
        {
            get;
        }

        public abstract void AddPlayer(PlayerBase aPlayer);

        /// <summary>
        /// 查找下一位活着的玩家
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <returns>玩家对象</returns>
        public abstract PlayerBase? NextPlayer(PlayerBase aPlayer);

        /// <summary>
        /// 获得A玩家到B玩家的距离，不包括-1马计算,但包括B武将的+1马计算
        /// </summary>
        /// <param name="A">起点玩家</param>
        /// <param name="B">目的玩家</param>
        /// <returns>距离</returns>
        public abstract byte Distance(PlayerBase A, PlayerBase B);

        public abstract byte CalcShaDistance(PlayerBase aPlayer, BattlefieldBase aBattlefield, bool aWithWeapon = true);

        internal abstract bool WithinKitRange(PlayerBase aSource, PlayerBase aTarget, BattlefieldBase aBattlefield);

        internal abstract bool WithinShaRange(PlayerBase aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, bool aWithWeapon = true);

        public abstract byte CalcKitDistance(PlayerBase aPlayer, BattlefieldBase aBattlefield);

        public abstract IEnumerator<PlayerBase> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract bool NextAliveUntilNullOrStop(ref PlayerBase aPlayer, PlayerBase aStop);

        public abstract bool NextAliveUntilNull(ref PlayerBase aPlayer);
    }
}
