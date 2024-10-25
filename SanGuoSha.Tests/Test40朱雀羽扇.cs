using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class ZhuQueYuShanCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.Yes("A"),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Weapon = new Card(8, Card.Suit.Heart, 8, CardEffect.朱雀羽扇);
                playerB.Armor = new Card(9, Card.Suit.Heart, 9, CardEffect.藤甲);
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Equal(1, battlefield.CardsHeap.TrashCardCount);
                Assert.Equal(Card.ElementType.None, battlefield.CardsHeap.Pop().Element);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀, Card.ElementType.Thunder),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Weapon = new Card(8, Card.Suit.Heart, 8, CardEffect.朱雀羽扇);
                playerB.Armor = new Card(9, Card.Suit.Heart, 9, CardEffect.藤甲);
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerB.Health);
                Assert.Equal(1, battlefield.CardsHeap.TrashCardCount);
                Assert.Equal(Card.ElementType.Thunder, battlefield.CardsHeap.Pop().Element);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀, Card.ElementType.Fire),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Weapon = new Card(8, Card.Suit.Heart, 8, CardEffect.朱雀羽扇);
                playerB.Armor = new Card(9, Card.Suit.Heart, 9, CardEffect.藤甲);
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Equal(1, battlefield.CardsHeap.TrashCardCount);
                Assert.Equal(Card.ElementType.Fire, battlefield.CardsHeap.Pop().Element);
            }
        }
    }
}
