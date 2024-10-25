using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SGuoSe: AdvSkill, IAggressiveStyleSkill
    {
        public SGuoSe(): base(aSkillName: "国色", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) 
        {
            AggressiveMaxTimes = -1;
        }


        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards[0].CardSuit != Card.Suit.Diamond) return false;
            if (aTargets[0].HasDebuff(CardEffect.乐不思蜀)) return false;
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            if(!collector.ContainsCard(aAnswer.Cards[0])) return;
            var er = new EventRecord(
                aSource: aAnswer.Leader, 
                aTarget: aAnswer.Targets[0], 
                aCards: aAnswer.Cards,
                aEffect: CardEffect.乐不思蜀, 
                aSkill: this,
                aIgnoreWuXie: true
            );
            aBattlefield.NewEventNode(er);
        }
    }
}
