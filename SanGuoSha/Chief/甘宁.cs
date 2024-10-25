using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CGanNing: ChiefBase
    {
        public CGanNing(): base("甘宁", Camp.吴, GenderType.Male, 4)
        {
            Skills.Add(new SQiXi());
        }
    }
}
