using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class STieQi : SkillBase
    {
        public STieQi(): base(aSkillName: "铁骑", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override int CalcAskforTimes(PlayerBase aPlayer, PlayerBase aTarget, CardEffect aEffect, int aOldTimes, BattlefieldBase aBattlefield)
        {
            if (aEffect != CardEffect.杀) return aOldTimes;
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForYN(AskForEnum.铁骑发动, aPlayer);
            if (!response.YN) return aOldTimes;
            using var collector = aBattlefield.NewCollector();
            var card = collector.PopSentenceBySkill(aPlayer, this);
            return card.Color == Card.CardColor.Red ? 0 : aOldTimes;
        }
    }
}
