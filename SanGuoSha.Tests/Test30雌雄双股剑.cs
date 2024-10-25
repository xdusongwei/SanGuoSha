using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class CiXiongShuangGuJianCase
    {
        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.雌雄双股剑),
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
                        new Card(1, Card.Suit.Heart, 1, CardEffect.雌雄双股剑),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
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
                        (aa) => aa.Answer(
                            aUID: "A",
                            aYN: false
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A")
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief.Gender = ChiefBase.GenderType.Female;
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.雌雄双股剑),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
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
                        GameDebug.Yes("A"),
                        GameDebug.FinishLeading("A")
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief.Gender = ChiefBase.GenderType.Female;
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.雌雄双股剑),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.桃),
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
                        GameDebug.Yes("A"),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A")
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief.Gender = ChiefBase.GenderType.Female;
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Equal(2, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.雌雄双股剑),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
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
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
                        ),
                        GameDebug.FinishLeading("A")
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief.Gender = ChiefBase.GenderType.Female;
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
                        new Card(1, Card.Suit.Heart, 1, CardEffect.雌雄双股剑),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.桃),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.桃),
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
                        GameDebug.Yes("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [99]
                        ),
                        GameDebug.FinishLeading("A")
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief.Gender = ChiefBase.GenderType.Female;
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 2);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Equal(2, playerB.Health);
            }
        }
    }
}