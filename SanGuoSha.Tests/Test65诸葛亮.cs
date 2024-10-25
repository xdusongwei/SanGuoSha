using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class ZhuGeLiangCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.桃园结义),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.桃园结义),
                    ],
                    aActions: [
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aGuanXingTop: [1],
                            aGuanXingBottom: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                )
                {
                    TakingCardsCount = 2,
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhuGeLiang();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: false
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2],
                            aTargetUIDs: ["A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhuGeLiang();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.青龙偃月刀);
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
            }
        }
    }
}
