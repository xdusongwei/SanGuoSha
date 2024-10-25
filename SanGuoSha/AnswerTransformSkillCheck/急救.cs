using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerTransFormSkillCheck
{   
    [TransformSkill("急救")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: true, aWeapon: true, aArmor: true, aHorse: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    internal class 急救;
}
