using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerTransFormSkillCheck
{   
    [TransformSkill("倾国")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    internal class 倾国;
}
