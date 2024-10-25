using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class XiaHouDunCase
    {
        [Fact]
        public void WrongArguments()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CXiaHouDun();
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerA.Hands);
            }
        }

        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CXiaHouDun();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CXiaHouDun();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CXiaHouDun();
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2, 3]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CXiaHouDun();
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Equal(1, playerB.Health);
                Assert.Empty(playerA.Hands);
            }
        }
    }
}
