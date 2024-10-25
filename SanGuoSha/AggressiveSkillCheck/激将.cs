using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("激将")]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(0)]
    [SourceNotInTargets]
    [PlayersDistinct]
    internal class 激将;
}
