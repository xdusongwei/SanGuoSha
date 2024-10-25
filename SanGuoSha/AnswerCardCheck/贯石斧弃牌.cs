using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{
    [AnswerMeta(AskForEnum.贯石斧弃牌)]
    [ChooseMyselfCards(aCardCount: 2, aEnableEmpty: false, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true, aWeapon: false, aArmor: true, aHorse: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 贯石斧弃牌;
}
