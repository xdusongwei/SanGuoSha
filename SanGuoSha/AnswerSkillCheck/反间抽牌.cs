using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerSkillCheck
{   
    [AnswerMeta(AskForEnum.反间抽牌)]
    [ChooseTargetCards(aCardCount: 1, aEnableEmpty: false, aAsManyAsPossible: true)]
    [CardsFrom(aHand: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 反间抽牌;
}
