using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SLuoShen: SkillBase
    {
        public SLuoShen(): base(aSkillName: "洛神", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnSentenceCardTakeEffect(PlayerBase aPlayer, Card aCard, BattlefieldBase aBattlefield)
        {
            if (aCard.Color == Card.CardColor.Black)
            {
                using var collector = aBattlefield.NewCollector();
                collector.Pick(aCard, aPlayer);
            }
        }

        public override void BeforeTurnStart(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            do
            {
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForYN(AskForEnum.洛神发动, aPlayer);
                if (!response.YN) break;
            } while (collector.PopSentenceBySkill(aPlayer, this).Color == Card.CardColor.Black);
        }
    }
}
