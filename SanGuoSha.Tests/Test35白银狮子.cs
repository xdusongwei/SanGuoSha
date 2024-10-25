using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class BaiYinShiZiCase
    {
        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.白银狮子),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪电),
                        new Card(3, Card.Suit.Spade, 2, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                        GameDebug.FinishLeading("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.白银狮子),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.白银狮子),
                        new Card(3, Card.Suit.Spade, 2, CardEffect.白银狮子),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 1,
                    aInitHealth: 99
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(3, playerA.Health);
            }
        }
    }
}