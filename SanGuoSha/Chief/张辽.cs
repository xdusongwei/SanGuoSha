using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CZhangLiao: ChiefBase
    {
        public CZhangLiao(): base("张辽", Camp.魏, GenderType.Male, 4)
        {
            Skills.Add(new STuXi());
        }
    }
}
