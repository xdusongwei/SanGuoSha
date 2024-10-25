using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{
    [AnswerMeta(AskForEnum.无懈可击)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [EffectFilter(CardEffect.无懈可击)]
    internal class 无懈可击;
}
