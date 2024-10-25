using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class LiuBeiCase
    {
        [Fact]
        public void WrongArguments()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B", "C"],
                            aSkillName: "仁德"
                        ),
                    ],
                    aPlayerCount: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Single(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1, 3],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerA.Health = 1;
                playerA.Weapon = new Card(3, Card.Suit.Heart, 3, CardEffect.麒麟弓);
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Single(playerA.Hands);
            }
        }

        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerA.Health);
                Assert.Single(playerA.Hands);
                Assert.Single(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Empty(playerA.Hands);
                Assert.Equal(2, playerB.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Empty(playerA.Hands);
                Assert.Equal(3, playerB.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Spade, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1, 2, 3],
                            aTargetUIDs: ["B"],
                            aSkillName: "仁德"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerA.Health = 1;
                battlefield.TakingCards(playerA, 3);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Empty(playerA.Hands);
                Assert.Equal(3, playerB.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["C"],
                            aSkillName: "激将"
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Chief = new CLiuBei();
                playerB.Chief.ChiefCamp = ChiefBase.Camp.蜀;
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Single(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["C"],
                            aSkillName: "激将"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1]
                        ),
                        GameDebug.DoNothing("C"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                var playerC = battlefield.Players.Skip(2).First();
                playerA.Chief = new CLiuBei();
                playerB.Chief.ChiefCamp = ChiefBase.Camp.蜀;
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerC.Health);
                Assert.Empty(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.南蛮入侵),
                    ],
                    aActions: [
                        GameDebug.FinishLeading("A"),
                        GameDebug.FinishLeading("B"),
                        (aa) => aa.Answer(
                            aUID: "C",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aSkillName: "激将"
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("C"),
                    ],
                    aPlayerCount: 3,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                var playerC = battlefield.Players.Skip(2).First();
                playerA.Chief = new CLiuBei();
                playerB.Chief.ChiefCamp = ChiefBase.Camp.蜀;
                battlefield.TakingCards(playerB, 1);
                battlefield.TakingCards(playerC, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Health);
                Assert.Equal(1, playerB.Health);
                Assert.Empty(playerB.Hands);
            }
        }
    }
}
