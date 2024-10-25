using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class DaQiaoCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Diamond, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Diamond, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.闪),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"],
                            aSkillName: "国色"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["C"],
                            aSkillName: "国色"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 3,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CDaQiao();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Diamond, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Diamond, 2, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2],
                            aTargetUIDs: ["C"]
                        ),
                        GameDebug.DoNothing("C"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                var playerC = battlefield.Players.Skip(2).First();
                playerB.Chief = new CDaQiao();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerC.Health);
            }
        }
    }
}
