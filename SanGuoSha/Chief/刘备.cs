using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CLiuBei: ChiefBase
    {
        public CLiuBei(): base("刘备", Camp.蜀, GenderType.Male, 4)
        {
            Skills.Add(new SRenDe());
            Skills.Add(new SJiJiang());
        }
    }
}
