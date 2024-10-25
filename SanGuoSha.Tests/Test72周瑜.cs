using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class ZhouYuCase
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
                    ],
                    aActions: [
                        GameDebug.FinishLeading("A"),
                    ],
                    aInitHealth: 3
                )
                {
                    TakingCardsCount = 2,
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhouYu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(3, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Diamond, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aSkillName: "反间"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aSuit: "Diamond"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ]
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhouYu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerB.Health);
                Assert.Empty(playerA.Hands);
                Assert.Single(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Diamond, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aSkillName: "反间"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aSuit: "Heart"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ]
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhouYu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Empty(playerA.Hands);
                Assert.Single(playerB.Hands);
            }
        }
    }
}
