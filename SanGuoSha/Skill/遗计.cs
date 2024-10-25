using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SYiJi: SkillBase
    {
        public SYiJi(): base(aSkillName: "遗计", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override void OnCreate(PlayerBase aPlayer)
        {
            aPlayer.Slots.Slots.Add(new CardSlot(SkillName, false, true));
        }

        public override void OnPlayerHarmed(EventRecord aSourceEvent, PlayerBase? aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, sbyte aDamage)
        {
            var slot = aTarget.Slots[SkillName];
            for (int i = 0; i < aDamage; i++)
            {
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForYN(AskForEnum.遗计发动, aTarget);
                if (!response.YN) break;
                var cards = aBattlefield.CardsHeap.Pop(2);
                slot.Cards.Clear();
                slot.Cards.AddRange(cards);
                int times = 0;
                while (slot.Cards.Count > 0 && times < 2)
                {
                    using var aaSelect = aBattlefield.NewAsk();
                    var responseSelect = aaSelect.AskForCards(AskForEnum.遗计分牌, aTarget);
                    if (responseSelect.Targets.Length != 1 || responseSelect.Targets[0] == aTarget || responseSelect.Targets[0].Dead || responseSelect.Cards.Length == 0) break;
                    foreach (var c in responseSelect.Cards)
                        if (!slot.Cards.Contains(c)) break;
                    foreach (var c in responseSelect.Cards)
                    {
                        slot.Cards.Remove(c);
                        responseSelect.Targets[0].Hands.Add(c);
                    }
                    
                    times++;
                }
                if(slot.Cards.Count > 0)
                {
                    foreach(var c in slot.Cards.ToArray())
                    {
                        slot.Cards.Remove(c);
                        aTarget.Hands.Add(c);
                    }
                }
                slot.Reset();
            }
        }
    }
}
