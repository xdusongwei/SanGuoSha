using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerCardCheck
{   
    [AnswerMeta(AskForEnum.雌雄双股剑弃牌)]
    [ChooseMyselfCards(aCardCount: 1, aEnableEmpty: true, aAsManyAsPossible: false)]
    [CardsFrom(aHand: true)]
    [DisableWeapon]
    [DisableArmor]
    [DisableSkill]
    [DisableAutoEffect]
    internal class 雌雄双股剑弃牌;
}
