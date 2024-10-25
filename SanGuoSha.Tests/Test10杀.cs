using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Battlefield;


namespace SanGuoSha.Tests
{
    public class ShaCase
    {
        [Fact]
        public void ShaWithoutLeader()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: string.Empty,
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                ],
                aPlayerCount: 2
            )
            {
                ThrowWrongLeader = false,
                AnswerTimeout = 1,
            };
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(playerB.MaxHealth, playerB.Health);
        }

        [Fact]
        public void ShaOvertimes()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                    GameDebug.DoNothing("B"),
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [2],
                        aTargetUIDs: ["B"]
                    ),
                ],
                aPlayerCount: 2
            )
            {
                ThrowWrongLeader = false,
                AnswerTimeout = 1,
            };
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(1, playerB.Health);
        }

        [Fact]
        public void ShaWithoutTargets()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: []
                    ),
                    // GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(playerB.MaxHealth, playerB.Health);
        }

        [Fact]
        public void ShaTargetsDuplicate()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
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
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(playerB.MaxHealth, playerB.Health);
        }

        [Fact]
        public void ShaTargetsOverlimit()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B", "C"]
                    ),
                ],
                aPlayerCount: 3
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(playerB.MaxHealth, playerB.Health);
        }

        [Fact]
        public void ShaYourself()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["A"]
                    ),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(playerA.MaxHealth, playerA.Health);
        }

        [Fact]
        public void ShaDeadTarget()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                ],
                aPlayerCount: 3
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            playerB.Dead = true;
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(playerB.MaxHealth, playerB.Health);
        }

        [Fact]
        public void BasicShaWithoutShan()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                    GameDebug.DoNothing("B"),
                    GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.TakingCards(playerB, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(1, playerB.Health);
        }

        [Fact]
        public void BasicShaWithShan()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                    (aa) => aa.Answer(
                        aUID: "B",
                        aCardIDs: [2]
                    ),
                    GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            battlefield.TakingCards(playerA, 1);
            battlefield.TakingCards(playerB, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(2, playerB.Health);
        }
    }
}