using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CSunQuan: ChiefBase
    {
        public CSunQuan(): base("孙权", Camp.吴, GenderType.Male, 4)
        {
            Skills.Add(new SZhiHeng());
            Skills.Add(new SJiuYuan());
        }
    }
}
