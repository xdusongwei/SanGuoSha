using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerSkillCheck
{   
    [AnswerMeta(AskForEnum.鬼才改判)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true, aWeapon: false, aArmor: false, aHorse: false, aTrial: false)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 鬼才改判;
}
