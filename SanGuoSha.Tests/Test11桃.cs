using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class TaoCase
    {
        [Fact]
        public void BasicTao()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.桃),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["A"]
                    ),
                    GameDebug.FinishLeading("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            playerA.Health = 1;
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(2, playerA.Health);
        }

        [Fact]
        public void WrongTarget()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.桃),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            playerB.Health = 1;
            battlefield.TakingCards(playerA, 1);
            battlefield.LogicLoop(playerA, true);
            Assert.Equal(1, playerB.Health);
        }

        [Fact]
        public void CryForHelpNoHelp()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.桃),
                    new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                    GameDebug.DoNothing("B"),
                    GameDebug.DoNothing("A"),
                    GameDebug.DoNothing("B"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            playerB.Health = 1;
            battlefield.TakingCards(playerA, 2);
            battlefield.TakingCards(playerB, 1);
            Assert.Throws<ContestFinished>(() => battlefield.LogicLoop(playerA, true));
        }

        [Fact]
        public void CryForHelpSourceHelp()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.桃),
                    new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
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
                    GameDebug.DoNothing("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            playerB.Health = 1;
            battlefield.TakingCards(playerA, 2);
            battlefield.TakingCards(playerB, 1);
            battlefield.LogicLoop(playerA, true);
        }

        [Fact]
        public void CryForHelpPreDefunctHelp()
        {   
            var battlefield = new GameDebug(
                aCards: [
                    new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    new Card(2, Card.Suit.Heart, 2, CardEffect.桃),
                    new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
                ],
                aActions: [
                    (aa) => aa.Answer(
                        aUID: "A",
                        aCardIDs: [1],
                        aTargetUIDs: ["B"]
                    ),
                    GameDebug.DoNothing("B"),
                    GameDebug.DoNothing("A"),
                    (aa) => aa.Answer(
                        aUID: "B",
                        aCardIDs: [3],
                        aTargetUIDs: ["B"]
                    ),
                    GameDebug.DoNothing("A"),
                ],
                aPlayerCount: 2
            );
            var playerA = battlefield.Players.Skip(0).First();
            var playerB = battlefield.Players.Skip(1).First();
            playerB.Health = 1;
            battlefield.TakingCards(playerA, 2);
            battlefield.TakingCards(playerB, 1);
            battlefield.LogicLoop(playerA, true);
        }
    }
}