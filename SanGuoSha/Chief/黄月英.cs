using SanGuoSha.BaseClass;
using SanGuoSha.Skill;


namespace SanGuoSha.Chief
{
    public class CHuangYueYing: ChiefBase
    {
        public CHuangYueYing(): base("黄月英", Camp.蜀, GenderType.Female, 3)
        {
            Skills.Add(new SJiZhi());
            Skills.Add(new SQiCai());
        }
    }
}
