using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class ZhangFeiCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1]
                        ),
                        GameDebug.DoNothing("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [2]
                        ),
                        GameDebug.DoNothing("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [3]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangFei();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.青龙偃月刀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [4]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [5]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1]
                        ),
                        GameDebug.DoNothing("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [2]
                        ),
                        GameDebug.DoNothing("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [3]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangFei();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 5);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
        }
    }
}
