using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class ZhangLiaoCase
    {
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
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: true,
                            aTargetUIDs: []
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                )
                {
                    TakingCardsCount = 2
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangLiao();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: true,
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                )
                {
                    TakingCardsCount = 2
                };
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangLiao();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: true,
                            aTargetUIDs: ["B", "C", "D"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangLiao();
                var playerB = battlefield.Players.Skip(1).First();
                var playerC = battlefield.Players.Skip(2).First();
                var playerD = battlefield.Players.Skip(3).First();
                battlefield.TakingCards(playerB, 1);
                battlefield.TakingCards(playerC, 1);
                battlefield.TakingCards(playerD, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Single(playerC.Hands);
                Assert.Single(playerD.Hands);
            }
        }

        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: true,
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangLiao();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Empty(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: true,
                            aTargetUIDs: ["B", "C"]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CZhangLiao();
                var playerB = battlefield.Players.Skip(1).First();
                var playerC = battlefield.Players.Skip(2).First();
                battlefield.TakingCards(playerB, 1);
                battlefield.TakingCards(playerC, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
                Assert.Empty(playerB.Hands);
                Assert.Empty(playerC.Hands);
            }
        }
    }
}
