using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SLiJian: AdvSkill, IAggressiveStyleSkill
    {
        public SLiJian(): base(aSkillName: "离间", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aTargets[0].Chief.Gender != ChiefBase.GenderType.Male || aTargets[1].Chief.Gender != ChiefBase.GenderType.Male) return false;
            return true;
        }

        public bool Prepare(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var t = aBattlefield.AggressiveCards[CardEffect.决斗];
            return aBattlefield.CheckLeadingAnswer(t, aAnswer);
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            if(!collector.ContainsCard(aAnswer.Cards[0])) return;
            var er = new EventRecord(
                aSource: aAnswer.Leader, 
                aTarget: aAnswer.Targets[0], 
                aTarget2: aAnswer.Targets[1], 
                aEffect: CardEffect.决斗, 
                aSkill: this,
                aIgnoreWuXie: true
            );
            aBattlefield.NewEventNode(er);
        }
    }
}
