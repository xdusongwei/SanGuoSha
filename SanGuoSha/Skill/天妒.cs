using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class STianDu: SkillBase
    {
        public STianDu(): base(aSkillName: "天妒", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnSentenceCardTakeEffect(PlayerBase aPlayer, Card aCard, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            collector.Pick(aCard, aPlayer);
        }
    }
}
