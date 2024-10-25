using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class GanNingCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.无中生有),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.无中生有),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"],
                            aSkillName: "奇袭"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [3]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [2],
                            aTargetUIDs: ["B"],
                            aSkillName: "奇袭"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [4]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGanNing();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Weapon = new Card(4, Card.Suit.Heart, 4, CardEffect.诸葛连弩);
                playerB.Armor = new Card(3, Card.Suit.Heart, 3, CardEffect.八卦阵);
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Null(playerB.Weapon);
                Assert.Null(playerB.Armor);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Spade, 1, CardEffect.无中生有),
                        new Card(2, Card.Suit.Spade, 2, CardEffect.无中生有),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aTargetUIDs: ["B"],
                            aSkillName: "奇袭"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [AskAnswer.InvisableHandCard]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGanNing();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Empty(playerB.Hands);
            }
        }
    }
}
