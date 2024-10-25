using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class NanManRuQinCase
    {
        [Fact]
        public void Basic()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.南蛮入侵),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                    new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    new Card(4, Card.Suit.Heart, 4, CardEffect.闪),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1]
                    ),
                    (aa) => aa.Answer(
                        aUID: "B",
                        aCardIDs: [2],
                        aTargetUIDs: ["A"]
                    ),
                    (aa) => aa.Answer(
                        aUID: "C",
                        aCardIDs: [3],
                        aTargetUIDs: ["A"]
                    ),
                    (aa) => aa.Answer(
                        aUID: "D",
                        aCardIDs: [4],
                        aTargetUIDs: ["A"]
                    ),
                    GameDebug.DoNothing("E"),
                    GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 5
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            var playerC = battlefield.Players.Skip(2).First();
            var playerD = battlefield.Players.Skip(3).First();
            var playerE = battlefield.Players.Skip(4).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.TakingCards(playerB, 1);
            battlefield.TakingCards(playerC, 1);
            battlefield.TakingCards(playerD, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(2, playerA.Health);
            Assert.Equal(2, playerB.Health);
            Assert.Equal(2, playerC.Health);
            Assert.Equal(1, playerD.Health);
            Assert.Equal(1, playerE.Health);
            Assert.Empty(playerA.Hands);
            Assert.Empty(playerB.Hands);
            Assert.Empty(playerC.Hands);
            Assert.Single(playerD.Hands);
            Assert.Empty(playerE.Hands);
        }
    }
}