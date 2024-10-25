using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CDiaoChan: ChiefBase
    {
        public CDiaoChan(): base("貂蝉", Camp.群, GenderType.Female, 3)
        {
            Skills.Add(new SLiJian());
            Skills.Add(new SBiYue());
        }
    }
}
