using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SQiXi: AdvSkill, IAggressiveStyleSkill
    {
        public SQiXi(): base(aSkillName: "奇袭", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) 
        {
            AggressiveMaxTimes = -1;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards[0].Color != Card.CardColor.Black) return false;
            return true;
        }

        public bool Prepare(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var t = aBattlefield.AggressiveCards[CardEffect.过河拆桥];
            return aBattlefield.CheckLeadingAnswer(t, aAnswer);
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            if(collector.ContainsCard(aAnswer.Cards[0]))
            {
                foreach(var t in aAnswer.Targets)
                {
                    var er = new EventRecord(
                        aSource: aAnswer.Leader, 
                        aTarget: t,
                        aEffect: CardEffect.过河拆桥, 
                        aCards: aAnswer.Cards,
                        aSkill: this,
                        aIgnoreWuXie: true
                    );
                    aBattlefield.NewEventNode(er);
                }
            }
        }
    }
}
