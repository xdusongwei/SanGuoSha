using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class STuXi: SkillBase
    {
        public STuXi(): base(aSkillName: "突袭", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void TakingCards(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForYN(AskForEnum.突袭发动, aPlayer);
            if (!response.YN) return;
            if (response.Targets.Length > 2 || response.Targets.Length == 0)
                return;
            foreach (var p in response.Targets)
                if (p == aPlayer || p.Dead || !p.HasHand) return;
            aBattlefield.ActionPlayerData.TakeCardsCount = 0;
            foreach (var p in response.Targets)
            {
                Card card = p.Hands[aBattlefield.GetRandom(p.Hands.Count)];
                aBattlefield.Move(p, aPlayer, [card]);
            }
        }
    }
}
