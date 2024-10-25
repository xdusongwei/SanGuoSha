using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SQiCai : SkillBase
    {
        public SQiCai(): base(aSkillName: "奇才", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}


        public override byte CalcKitDistance(PlayerBase aPlayer, byte aOldRange, BattlefieldBase aBattlefield)
        {
            return 100; //是的,这就是距离无限 :)
        }
    }
}
