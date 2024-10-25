using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CGuoJia: ChiefBase
    {
        public CGuoJia(): base("郭嘉", Camp.魏, GenderType.Male, 3)
        {
            Skills.Add(new STianDu());
            Skills.Add(new SYiJi());
        }
    }
}
