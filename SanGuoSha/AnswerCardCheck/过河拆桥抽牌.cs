using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{   
    [AnswerMeta(AskForEnum.过河拆桥抽牌)]
    [ChooseTargetCards(aCardCount: 1, aEnableEmpty: false, aAsManyAsPossible: true)]
    [CardsFrom(aHand: true, aWeapon: true, aArmor: true, aHorse: true, aTrial: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 过河拆桥抽牌;
}
