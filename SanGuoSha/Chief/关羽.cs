using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CGuanYu: ChiefBase
    {
        public CGuanYu(): base("关羽", Camp.蜀, GenderType.Male, 4)
        {
            Skills.Add(new SWuSheng());
        }
    }
}
