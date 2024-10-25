using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SJianXiong: SkillBase
    {
        public SJianXiong(): base(aSkillName: "奸雄", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnPlayerHarmed(EventRecord aSourceEvent, PlayerBase? aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, sbyte aDamage)
        {
            using var collector = aBattlefield.NewCollector();
            if (aSourceEvent.Cards.Length != 0 && collector.ContainsAnyCards(aSourceEvent.Cards))
            {
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForYN(AskForEnum.奸雄发动, aTarget);
                if (!response.YN) return;
                foreach(var c in aSourceEvent.Cards)
                    collector.Pick(c, aTarget);
            }
        }
    }
}
