using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SPaoXiao: SkillBase
    {
        public SPaoXiao(): base(aSkillName: "咆哮", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void WeaponUpdated(PlayerBase aPlayer, Card? aWeapon , BattlefieldBase aBattlefield)
        {
            if(aBattlefield.ActionPlayerData.CurrentPlayer != aPlayer) return;
            if(!aBattlefield.ActionPlayerData.ShaNoLimitFlags.Contains(SkillName))
                aBattlefield.ActionPlayerData.ShaNoLimitFlags.Add(SkillName);
        }

        
        public override void BeforeTurnStart(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aBattlefield.ActionPlayerData.ShaNoLimitFlags.Add(SkillName);
        }

        public override void AfterTurnEnd(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aBattlefield.ActionPlayerData.ShaNoLimitFlags.Remove(SkillName);
        }

        public override void AggressiveUsingEffect(PlayerBase aPlayer, CardEffect aEffect, BattlefieldBase aBattlefield)
        {
            var apd = aBattlefield.ActionPlayerData;
            if(apd.CurrentPlayer != aPlayer) return;
            if(apd.PlayerStage != PlayerStageEnum.Leading) return;
            if(aEffect != CardEffect.杀) return;
            if(!apd.ShaNoLimitFlags.Contains(SkillName)) return;
            if(apd.ShaNoLimitFlags.Count > 1) return;
            if(apd.ShaTimes < 2) return;
            aBattlefield.CreateActionNode(new ActionNode(aPlayer, SkillName));
        }
    }
}
