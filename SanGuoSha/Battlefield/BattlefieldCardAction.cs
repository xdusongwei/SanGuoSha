using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase
    {
        public override CardCollector NewCollector() => new(this, this);

        public override Card[] TakingCards(PlayerBase aPlayer, int n)
        {
            if (n <= 0) return [];
            if (aPlayer.Dead) return [];
            Card[] ret = CardsHeap.Pop(n);
            aPlayer.Hands.AddRange(ret);
            CreateActionNode(new ActionNode(
                aLeader: aPlayer,
                aAction: "TakingCards",
                aCards: ret,
                aValue: n,
                aCardsInPrivate: true
            ));
            return ret;
        }

        public override Card PopSentenceCard(PlayerBase aPlayer)
        {
            var c = CardsHeap.Pop(); //取一张牌
            var t = aPlayer; //现在从判定牌持有的玩家开始,轮询武将OnChiefSentenceCardShow_Turn方法
            do
            {
                if(t == null) break;
                foreach (var s in t.Skills)
                    c = s.OnSentenceCardShow(aPlayer, c, t, this);
            }
            while (Players.NextAliveUntilNullOrStop(ref t, aPlayer));
            //最后这里设定判定牌的性质是考虑之前判定牌不确定的因素
            return c;
        }

        /// <summary>
        /// 移除玩家的手牌,如果不能全部移除将不会改变玩家的手牌
        /// 注意,方法成功之后移除的手牌并不会放入弃牌堆
        /// </summary>
        /// <param name="aPlayer">玩家</param>
        /// <param name="aCards">需要移除的手牌</param>
        /// <returns>移除正常返回true</returns>
        private bool RemoveHand(PlayerBase aPlayer, Card[] aCards)
        {
            foreach (var c in aCards)
            {
                if (!aPlayer.Hands.Contains(c)) return false;
            }
            List<Card> old = [.. aPlayer.Hands];
            foreach (var c in aCards)
            {
                if (!aPlayer.RemoveHand(c))
                {
                    aPlayer.Hands = old;
                    return false;
                }
            }
            return true;
        }

        public override bool RemoveCard(PlayerBase aPlayer, Card[] aCards)
        {
            if(aPlayer == null) return false;
            if(aCards.Length == 0) return false;
            foreach (var c in aCards)
            {
                if(!aPlayer.HasCardsInHandOrEquipage(aCards) && !aPlayer.Debuff.Contains(c))
                    return false;
            }
            foreach (var c in aCards)
            {
                if (!aPlayer.RemoveHand(c) && !aPlayer.RemoveDebuff(c))
                {
                    if (aPlayer.Weapon != null && aPlayer.Weapon.IsSame(c))
                    {
                        aPlayer.UnloadWeapon(this);
                        foreach (var s in aPlayer.Skills)
                            s.DropEquipage(aPlayer, this);
                    }
                    else if (aPlayer.Armor != null && aPlayer.Armor.IsSame(c))
                    {
                        aPlayer.UnloadArmor(this);
                        foreach (var s in aPlayer.Skills)
                            s.DropEquipage(aPlayer, this);
                    }
                    else if (aPlayer.Jia1Ma != null && aPlayer.Jia1Ma.IsSame(c))
                    {
                        aPlayer.UnloadJia1(this);
                        foreach (var s in aPlayer.Skills)
                            s.DropEquipage(aPlayer, this);
                    }
                    else if (aPlayer.Jian1Ma != null && aPlayer.Jian1Ma.IsSame(c))
                    {
                        aPlayer.UnloadJian1(this);
                        foreach (var s in aPlayer.Skills)
                            s.DropEquipage(aPlayer, this);
                    }
                }
            }
            foreach(var s in aPlayer.Skills)
                s.OnRemoveCards(aPlayer, this);
            return true;
        }

        public override bool Move(PlayerBase aFrom, PlayerBase aTo, Card[] aCards)
        {
            if (aFrom.Dead || aTo.Dead) return false;
            if (aFrom == aTo) return false;
            if(RemoveCard(aFrom, aCards))
            {
                foreach (var c in aCards)
                    aTo.Hands.Add(c);
                return true;
            }
            return false;
        }
    }
}