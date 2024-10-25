using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CZhouYu: ChiefBase
    {
        public CZhouYu(): base("周瑜", Camp.吴, GenderType.Male, 3)
        {
            Skills.Add(new SYingZi());
            Skills.Add(new SFanJian());
        }
    }
}
