using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CZhaoYun: ChiefBase
    {
        public CZhaoYun(): base("赵云", Camp.蜀, GenderType.Male, 4)
        {
            Skills.Add(new SLongDan());
        }
    }
}
