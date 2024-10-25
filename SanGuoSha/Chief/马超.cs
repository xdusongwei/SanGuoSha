using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CMaChao: ChiefBase
    {
        public CMaChao(): base("马超", Camp.蜀, GenderType.Male, 4)
        {
            Skills.Add(new SMaShu());
            Skills.Add(new STieQi());
        }
    }
}
