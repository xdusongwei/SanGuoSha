using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerTransFormSkillCheck
{   
    [TransformSkill("激将")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: false)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(0)]
    internal class 激将;
}
