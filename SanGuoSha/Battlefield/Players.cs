using System.Runtime.CompilerServices;
using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.Battlefield
{
    /// <summary>
    /// 玩家集合
    /// </summary>
    public class Players: PlayersBase
    {
        private volatile List<PlayerBase> PlayerList = [];

        /// <summary>
        /// 增加一个玩家
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        public override void AddPlayer(PlayerBase aPlayer)
        {
            var newList = PlayerList.ToList();
            newList.Add(aPlayer);
            PlayerList = newList.Distinct().ToList();
        }

        public override PlayerBase? NextPlayer(PlayerBase aPlayer)
        {
            int i = 0;
            foreach (var p in PlayerList)
            {
                if (p == aPlayer)
                {
                    int j = PlayerList.Count;
                    while (j > 0)
                    {
                        if (!PlayerList[i = (++i) % PlayerList.Count].Dead)
                            return PlayerList[i];
                        j--;
                    }
                    return null;
                }
                i++;
            }
            return null;
        }

        /// <summary>
        /// 获取存活的玩家数量
        /// </summary>
        public override int AliveCount
        {
            get
            {
                return PlayerList.Where(i => !i.Dead).Count();
            }
        }

        public override byte Distance(PlayerBase A, PlayerBase B)
        {
            if (A == null || B == null) return byte.MaxValue;
            if (A.Dead || B.Dead || A == B) return 0;
            var players = PlayerList;
            var pa = Convert.ToByte(players.IndexOf(A));
            var pb = Convert.ToByte(players.IndexOf(B));
            
            byte t1 = 0;
            byte t2 = 0;
            var p = pa;
            while (p != pb && t1 < byte.MaxValue)
            {
                if(!players[p].Dead)
                    t1++;
                p = (byte)((p + 1) % players.Count);
            }
            p = pb;
            while (p != pa && t2 < byte.MaxValue)
            {
                if (!players[p].Dead)
                    t2++;
                p = (byte)((p + 1) % players.Count);
            }
            if (B.Jia1Ma != null)
            {
                t1++; t2++;
            }
            return Math.Min(t1, t2);
        }

        internal override bool WithinKitRange(PlayerBase aSource, PlayerBase aTarget, BattlefieldBase aBattlefield)
        {
            byte dis = Distance(aTarget, aSource);
            return CalcKitDistance(aSource, aBattlefield) >= dis;
        }

        internal override bool WithinShaRange(PlayerBase aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, bool aWithWeapon = true)
        {
            var dis = Distance(aSource, aTarget);
            return CalcShaDistance(aSource, aBattlefield, aWithWeapon) >= dis;
        }

        public override byte CalcKitDistance(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            byte ret = 1;
            if (aPlayer.Jian1Ma != null)
                ret++;
            foreach (var s in aPlayer.Skills)
                ret = s.CalcKitDistance(aPlayer, ret, aBattlefield);
            return ret;
        }

        public override byte CalcShaDistance(PlayerBase aPlayer, BattlefieldBase aBattlefield, bool aWithWeapon = true)
        {
            byte ret = 1;
            if (aWithWeapon && aPlayer.WeaponEffect != CardEffect.None)
                ret = byte.Max(ret, Convert.ToByte(WeaponProc.WeaponRange(aPlayer.WeaponEffect)));
            if (aPlayer.Jian1Ma != null)
                ret++;
            foreach (var s in aPlayer.Skills)
                ret = s.CalcShaDistance(aPlayer, ret, aBattlefield);
            return ret;
        }

        public override IEnumerator<PlayerBase> GetEnumerator()
        {
            return PlayerList.GetEnumerator();
        }

        public override bool NextAliveUntilNullOrStop(ref PlayerBase aPlayer, PlayerBase aStop)
        {
            if(aPlayer == null || aStop == null) throw new Exception("循环遍历玩家的参数不正确");
            if(aPlayer.Dead || !PlayerList.Contains(aPlayer)) return false;
            if(aStop.Dead || !PlayerList.Contains(aStop)) return false;
            var nextPlayer = NextPlayer(aPlayer);
            if(nextPlayer == null) return false;
            if(nextPlayer != aStop)
            {
                aPlayer = nextPlayer!;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool NextAliveUntilNull(ref PlayerBase aPlayer)
        {
            if(aPlayer == null) throw new Exception("循环遍历玩家的参数不正确");
            if(aPlayer.Dead || !PlayerList.Contains(aPlayer)) return false;
            var nextPlayer = NextPlayer(aPlayer);
            if(nextPlayer == null) return false;
            aPlayer = nextPlayer!;
            return true;
        }
    }
}
