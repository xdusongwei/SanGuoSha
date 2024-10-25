using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveSkillCheck
{   
    [AggressiveSkill("奇袭")]
    [CardsFrom(aHand: true)]
    [DisableAutoEffect]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    [NeedTargets(1)]
    [SourceNotInTargets]
    internal class 奇袭;
}
