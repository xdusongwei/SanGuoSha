using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class SunShangXiangCase
    {
        [Fact]
        public void WrongArguments()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.丈八蛇矛),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.八卦阵),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1, 2],
                            aTargetUIDs: ["B"],
                            aSkillName: "结姻"
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CSunShangXiang();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Equal(2, playerB.Health);
            }
        }

        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.丈八蛇矛),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.八卦阵),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.加1马),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.减1马),
                        new Card(5, Card.Suit.Spade, 1, CardEffect.丈八蛇矛),
                        new Card(6, Card.Suit.Spade, 2, CardEffect.八卦阵),
                        new Card(7, Card.Suit.Spade, 3, CardEffect.加1马),
                        new Card(8, Card.Suit.Spade, 4, CardEffect.减1马),
                        new Card(9, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(10, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(11, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(12, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(13, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(14, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(15, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(16, Card.Suit.Spade, 1, CardEffect.杀),
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
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [4]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [5]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [6]
                        ),
                         (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [7]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [8]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 8
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CSunShangXiang();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 8);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(8, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.过河拆桥),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.过河拆桥),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.过河拆桥),
                        new Card(4, Card.Suit.Spade, 4, CardEffect.过河拆桥),
                        new Card(5, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(6, Card.Suit.Spade, 2, CardEffect.杀),
                        new Card(7, Card.Suit.Spade, 3, CardEffect.杀),
                        new Card(8, Card.Suit.Spade, 4, CardEffect.杀),
                        new Card(9, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(10, Card.Suit.Spade, 2, CardEffect.杀),
                        new Card(11, Card.Suit.Spade, 3, CardEffect.杀),
                        new Card(12, Card.Suit.Spade, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [13]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [14]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [15]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [4],
                            aTargetUIDs: ["B"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [16]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CSunShangXiang();
                playerB.Weapon = new Card(13, Card.Suit.Heart, 1, CardEffect.诸葛连弩);
                playerB.Armor = new Card(14, Card.Suit.Heart, 1, CardEffect.八卦阵);
                playerB.Jia1Ma = new Card(15, Card.Suit.Heart, 1, CardEffect.加1马);
                playerB.Jian1Ma = new Card(16, Card.Suit.Heart, 1, CardEffect.减1马);
                battlefield.TakingCards(playerA, 4);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(8, playerB.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.丈八蛇矛),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.八卦阵),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1, 2],
                            aTargetUIDs: ["B"],
                            aSkillName: "结姻"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CSunShangXiang();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Health = 1;
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(2, playerB.Health);
            }
        }
    }
}
