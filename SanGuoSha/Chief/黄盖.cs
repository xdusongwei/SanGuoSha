using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CHuangGai: ChiefBase
    {
        public CHuangGai(): base("黄盖", Camp.吴, GenderType.Male, 4)
        {
            Skills.Add(new SKuRou());
        }
    }
}
