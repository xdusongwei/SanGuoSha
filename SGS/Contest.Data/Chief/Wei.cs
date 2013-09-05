using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 曹操
    /// </summary>
    public class CaoCao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public CaoCao(Players aPlayers)
            : base("曹操", Camp.Wei, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillJianXiong());
            Skills.Add(new SkillHuJia());
        }
    }

    /// <summary>
    /// 夏侯惇
    /// </summary>
    public class XiaHouDun : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public XiaHouDun(Players aPlayers)
            : base("夏侯惇", Camp.Wei, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillGangLie());
        }
    }

    /// <summary>
    /// 甄姬
    /// </summary>
    public class ZhenJi : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhenJi(Players aPlayers)
            : base("甄姬", Camp.Wei, SexType.Female, aPlayers, 3)
        {
            Skills.Add(new SkillQingGuo());
            Skills.Add(new SkillLuoShen());
        }
    }

    /// <summary>
    /// 郭嘉
    /// </summary>
    public class GuoJia : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public GuoJia(Players aPlayers)
            : base("郭嘉", Camp.Wei, SexType.Male, aPlayers, 3)
        {
            Skills.Add(new SkillTianDu());
            Skills.Add(new SkillYiJi());
        }
    }

    /// <summary>
    /// 张辽
    /// </summary>
    public class ZhangLiao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhangLiao(Players aPlayers)
            : base("张辽", Camp.Wei, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillTuXi());
        }
    }

    /// <summary>
    /// 许褚
    /// </summary>
    public class XuChu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public XuChu(Players aPlayers)
            : base("许褚", Camp.Wei, SexType.Male, aPlayers, 4)
        {
            Skills.Add(new SkillLuoYi());
        }
    }

    /// <summary>
    /// 司马懿
    /// </summary>
    public class SiMaYi : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public SiMaYi(Players aPlayers)
            : base("司马懿", Camp.Wei, SexType.Male, aPlayers, 3)
        {
            Skills.Add(new SkillFanKui());
            Skills.Add(new SkillGuiCai());
        }
    }
}
