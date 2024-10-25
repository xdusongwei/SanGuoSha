using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("武圣")]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    [SourceNotInTargets]
    [PlayersDistinct]
    internal class 武圣;
}
