using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CDaQiao: ChiefBase
    {
        public CDaQiao(): base("大乔", Camp.吴, GenderType.Female, 3)
        {
            Skills.Add(new SLiuLi());
            Skills.Add(new SGuoSe());
        }
    }
}
