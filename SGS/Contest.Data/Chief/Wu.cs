using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 孙权
    /// </summary>
    public class SunQuan : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public SunQuan(Players aPlayers)
            : base("孙权", Camp.Wu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillZhiHeng());
            Skills.Add(new SkillJiuYuan());
        }
    }

    /// <summary>
    /// 甘宁 
    /// </summary>
    public class GanNing : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public GanNing(Players aPlayers)
            : base("甘宁", Camp.Wu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillQiXi());
        }
    }

    /// <summary>
    /// 吕蒙 
    /// </summary>
    public class LvMeng : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public LvMeng(Players aPlayers)
            : base("吕蒙", Camp.Wu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillKeJi());
        }
    }

    /// <summary>
    /// 黄盖
    /// </summary>
    public class HuangGai : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public HuangGai(Players aPlayers)
            : base("黄盖", Camp.Wu, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillKuRou());
        }
    }

    /// <summary>
    /// 周瑜
    /// </summary>
    public class ZhouYu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhouYu(Players aPlayers)
            : base("周瑜", Camp.Wu, SexType.Male, aPlayers, 3)
        {
            Skills.Add(new SkillYingZi());
            Skills.Add(new SkillFanJian());
        }
    }

    /// <summary>
    /// 大乔
    /// </summary>
    public class DaQiao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public DaQiao(Players aPlayers)
            : base("大乔", Camp.Wu, SexType.Female, aPlayers, 3)
        {
            Skills.Add(new SkillGuoSe());
            Skills.Add(new SkillLiuLi());
        }
    }

    /// <summary>
    /// ;陆逊
    /// </summary>
    public class LuXun : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public LuXun(Players aPlayers)
            : base("陆逊", Camp.Wu, SexType.Male, aPlayers, 3)
        {
            Skills.Add(new SkillLianYing());
            Skills.Add(new SkillQianXun());
        }
    }

    /// <summary>
    /// 孙尚香
    /// </summary>
    public class SunShangXiang : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public SunShangXiang(Players aPlayers)
            : base("孙尚香", Camp.Wu, SexType.Female, aPlayers, 3)
        {
            Skills.Add(new SkillJieYin());
            Skills.Add(new SkillXiaoJi());
        }
    }
}
