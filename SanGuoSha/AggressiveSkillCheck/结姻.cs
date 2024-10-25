using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("结姻")]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(2)]
    [NeedTargets(1)]
    [SourceNotInTargets]
    [PlayersDistinct]
    internal class 结姻;
}
