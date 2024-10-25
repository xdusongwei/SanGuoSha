using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SZhiHeng: AdvSkill, IAggressiveStyleSkill
    {
        public SZhiHeng(): base(aSkillName: "制衡", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards.Length == 0) return false;
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            aBattlefield.TakingCards(aAnswer.Leader, aAnswer.Cards.Length);
        }
    }
}
