using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SanGuoSha.Contest.Data
{
    /// <summary>
    /// 华佗
    /// </summary>
    public class HuaTuo : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public HuaTuo(Players aPlayers)
            : base("华佗", Camp.Qun, SexType.Male, aPlayers, 3)
        {
            Skills.Add(new SkillQingNang());
            Skills.Add(new SkillJiJiu());
        }
    }

    /// <summary>
    /// 吕布
    /// </summary>
    public class LvBu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public LvBu(Players aPlayers)
            : base("吕布", Camp.Qun, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillWuShuang());
        }
    }

    /// <summary>
    /// 貂蝉
    /// </summary>
    public class DiaoChan : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public DiaoChan(Players aPlayers)
            : base("貂蝉", Camp.Qun, SexType.Female, aPlayers, 3)
        {
            Skills.Add(new SkillBiYue());
            Skills.Add(new SkillLiJian());
        }
    }
}
