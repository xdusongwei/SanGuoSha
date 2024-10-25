using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class GuanYuCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1],
                            aSkillName: "武圣"
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGuanYu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.闪),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.南蛮入侵),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1],
                            aSkillName: "武圣"
                        ),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGuanYu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.TakingCards(playerB, 1);
                battlefield.LogicLoop(playerB, true);
                Assert.Empty(playerA.Hands);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1],
                            aSkillName: "武圣"
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGuanYu();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B", "C", "D"],
                            aCardIDs: [1],
                            aSkillName: "武圣"
                        ),
                        GameDebug.DoNothing("B"),
                        GameDebug.DoNothing("C"),
                        GameDebug.DoNothing("D"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 4
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CGuanYu();
                playerA.Weapon = new Card(2, Card.Suit.Heart, 2, CardEffect.方天画戟);
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Empty(playerA.Hands);
                Assert.Equal(1, playerB.Health);
            }
        }
    }
}
