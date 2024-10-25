using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerSkillCheck
{   
    [AnswerMeta(AskForEnum.遗计分牌)]
    [ChooseMyselfCards(aCardCount: 2, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: false, aWeapon: false, aArmor: false, aHorse: false, aTrial: false, aSlot: "遗计")]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 遗计分牌;
}
