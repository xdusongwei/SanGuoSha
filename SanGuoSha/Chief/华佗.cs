using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CHuaTuo: ChiefBase
    {
        public CHuaTuo(): base("华佗", Camp.群, GenderType.Male, 3)
        {
            Skills.Add(new SQingNang());
            Skills.Add(new SJiJiu());
        }
    }
}
