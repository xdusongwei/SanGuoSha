using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("离间")]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    [NeedTargets(2)]
    [SourceNotInTargets]
    [PlayersDistinct]
    internal class 离间;
}
