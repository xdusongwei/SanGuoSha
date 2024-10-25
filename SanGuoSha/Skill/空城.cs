using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SKongCheng : SkillBase
    {
        public SKongCheng(): base(aSkillName: "空城", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override bool EffectFeasible(CardEffect aEffect, PlayerBase aTarget, bool aFeasible , BattlefieldBase aBattlefield)
        {
            switch (aEffect)
            {
                case CardEffect.决斗:
                case CardEffect.杀:
                    if (aTarget.Hands.Count == 0)
                        return false;
                    break;
            }
            return aFeasible;
        }
    }
}
