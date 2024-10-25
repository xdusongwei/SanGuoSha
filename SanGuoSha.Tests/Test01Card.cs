using Xunit;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Tests
{
    public class CardCase
    {
        [Fact]
        public void PopOneCard()
        {
            using var heap = new CardHeap();
            var card = CardHeap.Heart;
            card.ToString();
            Assert.Throws<NoMoreCard>(heap.Pop);
            heap.AddCard(card);
            Assert.True(heap.Exist(CardHeap.Heart));
            Assert.False(heap.Exist(CardHeap.Diamond));
            Assert.Equal(card, heap.Pop());
        }

        [Fact]
        public void PopMoreCard()
        {
            using var heap = new CardHeap();
            heap.AddCards([CardHeap.Spade, CardHeap.Heart, ]);
            Assert.Throws<NoMoreCard>(() => heap.Pop(3));
            var cards = heap.Pop(2);
            Assert.Equal(2, cards.Length);
            Assert.Contains(cards, i => i == CardHeap.Spade);
            Assert.Contains(cards, i => i == CardHeap.Heart);

            heap.AddCards(cards);
            cards = heap.Pop(1);
            Assert.Single(cards);
            Assert.Contains(cards, i => i == CardHeap.Spade || i == CardHeap.Heart);
        }

        [Fact]
        public void InitCardPackage()
        {
            using var heap = new CardHeap();
            heap.FillOriginCards();
            heap.FillExCards();
            heap.FillShenCards();
            heap.FillCards();
        }
    }
}