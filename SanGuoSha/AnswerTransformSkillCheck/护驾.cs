using SanGuoSha.BaseClass;


namespace SanGuoSha.AnswerTransFormSkillCheck
{   
    [TransformSkill("护驾")]
    [ChooseMyselfCards]
    [CardsFrom(aHand: false)]
    [NoOneDead]
    [NeedSourcePlayer]
    [NeedCards(0)]
    internal class 护驾;
}
