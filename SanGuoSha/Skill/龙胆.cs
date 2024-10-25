using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SLongDan: AdvSkill, IAggressiveStyleSkill, ITransformStyleSkill
    {
        public SLongDan(): base(aSkillName: "龙胆", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if(aAskFor == AskForEnum.杀 && aPlayer.HasHand) return true;
            if(aAskFor == AskForEnum.闪 && aPlayer.HasHand) return true;
            return false;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards.Length != 1 || !aPlayer.HasCardsInHand(aCards)) return false;
            return aAskFor == AskForEnum.杀 || aAskFor == AskForEnum.闪;
        }

        public AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var card = aAnswer.Cards[0];
            if(card.CardEffect == CardEffect.杀 && aAnswer.AskFor == AskForEnum.闪)
                aAnswer.Effect = CardEffect.闪;
            if(card.CardEffect == CardEffect.闪 && aAnswer.AskFor == AskForEnum.杀)
                aAnswer.Effect = CardEffect.杀;
            return aAnswer;
        }

        bool IAggressiveStyleSkill.Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return aCards[0].CardEffect == CardEffect.闪;
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
                        aEffect: aAnswer.Effect, 
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
