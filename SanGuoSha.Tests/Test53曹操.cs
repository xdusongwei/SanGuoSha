using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class CaoCaoCase
    {
        private class WeiChief: ChiefBase.BlankChief
        {
            public WeiChief(): base()
            {
                ChiefCamp = Camp.魏;
            }
        }

        [Fact]
        public void WrongArguments()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.DoNothing("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: true,
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("B")
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CCaoCao();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new WeiChief();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Single(playerA.Hands);
                Assert.Empty(playerB.Hands);
            }
        }

        [Fact]
        public void Basic()
        {  
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.Yes("A"),
                        GameDebug.FinishLeading("B")
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CCaoCao();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new WeiChief();
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Single(playerA.Hands);
                Assert.Empty(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                    ],
                    aActions: [
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1],
                            aTargetUIDs: ["A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aSkillName: "护驾"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("B")
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CCaoCao();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new WeiChief();
                playerA.Role = PlayerRole.Majesty;
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Empty(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.万箭齐发),
                    ],
                    aActions: [
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1]
                        ),
                        GameDebug.DoNothing("C"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aSkillName: "护驾"
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.DoNothing("C"),
                        GameDebug.DoNothing("B"),
                        GameDebug.DoNothing("C"),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("B")
                    ],
                    aPlayerCount: 3,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CCaoCao();
                playerA.Health = 1;
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new WeiChief();
                var playerC = battlefield.Players.Skip(2).First();
                playerC.Chief = new WeiChief();
                playerC.Role = PlayerRole.Majesty;
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.True(playerA.Dead);
                Assert.Equal(1, playerC.Health);
            }
        }
    }
}
