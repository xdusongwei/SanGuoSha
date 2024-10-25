using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SJiZhi : SkillBase
    {
        public SJiZhi(): base(aSkillName: "集智", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnUseEffect(PlayerBase aPlayer, CardEffect aEffect, BattlefieldBase aBattlefield)
        {
            if(!Card.IsKit(aEffect)) return;
            aBattlefield.TakingCards(aPlayer, 1);
        }
    }
}
