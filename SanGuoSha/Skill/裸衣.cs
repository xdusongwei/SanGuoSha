using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SLuoYi: SkillBase
    {
        public SLuoYi(): base(aSkillName: "裸衣", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        private bool Active = false;

        public override void BeforeTurnStart(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForYN(AskForEnum.裸衣发动, aPlayer);
            if (!response.YN) return;
            Active = true;
            aBattlefield.ActionPlayerData.TakeCardsCount--;
        }

        public override void AfterTurnEnd(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            Active = false;
        }

        public override sbyte CalcDamage(PlayerBase aPlayer, CardEffect aEffect, sbyte aDamage, BattlefieldBase aBattlefield)
        {
            if (!Active) return aDamage;
            return aEffect switch
            {
                CardEffect.杀 or CardEffect.决斗 => ++aDamage,
                _ => aDamage,
            };
        }
    }
}
