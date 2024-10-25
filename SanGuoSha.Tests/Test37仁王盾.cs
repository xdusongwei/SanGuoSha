using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class RenWangDunCase
    {
        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.仁王盾),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.仁王盾),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerB.Hands);
                Assert.Equal(1, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.仁王盾),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.丈八蛇矛),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3, 4],
                            aWeaponEffect: "丈八蛇矛",
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.仁王盾),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.丈八蛇矛),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3, 4],
                            aWeaponEffect: "丈八蛇矛",
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.仁王盾),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.丈八蛇矛),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3, 4],
                            aWeaponEffect: "丈八蛇矛",
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerB.Hands);
                Assert.Equal(1, playerA.Health);
            }
        }
    }
}