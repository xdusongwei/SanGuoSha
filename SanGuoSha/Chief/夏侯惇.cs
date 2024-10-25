using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CXiaHouDun: ChiefBase
    {
        public CXiaHouDun(): base("夏侯惇", Camp.魏, GenderType.Male, 4)
        {
            Skills.Add(new SGangLie());
        }
    }
}
