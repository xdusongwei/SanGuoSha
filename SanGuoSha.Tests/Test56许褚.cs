using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class XuChuCase
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
                        GameDebug.Yes("A"),
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
                )
                {
                    TakingCardsCount = 2,
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CXuChu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Equal(1, battlefield.ActionPlayerData.TakeCardsCount);
                Assert.Empty(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        GameDebug.DoNothing("A"),
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
                )
                {
                    TakingCardsCount = 2,
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CXuChu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerB.Health);
                Assert.Equal(2, battlefield.ActionPlayerData.TakeCardsCount);
                Assert.Single(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.决斗),
                    ],
                    aActions: [
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 3
                )
                {
                    TakingCardsCount = 2,
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CXuChu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Equal(1, battlefield.ActionPlayerData.TakeCardsCount);
                Assert.Empty(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.决斗),
                    ],
                    aActions: [
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                )
                {
                    TakingCardsCount = 2,
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CXuChu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
            }
        }
    }
}
