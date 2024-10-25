using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("反间")]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(0)]
    [NeedTargets(1)]
    [SourceNotInTargets]
    internal class 反间;
}
