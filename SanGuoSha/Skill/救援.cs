using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SJiuYuan: SkillBase
    {
        public SJiuYuan(): base(aSkillName: "救援", aEnabled: SkillEnabled.Passive, aIsMajestySkill: true) {}

        public override sbyte CalcRescuePoint(PlayerBase aPlayer, PlayerBase aRescuer, CardEffect aEffect, sbyte aOldPoint, BattlefieldBase aBattlefield)
        {
            var newPoint = aOldPoint;
            if(aRescuer.Chief.ChiefCamp == ChiefBase.Camp.吴 && aEffect == CardEffect.桃) newPoint++;
            return newPoint;
        }
    }
}
