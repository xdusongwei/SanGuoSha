using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SGuanXing: SkillBase
    {
        public SGuanXing(): base(aSkillName: "观星", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        private readonly string Total = AskAnswer.GuanXingSlotName;

        public override void OnCreate(PlayerBase aPlayer)
        {
            aPlayer.Slots.Slots.Add(new CardSlot(Total, false, false));
        }

        public override void BeforeTurnStart(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aPlayer.Slots[Total].Cards.Clear();
            using var aa = aBattlefield.NewAsk();
            var responseYN = aa.AskForYN(AskForEnum.观星发动, aPlayer);
            if(!responseYN.YN) return;
            int alive = aBattlefield.Players.AliveCount;
            alive = int.Min(5, alive);
            var cards = aBattlefield.CardsHeap.Pop(alive);
            aPlayer.Slots[Total].Cards.AddRange(cards);
            using var aaGuanXing = aBattlefield.NewAsk();
            var responseGuanXing = aaGuanXing.AskForCards(AskForEnum.观星, aPlayer);
            if(responseGuanXing.GuanXingBottom.Length + responseGuanXing.GuanXingTop.Length != alive)
                aBattlefield.CardsHeap.PutOnBottom(cards);
            else
            {
                aBattlefield.CardsHeap.PutOnTop(responseGuanXing.GuanXingTop);
                aBattlefield.CardsHeap.PutOnBottom(responseGuanXing.GuanXingBottom);
            }
            aPlayer.Slots[Total].Cards.Clear();
        }

        public override void AfterTurnEnd(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            aPlayer.Slots[Total].Cards.Clear();
        }
    }
}
