using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CLvMeng: ChiefBase
    {
        public CLvMeng(): base("吕蒙", Camp.吴, GenderType.Male, 4)
        {
            Skills.Add(new SKeJi());
        }
    }
}
