using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SQianXun: SkillBase
    {
        public SQianXun(): base(aSkillName: "谦逊", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override bool EffectFeasible(CardEffect aEffect, PlayerBase aTarget, bool aFeasible, BattlefieldBase aBattlefield)
        {
            switch (aEffect)
            {
                case CardEffect.顺手牵羊:
                case CardEffect.乐不思蜀:
                    return false;
            }
            return aFeasible;
        }
    }
}
