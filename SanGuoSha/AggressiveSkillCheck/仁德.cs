using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("仁德")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedTargets(1)]
    [SourceNotInTargets]
    internal class 仁德;
}
