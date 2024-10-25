using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{
    [AnswerMeta(AskForEnum.杀)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true)]
    [DisableArmor]
    [EffectFilter(CardEffect.杀)]
    internal class 杀;
}
