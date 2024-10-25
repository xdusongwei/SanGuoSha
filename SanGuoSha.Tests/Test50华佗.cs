using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class HuaTuoCase
    {
        [Fact]
        public void WrongArguments()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.八卦阵),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.决斗),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3],
                            aTargetUIDs: ["A", "B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.DoNothing("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aSkillName: "急救"
                        ),
                        GameDebug.DoNothing("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuaTuo();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Health = 1;
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                Assert.Throws<ContestFinished>(() => battlefield.LogicLoop(playerA, true));
                Assert.NotNull(playerA.Armor);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.八卦阵),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aSkillName: "急救"
                        ),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 1
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuaTuo();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Equal(1, playerA.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.八卦阵),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aSkillName: "青囊"
                        ),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 1
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuaTuo();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Equal(1, playerA.Health);
            }
        }

        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.八卦阵),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.决斗),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3],
                            aTargetUIDs: ["A", "B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.DoNothing("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aSkillName: "急救"
                        ),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuaTuo();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Health = 1;
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Null(playerA.Armor);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.八卦阵),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["A"],
                            aSkillName: "青囊"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 1
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuaTuo();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(2, playerA.Health);
            }
        }
    }
}
