using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{   
    [AnswerMeta(AskForEnum.顺手牵羊抽牌)]
    [ChooseTargetCards(aCardCount: 1, aEnableEmpty:false, aAsManyAsPossible: true, aCardsInPriate: true)]
    [CardsFrom(aHand: true, aWeapon: true, aArmor: true, aHorse: true, aTrial: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 顺手牵羊抽牌;
}
