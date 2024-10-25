using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class QingLongYanYueDaoCase
    {
        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.青龙偃月刀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.青龙偃月刀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [4]
                        ),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 3);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.青龙偃月刀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                        new Card(6, Card.Suit.Heart, 6, CardEffect.杀),
                        new Card(7, Card.Suit.Heart, 7, CardEffect.杀),
                        new Card(8, Card.Suit.Heart, 8, CardEffect.闪),
                        new Card(9, Card.Suit.Heart, 9, CardEffect.闪),
                        new Card(10, Card.Suit.Heart, 10, CardEffect.闪),
                        new Card(11, Card.Suit.Heart, 11, CardEffect.闪),
                        new Card(12, Card.Suit.Heart, 12, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [8]
                        ),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [9]
                        ),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [4]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [10]
                        ),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [5]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [11]
                        ),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [6]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [12]
                        ),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [7]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 7);
                battlefield.TakingCards(playerB, 5);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
        }
    }
}