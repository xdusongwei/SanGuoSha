using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Chief;


namespace SanGuoSha.Tests
{
    public class HuangYueYingCase
    {
        [Fact]
        public void Basic()
        {
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.无中生有),
                        new Card(2, Card.Suit.Heart, 2, CardEffect.无中生有),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.无中生有),
                        new Card(4, Card.Suit.Heart, 4, CardEffect.无中生有),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aCardIDs: [1]
                        ),
                        GameDebug.FinishLeading("A"),
                    ],
                    aPlayerCount: 2,
                    aInitHealth: 3
                );
                var playerA = battlefield.Players.Skip(0).First();
                playerA.Chief = new CHuangYueYing();
                var playerB = battlefield.Players.Skip(1).First();
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(3, playerA.Hands.Count);
            }
            {
                var battlefield = new GameDebug(
                    aCards: [
                        new Card(1, Card.Suit.Heart, 1, CardEffect.顺手牵羊),
                        new Card(3, Card.Suit.Heart, 3, CardEffect.无中生有),
                    ],
                    aActions: [
                        (aa) => aa.Answer(
                            aUID: "A",
                            aTargetUIDs: ["B"],
                            aCardIDs: [1]
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
                playerA.Chief = new CHuangYueYing();
                var playerB = battlefield.Players.Skip(1).First();
                playerB.Jia1Ma = new Card(2, Card.Suit.Heart, 2, CardEffect.加1马);
                battlefield.TakingCards(playerA, 1);
                battlefield.LogicLoop(playerA, true);
                Assert.Equal(2, playerA.Hands.Count);
            }
        }
    }
}
