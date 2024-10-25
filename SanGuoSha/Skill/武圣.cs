using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SWuSheng: AdvSkill, IAggressiveStyleSkill, ITransformStyleSkill
    {
        public SWuSheng(): base(aSkillName: "武圣", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return aAskFor == AskForEnum.杀 && aPlayer.HasHand;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards.Length != 1 || !aPlayer.HasCardsInHand(aCards)) return false;
            return aCards.Length == 1 && aAskFor == AskForEnum.杀 && aCards[0].Color == Card.CardColor.Red;
        }

        public AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            aAnswer.Effect = CardEffect.杀;
            return aAnswer;
        }

        bool IAggressiveStyleSkill.Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return aCards[0].Color == Card.CardColor.Red;
        }

        public bool Prepare(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var t = aBattlefield.AggressiveCards[CardEffect.杀];
            var result = aBattlefield.CheckLeadingAnswer(t, aAnswer);
            if(result)
                aAnswer.Effect = CardEffect.杀;
            return result;
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
                        aEffect: CardEffect.杀, 
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
