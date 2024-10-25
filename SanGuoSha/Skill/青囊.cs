using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SQingNang: AdvSkill, IAggressiveStyleSkill
    {
        public SQingNang(): base(aSkillName: "青囊", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (!aTargets[0].Injured) return false;
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            if(collector.ContainsCard(aAnswer.Cards[0]))
                aBattlefield.RegainHealth(aAnswer.Targets[0], 1);
        }
    }
}
