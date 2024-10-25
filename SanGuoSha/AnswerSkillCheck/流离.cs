using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerSkillCheck
{   
    [AnswerMeta(AskForEnum.流离)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 流离;
}
