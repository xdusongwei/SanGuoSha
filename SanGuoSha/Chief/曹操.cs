using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CCaoCao: ChiefBase
    {
        public CCaoCao(): base("曹操", Camp.魏, GenderType.Male, 4)
        {
            Skills.Add(new SJianXiong());
            Skills.Add(new SHuJia());
        }
    }
}
