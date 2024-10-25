using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class MaChaoCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.仁王盾),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1]
                        ),
                        GameDebug.Yes("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CMaChao();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.仁王盾),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1]
                        ),
                        GameDebug.Yes("A"),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CMaChao();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.仁王盾),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["C"],
                            aCardIDs: [1]
                        ),
                        GameDebug.Yes("A"),
                        GameDebug.DoNothing("C"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CMaChao();
                var playerC = battlefield.Players.Skip(2).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerC.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.顺手牵羊),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CMaChao();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Jia1Ma = new Card(2, Card.Suit.Heart, 2, CardEffect.加1马);
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
            }
        }
    }
}
