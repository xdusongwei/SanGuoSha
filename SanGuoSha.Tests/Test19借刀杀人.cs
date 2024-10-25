using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class JieDaoShaRenCase
    {
        [Fact]
        public void WrongArguments()
        {   
            { // 目标参数不全
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerB.Hands);
            }
            { // 不能借自己的武器
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["A", "B"]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerB.Hands);
            }
            { // 对方没有武器
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerB.Hands);
            }
            { // 对方装备武器但是目标参数错误
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "B"]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.丈八蛇矛);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerB.Hands);
            }
            { // 本人装备武器, 但是不能借自己的武器
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["A", "B"]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.丈八蛇矛);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerB.Hands);
            }
            { // 对方装备武器, 但是距离不够
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["C", "A"]
                        ),
                    ],
                    aPlayerCount: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerC = battlefield.Players.Skip(2).First();
                playerC.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerC, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerC.Hands);
            }
        }

        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Equal(2, playerA.Health);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Equal(1, playerA.Health);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Equal(2, playerA.Health);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.借刀杀人),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "A"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩);
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Single(playerB.Hands);
                Assert.Equal(2, playerA.Health);
                Assert.Equal(2, playerB.Health);
                Assert.True(playerA.Hands.Exists(i => i.ID == 4));
            }
        }
    }
}
