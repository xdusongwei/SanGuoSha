

namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 武将的定义
    /// </summary>
    public abstract class ChiefBase
    {
        /// <summary>
        /// 构造武将
        /// </summary>
        /// <param name="aChiefName">武将的名称</param>
        /// <param name="aCamp">武将所属势力</param>
        /// <param name="aSex">武将性别</param>
        /// <param name="aHealth">武将默认的最大血量</param>
        public ChiefBase(string aChiefName, Camp aCamp, GenderType aGender, sbyte aHealth)
        {
            ChiefName = aChiefName;
            ChiefCamp = aCamp;
            Gender = aGender;
            MaxHealth = aHealth;
        }

        /// <summary>
        /// 武将势力的枚举
        /// </summary>
        public enum Camp { 
            /// <summary>
            /// 魏
            /// </summary>
            魏, 
            /// <summary>
            /// 蜀
            /// </summary>
            蜀, 
            /// <summary>
            /// 吴
            /// </summary>
            吴, 
            /// <summary>
            /// 群
            /// </summary>
            群, 
            /// <summary>
            /// 神
            /// </summary>
            神,
        };
        /// <summary>
        /// 武将性别的枚举
        /// </summary>
        public enum GenderType { 
            /// <summary>
            /// 男性
            /// </summary>
            Male, 
            /// <summary>
            /// 女性
            /// </summary>
            Female, 
        };
        
        /// <summary>
        /// 武将的势力
        /// </summary>
        public Camp ChiefCamp
        {
            get;
            set;
        }
        /// <summary>
        /// 武将的名称
        /// </summary>
        public readonly string ChiefName;
        /// <summary>
        /// 武将的性别
        /// </summary>
        public GenderType Gender;

        /// <summary>
        /// 武将的技能列表
        /// </summary>
        public List<SkillBase> Skills
        {
            get;
            set;
        } = [];

        /// <summary>
        /// 武将的默认最大血量
        /// </summary>
        public sbyte MaxHealth
        {
            get;
            private set;
        }

        public class BlankChief: ChiefBase
        {
            public BlankChief()
            : base("白板", Camp.神, GenderType.Male, 3)
            {

            }
        }
    }
}
