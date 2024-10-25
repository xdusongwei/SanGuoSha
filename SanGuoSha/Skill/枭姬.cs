using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SXiaoJi: SkillBase
    {
        public SXiaoJi(): base(aSkillName: "枭姬", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void DropEquipage(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aBattlefield.TakingCards(aPlayer, 2);
        }
    }
}
