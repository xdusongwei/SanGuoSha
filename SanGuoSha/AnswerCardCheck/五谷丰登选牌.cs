using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{
    [AnswerMeta(AskForEnum.五谷丰登选牌)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: false, aWeapon: false, aArmor: false, aHorse: false, aTrial: false, aSlot: "五谷丰登")]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 五谷丰登选牌;
}
