using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class HanBingJianCase
    {
        [Fact]
        public void WrongArguments()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["D"]
                        ),
                    ],
                    aPlayerCount: 6
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerD = battlefield.Players.Skip(3).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Equal(2, playerD.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                        new Card(5, Card.Suit.Heart, 5, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [AskAnswer.InvisableHandCard, ]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [AskAnswer.InvisableHandCard, AskAnswer.InvisableHandCard, AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerB.Health);
            }
        }

        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["C"]
                        ),
                        GameDebug.DoNothing("C"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 6
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerC = battlefield.Players.Skip(2).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerC.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.减1马),
                        new Card(3, Card.Suit.Heart, 2, CardEffect.杀),
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
                            aCardIDs: [3],
                            aTargetUIDs: ["D"]
                        ),
                        GameDebug.DoNothing("D"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 6
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerD = battlefield.Players.Skip(3).First();
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerD.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [AskAnswer.InvisableHandCard, AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.加1马),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("B"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3, AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerB.Health);
                Assert.Null(playerB.Jia1Ma);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Jia1Ma = new Card(3, Card.Suit.Club, 3, CardEffect.加1马);
                playerB.Jian1Ma = new Card(4, Card.Suit.Club, 4, CardEffect.减1马);
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Null(playerB.Jia1Ma);
                Assert.Null(playerB.Jian1Ma);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.寒冰箭),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("A"),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(3, Card.Suit.Club, 3, CardEffect.丈八蛇矛);
                playerB.Armor = new Card(4, Card.Suit.Club, 4, CardEffect.仁王盾);
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Null(playerB.Weapon);
                Assert.Null(playerB.Armor);
                Assert.Equal(2, playerB.Health);
            }
        }
    }
}