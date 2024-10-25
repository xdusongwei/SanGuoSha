using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CSiMaYi: ChiefBase
    {
        public CSiMaYi(): base("司马懿", Camp.魏, GenderType.Male, 3)
        {
            Skills.Add(new SFanKui());
            Skills.Add(new SGuiCai());
        }
    }
}
