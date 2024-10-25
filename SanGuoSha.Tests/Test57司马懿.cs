using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class SiMaYiCase
    {
        [Fact]
        public void WrongArguments()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
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
                            aCardIDs: [AskAnswer.InvisableHandCard, 2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CSiMaYi();
                battlefield.TakingCards(playerA, 2);
                playerA.Armor = new Card(2, Card.Suit.Heart, 2, CardEffect.八卦阵);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerA.Hands);
                Assert.NotNull(playerA.Armor);
                Assert.Empty(playerB.Hands);
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
                playerB.Chief = new CSiMaYi();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
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
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CSiMaYi();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerB.Hands);
            }
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
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
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
                playerB.Chief = new CSiMaYi();
                battlefield.TakingCards(playerA, 1);
                playerA.Armor = new Card(2, Card.Suit.Heart, 2, CardEffect.八卦阵);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerB.Hands);
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
                            aCardIDs: [1],
                            aTargetUIDs: ["B"]
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.Yes("B"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: []
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CSiMaYi();
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(1, playerB.Health);
                Assert.Single(playerA.Hands);
                Assert.Empty(playerB.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1]
                        ),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CSiMaYi();
                playerA.PushTrialCard(new Card(3, Card.Suit.Diamond, 3, CardEffect.乐不思蜀), CardEffect.乐不思蜀);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.False(battlefield.ActionPlayerData.Lead);
                Assert.Empty(playerB.Hands);
                Assert.Equal(3, battlefield.CardsHeap.TrashCardCount);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: []
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new CSiMaYi();
                playerA.PushTrialCard(new Card(3, Card.Suit.Diamond, 3, CardEffect.乐不思蜀), CardEffect.乐不思蜀);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.True(battlefield.ActionPlayerData.Lead);
                Assert.Single(playerB.Hands);
                Assert.Equal(2, battlefield.CardsHeap.TrashCardCount);
            }
        }
    }
}
