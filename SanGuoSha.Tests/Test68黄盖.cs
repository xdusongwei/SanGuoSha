using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class HuangGaiCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.无中生有),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无中生有),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aSkillName: "苦肉"
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 10
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuangGai();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(9, playerA.Health);
                Assert.Equal(2, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.顺手牵羊),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无中生有),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aSkillName: "苦肉"
                        ),
                        (aa) => aa.Answer(
                            aUID: "A",
                            aSkillName: "苦肉"
                        ),
                        GameDebug.DoNothing("A"),
                        GameDebug.DoNothing("B"),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuangGai();
                var playerB = battlefield.Players.Skip(1).First();
                Assert.Throws<ContestFinished>(() => battlefield.LogicLoop(playerA, true));
                Assert.Empty(playerA.Hands);
            }
        }
    }
}
