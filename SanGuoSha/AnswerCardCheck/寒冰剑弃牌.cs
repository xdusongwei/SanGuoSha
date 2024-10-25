using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{   
    [AnswerMeta(AskForEnum.寒冰剑弃牌)]
    [ChooseTargetCards(aCardCount: 2, aEnableEmpty: false, aAsManyAsPossible: true)]
    [CardsFrom(aHand: true, aWeapon: true, aArmor: true, aHorse: true, aTrial: false)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 寒冰剑弃牌;
}
