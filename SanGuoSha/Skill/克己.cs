using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SKeJi: SkillBase
    {
        public SKeJi(): base(aSkillName: "克己", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}
        private bool UseShaEffect = false;

        public override void BeforeLeading(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            UseShaEffect = false;
        }

        public override void OnUseEffect(PlayerBase aPlayer, CardEffect aEffect, BattlefieldBase aBattlefield)
        {
            if(aBattlefield.ActionPlayerData.CurrentPlayer != aPlayer) return;
            if(aBattlefield.ActionPlayerData.PlayerStage != PlayerStageEnum.Leading) return;
            if(aEffect != CardEffect.杀) return;
            UseShaEffect = true;
        }

        public override void BeforeAbandonment(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            if(UseShaEffect) return;
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForYN(AskForEnum.克己发动, aPlayer);
            if (!response.YN) return;
            aBattlefield.ActionPlayerData.Abandonment = false;
        }
    }
}
