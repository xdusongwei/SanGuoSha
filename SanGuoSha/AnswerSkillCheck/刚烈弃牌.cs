using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerSkillCheck
{   
    [AnswerMeta(AskForEnum.刚烈弃牌)]
    [ChooseMyselfCards(aCardCount: 2, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true, aWeapon: false, aArmor: false, aHorse: false, aTrial: false)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 刚烈弃牌;
}
