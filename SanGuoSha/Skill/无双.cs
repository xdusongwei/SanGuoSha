using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SWuShuang: SkillBase
    {
        public SWuShuang(): base(aSkillName: "无双", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override int CalcAskforTimes(PlayerBase aPlayer, PlayerBase aTarget,  CardEffect aEffect, int aOldTimes , BattlefieldBase aBattlefield)
        {
            switch(aEffect)
            {
                case CardEffect.杀:
                case CardEffect.决斗:
                    aBattlefield.CreateActionNode(new ActionNode(aPlayer, SkillName));
                    return 2;
                default:
                    return aOldTimes;
            };
        }
    }
}
