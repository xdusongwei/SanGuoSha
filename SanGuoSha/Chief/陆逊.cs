using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CLuXun: ChiefBase
    {
        public CLuXun(): base("陆逊", Camp.吴, GenderType.Male, 3)
        {
            Skills.Add(new SQianXun());
            Skills.Add(new SLianYing());
        }
    }
}
