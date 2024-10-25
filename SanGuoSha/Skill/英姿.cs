using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SYingZi: SkillBase
    {
        public SYingZi(): base(aSkillName: "英姿", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void TakingCards(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aBattlefield.ActionPlayerData.TakeCardsCount++;
        }
    }
}
