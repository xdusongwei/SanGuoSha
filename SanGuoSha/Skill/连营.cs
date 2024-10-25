using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SLianYing: SkillBase
    {
        public SLianYing(): base(aSkillName: "连营", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnRemoveCards(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            if (aPlayer.Hands.Count != 0) return;
            aBattlefield.TakingCards(aPlayer, 1);
        }
    }
}
