using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CZhuGeLiang: ChiefBase
    {
        public CZhuGeLiang(): base("诸葛亮", Camp.蜀, GenderType.Male, 3)
        {
            Skills.Add(new SGuanXing());
            Skills.Add(new SKongCheng());
        }
    }
}
