using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CLvBu: ChiefBase
    {
        public CLvBu(): base("吕布", Camp.群, GenderType.Male, 4)
        {
            Skills.Add(new SWuShuang());
        }
    }
}
