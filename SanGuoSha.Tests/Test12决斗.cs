using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class JueDouCase
    {
        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [4]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.DoNothing("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 3);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [4]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 3);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [4]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [5]
                        ),
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
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
            }
        }

        [Fact]
        public void WuXie()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.无懈可击),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [4]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Single(playerA.Hands);
                Assert.Equal(2, playerB.Health);
                Assert.Single(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.无懈可击),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [4]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [5]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Empty(playerA.Hands);
                Assert.Equal(2, playerB.Health);
                Assert.Empty(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.无懈可击),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
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
                battlefield.TakingCards(playerA, 4);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerA.Hands);
            }
        }
    }
}