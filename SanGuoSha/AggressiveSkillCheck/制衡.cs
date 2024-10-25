using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("制衡")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: true, aWeapon: true, aArmor: true, aHorse: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedTargets(0)]
    internal class 制衡;
}
