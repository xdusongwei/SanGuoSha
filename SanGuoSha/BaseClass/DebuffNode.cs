

namespace SanGuoSha.BaseClass
{
    public class DebuffNode(Card aCard, CardEffect aEffect) : Card(aCard.ID, aCard.CardSuit, aCard.CardIndex, aCard.CardEffect, aCard.Element, aCard.CustomName)
    {
        public readonly CardEffect TrialEffect = aEffect;
    }
}
