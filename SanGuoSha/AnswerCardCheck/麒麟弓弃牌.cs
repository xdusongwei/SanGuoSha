using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{   
    [AnswerMeta(AskForEnum.麒麟弓弃牌)]
    [ChooseTargetCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: false, aWeapon: false, aArmor: false, aHorse: true, aTrial: false)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 麒麟弓弃牌;
}
