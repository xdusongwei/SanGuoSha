

namespace SanGuoSha.BaseClass
{
    public class CardCollector: IDisposable
    {
        private readonly SortedSet<Card> Cards;

        private bool disposed = false;

        private readonly CollectorData CollectorData;

        private readonly BattlefieldBase Battlefield;

        public CardCollector(ICollectorData aCollectorData, BattlefieldBase aBattlefield)
        {
            CollectorData = aCollectorData.CollectorData;
            Battlefield = aBattlefield;
            Cards = CollectorData.Cards;
            CollectorData.Counter++;
        }

        public Card PopSentenceByCard(PlayerBase aPlayer, CardEffect aEffect)
        {
            var result = PopSentenceCard(aPlayer);
            Battlefield.CreateActionNode(new ActionNode(aLeader: aPlayer, aAction: $"{aEffect}判定", aCards: [result]));
            if(Battlefield.AnswerCooldown > 0)
                Thread.Sleep(Battlefield.AnswerCooldown);
            return result;
        }

        public Card PopSentenceBySkill(PlayerBase aPlayer, SkillBase aSkill)
        {
            var result = PopSentenceCard(aPlayer);
            Battlefield.CreateActionNode(new ActionNode(aLeader: aPlayer, aAction: $"{aSkill.SkillName}判定", aCards: [result]));
            if(Battlefield.AnswerCooldown > 0)
                Thread.Sleep(Battlefield.AnswerCooldown);
            return result;
        }

        private Card PopSentenceCard(PlayerBase aPlayer)
        {
            var card = Battlefield.PopSentenceCard(aPlayer);
            if(ContainsCard(card)) throw new DuplicateCardError(card);
            Cards.Add(card);
            foreach(var s in aPlayer.Skills)
                s.OnSentenceCardTakeEffect(aPlayer, card, Battlefield);
            return card;
        }

        public bool DropPlayerReponse(AskForResult aAnswer)
        {
            if(Battlefield.RemoveCard(aAnswer.Leader, aAnswer.Cards))
            {
                foreach(var c in aAnswer.Cards)
                {
                    if(Cards.Contains(c)) throw new DuplicateCardError(c);
                    Cards.Add(c);
                }
                return true;
            }
            return false;
        }

        public Card[] PopCards(int n)
        {
            var cards = Battlefield.CardsHeap.Pop(n);
            foreach(var c in cards)
            {
                if(Cards.Contains(c)) throw new DuplicateCardError(c);
                Cards.Add(c);
            }
            return cards;
        }

        public void DropCards(PlayerBase aPlayer, Card[] aCards)
        {
            if(Battlefield.RemoveCard(aPlayer, aCards))
                foreach(var c in aCards)
                {
                    if(Cards.Contains(c)) throw new DuplicateCardError(c);
                    Cards.Add(c);
                }
        }

        /// <summary>
        /// 把没有在牌堆\玩家\打牌堆的牌加入进来, 这种场景比较特殊, 此方法慎用.
        /// </summary>
        /// <param name="aPlayer"></param>
        /// <param name="aCard"></param>
        /// <exception cref="DuplicateCardError"></exception>
        public void ForceDropCard(Card aCard)
        {
            if(Cards.Contains(aCard)) throw new DuplicateCardError(aCard);
            Cards.Add(aCard);
        }

        public void DropSlotCards(CardSlot aSlot)
        {
            if (aSlot.Recyclable && aSlot.Cards.Count != 0)
            {
                var cards = aSlot.Cards.Where(aSlot.CardInSlotAndNoOneChoose).ToArray();
                foreach(var c in cards)
                {
                    if(Cards.Contains(c)) throw new DuplicateCardError(c);
                    Cards.Add(c);
                }
            }
            aSlot.Reset();
        }

        public void EquipageReplace(PlayerBase aPlayer, Card? aCard)
        {
            if(aCard == null) return;
            if(ContainsCard(aCard)) throw new DuplicateCardError(aCard);
            if(Battlefield.RemoveCard(aPlayer, [aCard]))
                Cards.Add(aCard);
        }

        public void DropTrialCard(PlayerBase aPlayer, DebuffNode aCard)
        {
            if(ContainsCard(aCard)) throw new DuplicateCardError(aCard);
            if(Battlefield.RemoveCard(aPlayer, [aCard]))
                Cards.Add(aCard.GetOriginalCard());
        }

        public bool Pick(Card aCard)
        {
            if(aCard == null) return false;
            if(!Cards.Contains(aCard)) return false;
            Cards.Remove(aCard);
            return true;
        }

        /// <summary>
        /// 直接将成功捞出打牌堆的牌放入玩家手牌中
        /// </summary>
        /// <param name="aCard"></param>
        /// <param name="aPlayer"></param>
        /// <returns></returns>
        public bool Pick(Card aCard, PlayerBase aPlayer)
        {
            if(aCard == null) return false;
            if(!Cards.Contains(aCard)) return false;
            Cards.Remove(aCard);
            aPlayer.Hands.Add(aCard);
            return true;
        }

        public bool ContainsCard(Card aCard)
        {
            return Cards.Contains(aCard);
        }

        public bool ContainsCards(Card[] aCards)
        {
            if(aCards.Length == 0) return false;
            return !aCards.Any(i => !ContainsCard(i));
        }

        public bool ContainsAnyCards(Card[] aCards)
        {
            if(aCards.Length == 0) return false;
            return aCards.Any(ContainsCard);
        }


        ~CardCollector()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                }
                // 释放非托管资源
                CollectorData.Counter--;
                if(CollectorData.Counter == 0 && Cards.Count != 0)
                {
                    Battlefield.CardsHeap.AddCards([.. Cards]);
                    Cards.Clear();
                }
                disposed = true;
            }
        }
    }
}