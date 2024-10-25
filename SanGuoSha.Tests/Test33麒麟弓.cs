using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class QiLinGongCase
    {
        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.麒麟弓),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.减1马),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
                Assert.Null(playerB.Jian1Ma);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.麒麟弓),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.加1马),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
                Assert.Null(playerB.Jia1Ma);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.麒麟弓),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.加1马),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: []
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
                Assert.NotNull(playerB.Jia1Ma);
            }
        }
    }
}
