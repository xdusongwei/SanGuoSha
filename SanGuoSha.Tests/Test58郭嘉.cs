using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class GuoJiaCase
    {
        [Fact]
        public void WrongArguments()
        {
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
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2, 3],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CGuoJia();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(2, playerB.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.桃),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2, 3],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                )
                {
                    IgnoreResponseForbidden = false,
                };
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CGuoJia();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(3, playerB.Hands.Count);
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
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGuoJia();
                playerA.PushTrialCard(new Card(2, Card.Suit.Heart, 2, CardEffect.乐不思蜀), CardEffect.乐不思蜀);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
            }
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
                        GameDebug.Yes("B"),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CGuoJia();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerB.Hands.Count);
            }
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
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CGuoJia();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Single(playerB.Hands);
            }
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
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2, 3],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CGuoJia();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Empty(playerB.Hands);
            }
        }
    }
}
