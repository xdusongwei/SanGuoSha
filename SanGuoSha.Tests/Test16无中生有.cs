using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class WuZhongShengYouCase
    {
        [Fact]
        public void Basic()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.无中生有),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                    new Card(3, Card.Suit.Heart, 3, CardEffect.闪),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1]
                    ),
                    GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(2, playerA.Hands.Count);
            Assert.True(!playerA.Hands.Any(i => i.ID == 1));
        }
    }
}
