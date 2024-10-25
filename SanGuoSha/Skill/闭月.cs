using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SBiYue: SkillBase
    {
        public SBiYue(): base(aSkillName: "闭月", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void AfterTurnEnd(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aBattlefield.TakingCards(aPlayer, 1);
        }
    }
}
