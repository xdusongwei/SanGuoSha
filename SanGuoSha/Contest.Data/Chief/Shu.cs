using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SanGuoSha.Contest.Data
{
    /// <summary>
    /// 刘备
    /// </summary>
    public class LiuBei : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public LiuBei(Players aPlayers)
            : base("刘备", Camp.Shu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillRenDe());
            Skills.Add(new SkillJiJiang());
        }
    }

    /// <summary>
    /// 关羽
    /// </summary>
    public class GuanYu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public GuanYu(Players aPlayers)
            : base("关羽", Camp.Shu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillWuSheng());
        }
    }

    /// <summary>
    /// 张飞
    /// </summary>
    public class ZhangFei : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhangFei(Players aPlayers)
            : base("张飞", Camp.Shu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillPaoXiao());
        }
    }

    /// <summary>
    /// 诸葛亮
    /// </summary>
    public class ZhuGeLiang : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhuGeLiang(Players aPlayers)
            : base("诸葛亮", Camp.Shu, SexType.Male, aPlayers, 3)
        {
            Skills.Add(new SkillKongCheng());
            Skills.Add(new SkillGuanXing());
        }
    }

    /// <summary>
    /// 赵云
    /// </summary>
    public class ZhaoYun : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhaoYun(Players aPlayers)
            : base("赵云", Camp.Shu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillLongTeng());
        }
    }

    /// <summary>
    /// 马超
    /// </summary>
    public class MaChao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public MaChao(Players aPlayers)
            : base("马超", Camp.Shu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillMaShu());
            Skills.Add(new SkillTieQi());
        }
    }

    /// <summary>
    /// 黄月英
    /// </summary>
    public class HuangYueYing : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public HuangYueYing(Players aPlayers)
            : base("黄月英", Camp.Shu, SexType.Female, aPlayers, 3)
        {
            Skills.Add(new SkillJiZhi());
            Skills.Add(new SkillQiCai());
        }
    }
}
