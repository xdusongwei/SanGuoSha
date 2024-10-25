using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{
    [AnswerMeta(AskForEnum.桃)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true)]
    [DisableWeapon]
    [DisableArmor]
    [EffectFilter(CardEffect.桃)]
    internal class 桃;
}
