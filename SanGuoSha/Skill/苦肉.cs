using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SKuRou: AdvSkill, IAggressiveStyleSkill
    {
        public SKuRou(): base(aSkillName: "苦肉", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) 
        {
            AggressiveMaxTimes = -1;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            aBattlefield.DamageHealth(aAnswer.Leader, 1, null, new EventRecord(aTarget: aAnswer.Leader, aSkill: this));
            aBattlefield.TakingCards(aAnswer.Leader, 2);
        }
    }
}
