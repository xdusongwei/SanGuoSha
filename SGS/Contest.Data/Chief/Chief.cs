/*
 * 
 * Namespace SGS.ServerCore.Contest.Data
 * 各个武将的定义和武将堆
*/
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 武将堆,武将堆会按要求的方法产生一批武将对象
    /// </summary>
    internal class ChiefHeap
    {
        /// <summary>
        /// 随机数产生器
        /// </summary>
        private static RNGCryptoServiceProvider _random;
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="mod">模</param>
        /// <returns>模内的非负值</returns>
        private static int GetRandom(int mod)
        {
            if (_random == null) _random = new RNGCryptoServiceProvider();
            int max = mod;
            int rnd = int.MinValue;
            decimal _base = (decimal)long.MaxValue;
            byte[] rndSeries = new byte[8];
            _random.GetBytes(rndSeries);
            rnd = (int)(Math.Abs(BitConverter.ToInt64(rndSeries, 0)) / _base * (max));
            return rnd;
        }

        /// <summary>
        /// 武将列表乱序
        /// </summary>
        /// <param name="aList">源列表</param>
        /// <returns>乱序列表</returns>
        public static List<ChiefBase> ShuffleChiefsList(List<ChiefBase> aList)
        {
            List<ChiefBase> ret = new List<ChiefBase>();
            foreach (ChiefBase item in aList)
            {
                ret.Insert(GetRandom(ret.Count), item);
            }
            return ret;
        }

        /// <summary>
        /// 获取林包的武将
        /// </summary>
        /// <returns>一个包含全部林包武将的列表</returns>
        public static List<ChiefBase> GetLinPackChiefs()
        {
            List<ChiefBase> lst = new List<ChiefBase>();
            List<ChiefBase> ret = new List<ChiefBase>();
            lst.Add(new JiaXu(null));
            lst.Add(new DongZhuo(null));
            lst.Add(new LuSu(null));
            lst.Add(new SunJian(null));
            lst.Add(new ZhuRong(null));
            lst.Add(new MengHuo(null));
            lst.Add(new CaoPi(null));
            lst.Add(new XuHuang(null));
            foreach (ChiefBase item in lst)
            {
                ret.Insert(GetRandom(ret.Count), item);
            }
            return ret;
        }

        /// <summary>
        /// 获取火包的武将
        /// </summary>
        /// <returns>一个包含火包的武将列表</returns>
        public static List<ChiefBase> GetHuoPackChiefs()
        {
            List<ChiefBase> lst = new List<ChiefBase>();
            List<ChiefBase> ret = new List<ChiefBase>();
            lst.Add(new YuanShao(null));
            lst.Add(new PangDe(null));
            lst.Add(new YanLiangWenChou(null));
            lst.Add(new TaiShiCi(null));
            lst.Add(new PangTong(null));
            lst.Add(new WoLongZhuGe(null));
            lst.Add(new DianWei(null));
            lst.Add(new XunYu(null));
            foreach (ChiefBase item in lst)
            {
                ret.Insert(GetRandom(ret.Count), item);
            }
            return ret;
        }

        /// <summary>
        /// 获取风包的全部武将对象
        /// </summary>
        /// <returns>含有风包武将的列表</returns>
        public static List<ChiefBase> GetFengPackChiefs()
        {
            List<ChiefBase> lst = new List<ChiefBase>();
            List<ChiefBase> ret = new List<ChiefBase>();
            lst.Add(new CaoRen(null));
            lst.Add(new XiaHouYuan(null));
            lst.Add(new YuJi(null));
            lst.Add(new ZhangJiao(null));
            lst.Add(new WeiYan(null));
            lst.Add(new HuangZhong(null));
            lst.Add(new ZhouTai(null));
            lst.Add(new XiaoQiao(null));
            foreach (ChiefBase item in lst)
            {
                ret.Insert(GetRandom(ret.Count), item);
            }
            return ret;
        }

        /// <summary>
        /// 获取原版武将对象的列表
        /// </summary>
        /// <returns>一个包含原版武将的列表</returns>
        public static List<ChiefBase> GetOriginChiefs()
        {
            List<ChiefBase> lst = new List<ChiefBase>();
            List<ChiefBase> ret = new List<ChiefBase>();
            lst.Add(new CaoCao(null));
            lst.Add(new XiaHouDun(null));
            lst.Add(new SiMaYi(null));
            lst.Add(new ZhenJi(null));
            lst.Add(new HuangGai(null));
            lst.Add(new LvMeng(null));
            lst.Add(new GanNing(null));
            lst.Add(new ZhangFei(null));
            lst.Add(new ZhaoYun(null));
            lst.Add(new GuanYu(null));
            lst.Add(new XuChu(null));
            lst.Add(new LvBu(null));
            lst.Add(new ZhangLiao(null));
            lst.Add(new GuoJia(null));
            lst.Add(new LiuBei(null));
            lst.Add(new ZhuGeLiang(null));
            lst.Add(new MaChao(null));
            lst.Add(new HuangYueYing(null));
            lst.Add(new SunQuan(null));
            lst.Add(new ZhouYu(null));
            lst.Add(new DaQiao(null));
            lst.Add(new LuXun(null));
            lst.Add(new SunShangXiang(null));
            lst.Add(new HuaTuo(null));
            lst.Add(new DiaoChan(null));
            foreach (ChiefBase item in lst)
            {
                ret.Insert(GetRandom(ret.Count), item);
            }
            return ret;
        }
    }


    /*
     * 
     * 风包 武将
     * 
    */

    /// <summary>
    /// 夏侯渊
    /// </summary>
    public class XiaHouYuan : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public XiaHouYuan(Players aPlayers)
            : base("夏侯渊", Camp.Wei, SexType.Male, aPlayers, 4)
        {

        }
    }

    /// <summary>
    /// 曹仁
    /// </summary>
    public class CaoRen : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public CaoRen(Players aPlayers)
            : base("曹仁", Camp.Wei, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 小乔
    /// </summary>
    public class XiaoQiao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public XiaoQiao(Players aPlayers)
            : base("小乔", Camp.Wu, SexType.Female, 3)
        {

        }
    }

    /// <summary>
    /// 周泰
    /// </summary>
    public class ZhouTai : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhouTai(Players aPlayers)
            : base("周泰", Camp.Wu, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 黄忠
    /// </summary>
    public class HuangZhong : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public HuangZhong(Players aPlayers)
            : base("黄忠", Camp.Shu, SexType.Male, 4)
        {

        }

    }


    /// <summary>
    /// 魏延
    /// </summary>
    public class WeiYan : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public WeiYan(Players aPlayers)
            : base("魏延", Camp.Shu, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 张角
    /// </summary>
    public class ZhangJiao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhangJiao(Players aPlayers)
            : base("张角", Camp.Qun, SexType.Male, 3)
        {

        }
    }

    /// <summary>
    /// 于吉
    /// </summary>
    public class YuJi : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public YuJi(Players aPlayers)
            : base("于吉", Camp.Qun, SexType.Male, 3)
        {

        }
    }


    /*
     * 
     * 
     * 火包  武将 
     * 
    */
    /// <summary>
    /// 典韦
    /// </summary>
    public class DianWei : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public DianWei(Players aPlayers)
            : base("典韦", Camp.Wei, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 荀彧
    /// </summary>
    public class XunYu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public XunYu(Players aPlayers)
            : base("荀彧", Camp.Wei, SexType.Male, 3)
        {

        }
    }

    /// <summary>
    /// 卧龙诸葛
    /// </summary>
    public class WoLongZhuGe : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public WoLongZhuGe(Players aPlayers)
            : base("卧龙诸葛", Camp.Shu, SexType.Male, 3)
        {

        }
    }

    /// <summary>
    /// 庞统
    /// </summary>
    public class PangTong : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public PangTong(Players aPlayers)
            : base("庞统", Camp.Shu, SexType.Male, 3)
        {

        }
    }

    /// <summary>
    /// 太史慈
    /// </summary>
    public class TaiShiCi : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public TaiShiCi(Players aPlayers)
            : base("太史慈", Camp.Wu, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 颜良文丑
    /// </summary>
    public class YanLiangWenChou : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public YanLiangWenChou(Players aPlayers)
            : base("颜良&文丑", Camp.Qun, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 庞德
    /// </summary>
    public class PangDe : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public PangDe(Players aPlayers)
            : base("庞德", Camp.Qun, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 袁绍
    /// </summary>
    public class YuanShao : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public YuanShao(Players aPlayers)
            : base("袁绍", Camp.Qun, SexType.Male, 4)
        {

        }
    }

    /*
     * 
     * 林包  武将 
     * 
     * 
     * */
    /// <summary>
    /// 徐晃
    /// </summary>
    public class XuHuang : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public XuHuang(Players aPlayers)
            : base("徐晃", Camp.Wei, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 曹丕
    /// </summary>
    public class CaoPi : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public CaoPi(Players aPlayers)
            : base("曹丕", Camp.Wei, SexType.Male, 3)
        {

        }
    }

    /// <summary>
    /// 孟获
    /// </summary>
    public class MengHuo : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public MengHuo(Players aPlayers)
            : base("孟获", Camp.Shu, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 祝融
    /// </summary>
    public class ZhuRong : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public ZhuRong(Players aPlayers)
            : base("祝融", Camp.Shu, SexType.Female, 4)
        {

        }
    }

    /// <summary>
    /// 孙坚
    /// </summary>
    public class SunJian : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public SunJian(Players aPlayers)
            : base("孙坚", Camp.Wu, SexType.Male, 4)
        {

        }
    }

    /// <summary>
    /// 鲁肃
    /// </summary>
    public class LuSu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public LuSu(Players aPlayers)
            : base("鲁肃", Camp.Wu, SexType.Male, 3)
        {

        }
    }

    /// <summary>
    /// 董卓
    /// </summary>
    public class DongZhuo : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public DongZhuo(Players aPlayers)
            : base("董卓", Camp.Qun, SexType.Male, 8)
        {

        }
    }

    /// <summary>
    /// 贾诩
    /// </summary>
    public class JiaXu : ChiefBase
    {
        /// <summary>
        /// 武将构造函数
        /// </summary>
        /// <param name="aPlayers">玩家集合</param>
        public JiaXu(Players aPlayers)
            : base("贾诩", Camp.Qun, SexType.Male, 3)
        {

        }
    }

}
