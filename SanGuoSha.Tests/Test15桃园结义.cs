using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class TaoYuanJieYiCase
    {
        [Fact]
        public void Basic()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.桃园结义),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                    new Card(3, Card.Suit.Heart, 3, CardEffect.无懈可击),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1]
                    ),
                    // B给A无懈, C不选择反无懈
                    (aa) => aa.Answer(
                        aUID: "B",
                        aCardIDs: [2]
                    ),
                    GameDebug.DoNothing("C"),
                    // C无懈C自己
                    GameDebug.DoNothing("C"),
                    (aa) => aa.Answer(
                        aUID: "C",
                        aCardIDs: [3]
                    ),
                    GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 5
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            var playerC = battlefield.Players.Skip(2).First();
            var playerD = battlefield.Players.Skip(3).First();
            var playerE = battlefield.Players.Skip(4).First();
            playerA.Health = 1;
            playerB.Health = 1;
            playerC.Health = 1;
            playerD.Health = 1;
            battlefield.TakingCards(playerA, 1);
            battlefield.TakingCards(playerB, 1);
            battlefield.TakingCards(playerC, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(1, playerA.Health);
            Assert.Equal(2, playerB.Health);
            Assert.Equal(1, playerC.Health);
            Assert.Equal(2, playerD.Health);
            Assert.Equal(2, playerE.Health);
            Assert.Empty(playerA.Hands);
            Assert.Empty(playerB.Hands);
            Assert.Empty(playerC.Hands);
            Assert.Empty(playerD.Hands);
            Assert.Empty(playerE.Hands);
        }
    }
}