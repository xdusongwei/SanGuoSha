using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("国色")]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    [NeedTargets(1)]
    [SourceNotInTargets]
    internal class 国色;
}
