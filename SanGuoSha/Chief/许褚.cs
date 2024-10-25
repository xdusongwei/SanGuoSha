using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CXuChu: ChiefBase
    {
        public CXuChu(): base("许褚", Camp.魏, GenderType.Male, 4)
        {
            Skills.Add(new SLuoYi());
        }
    }
}
