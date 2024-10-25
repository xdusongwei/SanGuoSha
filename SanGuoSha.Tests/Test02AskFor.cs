using Xunit;
using SanGuoSha.BaseClass;
using SanGuoSha.Battlefield;


namespace SanGuoSha.Tests
{
    public class AskForCase
    {
        [Fact]
        public void BasicAskFor()
        {
            var playerA = new Player("a")
            {
                Role = PlayerRole.Majesty
            };
            var playerB = new Player("b")
            {
                Role = PlayerRole.Insurgent
            };
            var players = new Players();
            players.AddPlayer(playerA);
            players.AddPlayer(playerB);
            var battlefield = new Battlefield.Battlefield()
            {
                Players = players,
                AnswerTimeout = 30,
            };
            battlefield.CardsHeap.AddCards([
                new Card(1, Card.Suit.Heart, 1, CardEffect.杀),
                new Card(2, Card.Suit.Heart, 2, CardEffect.杀),
                new Card(3, Card.Suit.Heart, 3, CardEffect.杀),
                new Card(4, Card.Suit.Heart, 4, CardEffect.杀),
            ]);
            {
                var start = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                using var aa = battlefield.NewAsk();
                var response = aa.AskForCards(AskForEnum.Aggressive, playerA);
                Assert.True(response.TimeOut);
                var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                Assert.InRange(end - start, 25, 999);
            }
            {
                var start = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                using var aa = battlefield.NewAsk();
                var response = aa.AskForYN(AskForEnum.雌雄双股剑弃牌, playerA);
                Assert.True(response.TimeOut);
                Assert.False(response.YN);
                var end = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                Assert.InRange(end - start, 25, 999);
            }
        }

        [Fact]
        public void BasicAskForStackProtect()
        {
            var battlefield = new Battlefield.Battlefield();
            var stack = battlefield.AskStack;
            var itemA = new Tuple<string, AskForEnum>("A", AskForEnum.杀);
            stack.Push(itemA);
            var itemB = new Tuple<string, AskForEnum>("B", AskForEnum.杀);
            var itemC = new Tuple<string, AskForEnum>("A", AskForEnum.杀);
            Assert.False(stack.Contains(itemB));
            Assert.True(stack.Contains(itemC));
        }
    }
}
