using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SLiuLi: SkillBase
    {
        public SLiuLi(): base(aSkillName: "流离", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void BeforeProcessingEvent(PlayerBase aTarget, ref EventRecord aEvent, BattlefieldBase aBattlefield)
        {
            if (aEvent.Effect != CardEffect.杀) return;
            using var aaYN = aBattlefield.NewAsk();
            var responseYN = aaYN.AskForYN(AskForEnum.流离发动, aTarget);
            if (!responseYN.YN) return;
            using var aaSkill = aBattlefield.NewAsk();
            var responseSkill = aaSkill.AskForCards(AskForEnum.流离, aTarget);
            if (responseSkill.Cards.Length != 1) return;
            if (!aTarget.HasCardsInHandOrEquipage(responseSkill.Cards)) return;
            if (responseSkill.Targets.Length != 1) return;
            if (responseSkill.Targets[0] == aTarget) return;
            if (responseSkill.Targets[0] == aEvent.Source) return;
            if (responseSkill.Targets[0].Dead) return;
            var withWeapon = aTarget.Weapon != responseSkill.Cards[0];
            if (!aBattlefield.Players.WithinShaRange(aTarget, responseSkill.Targets[0], aBattlefield, withWeapon)) return;
            using var collector = aBattlefield.NewCollector();
            collector.DropPlayerReponse(responseSkill);
            aEvent.Target = responseSkill.Targets[0];
        }
    }
}
