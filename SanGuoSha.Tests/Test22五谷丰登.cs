using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class WuGuFengDengCase
    {
        [Fact]
        public void WrongArguments()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.五谷丰登),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.决斗),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.决斗),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Empty(battlefield.Slots[BattlefieldBase.WGFDSlotName].Cards);
            }
        }

        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.五谷丰登),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.决斗),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.决斗),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
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
                battlefield.LogicLoop(playerA, true);
                Assert.Single(playerA.Hands);
                Assert.Contains(playerA.Hands, i => i.ID == 3);
                Assert.Single(playerB.Hands);
                Assert.Contains(playerB.Hands, i => i.ID == 2);
                Assert.Empty(battlefield.Slots[BattlefieldBase.WGFDSlotName].Cards);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.五谷丰登),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.决斗),
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
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
                Assert.Empty(battlefield.Slots[BattlefieldBase.WGFDSlotName].Cards);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.五谷丰登),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无懈可击),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.决斗),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.决斗),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [3]
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
                Assert.Empty(playerA.Hands);
                Assert.Single(playerB.Hands);
                Assert.Empty(battlefield.Slots[BattlefieldBase.WGFDSlotName].Cards);
                Assert.Equal(3, battlefield.CardsHeap.TrashCardCount);
            }
        }
    }
}