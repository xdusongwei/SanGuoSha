/*
 * ChiefBase
 * Namespace SGS.ServerCore.Contest.Data
 * 武将的定义
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using BeaverMarkupLanguage;

namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 武将的定义
    /// </summary>
    public abstract class ChiefBase
    {
        /// <summary>
        /// 玩家集合
        /// </summary>
        public Players playersObject;

        /// <summary>
        /// 构造武将
        /// </summary>
        /// <param name="aChiefName">武将的名称</param>
        /// <param name="aCamp">武将所属势力</param>
        /// <param name="aSex">武将性别</param>
        /// <param name="aHealth">武将默认的最大血量</param>
        public ChiefBase(string aChiefName, Camp aCamp, SexType aSex, sbyte aHealth)
            : this(aChiefName, aCamp, aSex, null, aHealth)
        {
            
        }

        /// <summary>
        /// 构造武将.一般用于游戏测试
        /// </summary>
        /// <param name="aChiefName">武将的名称</param>
        /// <param name="aCamp">武将所属势力</param>
        /// <param name="aSex">武将性别</param>
        /// <param name="aPlayers">玩家集合</param>
        /// <param name="aHealth">武将默认的最大血量</param>
        public ChiefBase(string aChiefName , Camp aCamp , SexType aSex, Players aPlayers , sbyte aHealth)
        {
            ChiefName = aChiefName;
            ChiefCamp = aCamp;
            playersObject = aPlayers;
            Sex = aSex;
            Skills = new List<SkillBase>();
            Health = aHealth;
            ChiefStatus = Status.Insurgent;
            SlotsBuffer = new SlotContainer();
        }

        /// <summary>
        /// 武将势力的枚举
        /// </summary>
        public enum Camp { 
            /// <summary>
            /// 魏
            /// </summary>
            Wei, 
            /// <summary>
            /// 蜀
            /// </summary>
            Shu, 
            /// <summary>
            /// 吴
            /// </summary>
            Wu, 
            /// <summary>
            /// 群
            /// </summary>
            Qun, 
            /// <summary>
            /// 神
            /// </summary>
            Shen };
        /// <summary>
        /// 武将性别的枚举
        /// </summary>
        public enum SexType { 
            /// <summary>
            /// 男性
            /// </summary>
            Male, 
            /// <summary>
            /// 女性
            /// </summary>
            Female };
        /// <summary>
        /// 武将身份的枚举
        /// </summary>
        public enum Status { 
            /// <summary>
            /// 没有身份
            /// </summary>
            Unknown , 
            /// <summary>
            /// 主公
            /// </summary>
            Majesty, 
            /// <summary>
            /// 忠臣
            /// </summary>
            Loyalist, 
            /// <summary>
            /// 内奸
            /// </summary>
            Spy, 
            /// <summary>
            /// 反贼
            /// </summary>
            Insurgent };

        /// <summary>
        /// 武将的身份
        /// </summary>
        public Status ChiefStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 武将的势力
        /// </summary>
        public readonly Camp ChiefCamp;
        /// <summary>
        /// 武将的名称
        /// </summary>
        public readonly string ChiefName;
        /// <summary>
        /// 武将的性别
        /// </summary>
        public SexType Sex;

        /// <summary>
        /// 根据武将的名称判断两个武将对象是否相等
        /// </summary>
        /// <param name="aChief">另一个武将对象</param>
        /// <returns>若名称相等,返回true.参数为null返回false</returns>
        public bool IsMe(ChiefBase aChief )
        {
            if (aChief == null) return false;
            return aChief.ChiefName == ChiefName;
        }

        /// <summary>
        /// 获取逆时针顺序的下一位武将.这个属性可以使用的前提是:必须在构造函数中或者playersObject成员上设置好玩家集合对象,否则会产生null
        /// </summary>
        public ChiefBase Next
        {
            get
            {
                if (playersObject == null) return null;
                return playersObject.NextChief(this);
            }
        }

        /// <summary>
        /// 武将的技能列表
        /// </summary>
        internal List<SkillBase> Skills
        {
            get;
            set;
        }

        /// <summary>
        /// 将该武将的技能和相应的技能状态以消息的方式传送给所有玩家
        /// </summary>
        /// <param name="aData">游戏数据</param>
        public void ReportSkills(GlobalData aData)
        {
            Beaver root = new Beaver("skills", ChiefName);
            foreach (SkillBase s in Skills)
            {
                root.Add(string.Empty, new Beaver("skill", s.SkillName, s.SkillStatus.ToString()));
            }
            aData.Game.AsynchronousCore.SendMessage(root.ToString());
        }

        /// <summary>
        /// 武将的默认最大血量
        /// </summary>
        public sbyte Health
        {
            get;
            private set;
        }

        /// <summary>
        /// 武将的牌槽
        /// </summary>
        internal SlotContainer SlotsBuffer
        {
            get;
            private set;
        }

        /// <summary>
        /// 武将转玩家的方法
        /// </summary>
        /// <param name="aChiefs">武将</param>
        /// <param name="aPlayers">玩家集合</param>
        /// <returns>玩家组成的数组</returns>
        internal static Player[] Chiefs2Players(ChiefBase[] aChiefs , SGS.ServerCore.Contest.Data.Players aPlayers)
        {
            List<Player> lst = new List<Player>();
            if (aChiefs != null)
                foreach (ChiefBase c in aChiefs)
                {
                    Player p = aPlayers[c];
                    if (p != null)
                        lst.Add(p);
                }
            return lst.ToArray();
        }

        /// <summary>
        /// 武将名称转为XML节表述
        /// </summary>
        /// <param name="aNodeName">节名称</param>
        /// <param name="aChiefs">武将数组</param>
        /// <returns>一个XML节</returns>
        internal static XElement Chiefs2XML(string aNodeName, ChiefBase[] aChiefs)
        {
            XElement xchiefs = new XElement(aNodeName);
            foreach (ChiefBase c in aChiefs)
                xchiefs.Add(new XElement("chief", c.ChiefName));
            return xchiefs;
        }

        internal static BeaverMarkupLanguage.Beaver Chiefs2Beaver(string aNodeName, ChiefBase[] aChiefs)
        {
            Beaver ret = new Beaver();
            foreach (ChiefBase c in aChiefs)
                ret.Add(string.Empty, c.ChiefName);
            ret.SetHeaderElementName(aNodeName);
            return ret;
            //return new BeaverMarkupLanguage.Beaver(aNodeName, aChiefs.Select(i => i.ChiefName.ToString()));
        }
    }
}
