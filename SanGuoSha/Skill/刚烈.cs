using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SGangLie: SkillBase
    {
        public SGangLie(): base(aSkillName: "刚烈", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnPlayerHarmed(EventRecord aSourceEvent, PlayerBase? aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, sbyte aDamage)
        {
            if (aSource == null) return;
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForYN(AskForEnum.刚烈发动, aTarget);
            if (!response.YN) return;
            using var collector = aBattlefield.NewCollector();
            Card judgement = collector.PopSentenceBySkill(aTarget, this);
            if (judgement.CardSuit == Card.Suit.Heart) return;
            if (aSource.Hands.Count < 2)
            {
                aBattlefield.DamageHealth(aSource, 1, aTarget, new EventRecord(aTarget, aSource, [], CardEffect.None, this));
            }
            else
            {
                using var aaSelect = aBattlefield.NewAsk();
                var responseSelect = aaSelect.AskForCards(AskForEnum.刚烈弃牌, aSource);
                if (responseSelect.Cards.Length != 2)
                    aBattlefield.DamageHealth(aSource, 1, aTarget, new EventRecord(aTarget, aSource, [], CardEffect.None, this));
                else
                    collector.DropPlayerReponse(responseSelect);
            }
        }
    }
}
