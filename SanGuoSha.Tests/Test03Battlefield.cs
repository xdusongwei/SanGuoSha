using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Battlefield;


namespace SanGuoSha.Tests
{
    public class BattlefieldCase
    {
        [Fact]
        public void NoMoreCardBattlefield()
        {
            var playerA = new Player("a")
            {
                MaxHealth = 3,
                Health = 3,
                Role = PlayerRole.Majesty,
            };
            var playerB = new Player("b")
            {
                MaxHealth = 3,
                Health = 3,
                Role = PlayerRole.Insurgent,
            };
            var players = new Players();
            players.AddPlayer(playerA);
            players.AddPlayer(playerB);
            var battlefield = new Battlefield.Battlefield()
            {
                Mode = GameMode.FiveSTD,
                Players = players,
                AnswerTimeout = 0,
            };
            battlefield.CardsHeap.AddCards([
                new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
            ]);
            Assert.Throws<NoMoreCard>(() => battlefield.LogicLoop(playerA, true));
        }
    }
}