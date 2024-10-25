using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CSunShangXiang: ChiefBase
    {
        public CSunShangXiang(): base("孙尚香", Camp.吴, GenderType.Female, 3)
        {
            Skills.Add(new SJieYin());
            Skills.Add(new SXiaoJi());
        }
    }
}
