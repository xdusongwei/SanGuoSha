using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SFanKui: SkillBase
    {
        public SFanKui(): base(aSkillName: "反馈", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnPlayerHarmed(EventRecord aSourceEvent, PlayerBase? aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, sbyte aDamage)
        {
            if (aSource == null || !aSource.HasCard) return;
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForYN(AskForEnum.反馈发动, aTarget);
            if (!response.YN) return;
            using var aaSelect = aBattlefield.NewAsk();
            var responseSelect = aaSelect.AskForCards(AskForEnum.反馈抽牌, aTarget, aSource);
            if (!aSource.HasCardsInHandOrEquipage(responseSelect.Cards)) return;
            aBattlefield.Move(aSource, aTarget, responseSelect.Cards);
        }
    }
}
