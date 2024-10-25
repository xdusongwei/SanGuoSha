using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class SunQuanCase
    {
        private class WuChief: ChiefBase.BlankChief
        {
            public WuChief(): base()
            {
                ChiefCamp = Camp.吴;
            }
        }

        [Fact]
        public void Basic()
        {   
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.闪),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.闪),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.闪),
                        new Card(9, Card.Suit.Heart, 3, CardEffect.闪),
                        new Card(10, Card.Suit.Heart, 4, CardEffect.闪),
                        new Card(11, Card.Suit.Heart, 3, CardEffect.闪),
                        new Card(12, Card.Suit.Heart, 4, CardEffect.闪),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1, 2, 5, 6, 7, 8],
                            aSkillName: "制衡"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 6
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CSunQuan();
                var playerB = battlefield.Players.Skip(1).First();
                playerA.Weapon = new Card(5, Card.Suit.Club, 1, CardEffect.贯石斧);
                playerA.Armor = new Card(6, Card.Suit.Club, 1, CardEffect.八卦阵);
                playerA.Jia1Ma = new Card(7, Card.Suit.Club, 1, CardEffect.加1马);
                playerA.Jian1Ma = new Card(8, Card.Suit.Club, 1, CardEffect.减1马);
                battlefield.TakingCards(playerA, 2);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(6, playerA.Hands.Count);
                Assert.Null(playerA.Weapon);
                Assert.Null(playerA.Armor);
                Assert.Null(playerA.Jia1Ma);
                Assert.Null(playerA.Jian1Ma);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.桃),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [1],
                            aTargetUIDs: ["A"]
                        ),
                        GameDebug.DoNothing("A"),
                        (aa) => aa.Answer(
                            aUID: "B",
                            aCardIDs: [2]
                        ),
                        GameDebug.FinishLeading("B"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CSunQuan();
                playerA.Health = 1;
                playerA.Role = PlayerRole.Majesty;
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Chief = new WuChief();
                battlefield.TakingCards(playerB, 2);
                battlefield.LogicLoop(playerB, true);
                Assert.Equal(2, playerA.Health);
                Assert.Empty(playerB.Hands);
            }
        }
    }
}
