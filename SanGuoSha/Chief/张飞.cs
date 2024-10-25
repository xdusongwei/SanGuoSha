using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CZhangFei: ChiefBase
    {
        public CZhangFei(): base("张飞", Camp.蜀, GenderType.Male, 4)
        {
            Skills.Add(new SPaoXiao());
        }
    }
}
