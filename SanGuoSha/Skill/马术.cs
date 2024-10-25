using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SMaShu : SkillBase
    {
        public SMaShu(): base(aSkillName: "马术", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}


        public override byte CalcKitDistance(PlayerBase aPlayer, byte aOldRange, BattlefieldBase aBattlefield)
        {
            return ++aOldRange;
        }

        public override byte CalcShaDistance(PlayerBase aPlayer, byte aOldRange, BattlefieldBase aBattlefield)
        {
            return ++aOldRange;
        }
    }
}
