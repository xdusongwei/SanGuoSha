using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SFanJian: AdvSkill, IAggressiveStyleSkill
    {
        public SFanJian(): base(aSkillName: "反间", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var target = aAnswer.Targets[0];
            using var aaSuit = aBattlefield.NewAsk();
            var responseSuit = aaSuit.AskForSuit(AskForEnum.反间选花色, target);
            using var aaCard = aBattlefield.NewAsk();
            var responseCard = aaCard.AskForCards(AskForEnum.反间抽牌, target, aAnswer.Leader);
            if(responseCard.Cards.Length != 1) return;
            if(!aAnswer.Leader.HasCardsInHand(responseCard.Cards)) return;
            using var collector = aBattlefield.NewCollector();
            collector.DropCards(aAnswer.Leader, responseCard.Cards);
            var card = responseCard.Cards[0];
            if(card.CardSuit != responseSuit.Suit)
            {
                aBattlefield.DamageHealth(target, 1, aAnswer.Leader, new EventRecord(aSource: aAnswer.Leader, aTarget: target, aSkill: this));
            }
            if(target.Dead) return;
            collector.Pick(card, target);
        }
    }
}
