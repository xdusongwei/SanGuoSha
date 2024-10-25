using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("青囊")]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    [NeedTargets(1)]
    internal class 青囊;
}
