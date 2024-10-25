using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CZhenJi: ChiefBase
    {
        public CZhenJi(): base("甄姬", Camp.魏, GenderType.Female, 3)
        {
            Skills.Add(new SLuoShen());
            Skills.Add(new SQingGuo());
        }
    }
}
