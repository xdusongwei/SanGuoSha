using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class LvMengCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.闪),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CLvMeng();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 4);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.闪),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.闪),
                    ],
                    aActions: [
                        GameDebug.DoNothing("A"),
                        GameDebug.Yes("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CLvMeng();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 4);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(4, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.决斗),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.闪),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.闪),
                        new Card(5, Card.Suit.Spade, 5, CardEffect.闪),
                        new Card(6, Card.Suit.Spade, 6, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [6]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CLvMeng();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 5);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
            }
        }
    }
}
