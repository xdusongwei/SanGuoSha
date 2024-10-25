using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class ShanDianCase
    {
        [Fact]
        public void WrongArguments()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪电),
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
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪电),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [],
                            aTargetUIDs: ["A"]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
            }
        }

        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪电),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        GameDebug.FinishLeading("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Debuff);
                Assert.Single(playerB.Debuff);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪电),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        GameDebug.FinishLeading("B"),
                        GameDebug.DoNothing("A"),
                        GameDebug.DoNothing("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                Assert.Throws<ContestFinished>(() => battlefield.LogicLoop(playerA, true));
                Assert.Empty(playerA.Debuff);
                Assert.Empty(playerB.Debuff);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪电),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        GameDebug.FinishLeading("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Debuff);
                Assert.Single(playerB.Debuff);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪电),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪电),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.闪电),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("B"),
                        GameDebug.FinishLeading("A"),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2,
                    aMaxTurns: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Debuff);
                Assert.Single(playerB.Debuff);
            }
        }
    }
}