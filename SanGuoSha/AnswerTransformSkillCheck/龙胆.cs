using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerTransFormSkillCheck
{   
    [TransformSkill("龙胆")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: true)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(1)]
    internal class 龙胆;
}
