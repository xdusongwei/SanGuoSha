using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SJieYin: AdvSkill, IAggressiveStyleSkill
    {
        public SJieYin(): base(aSkillName: "结姻", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aTargets[0].Chief.Gender != ChiefBase.GenderType.Male) return false;
            if (!aTargets[0].Injured) return false;
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            if(!collector.ContainsCards(aAnswer.Cards)) return;
            aBattlefield.RegainHealth(aAnswer.Targets[0], 1);
            aBattlefield.RegainHealth(aAnswer.Leader, 1);
        }
    }
}
