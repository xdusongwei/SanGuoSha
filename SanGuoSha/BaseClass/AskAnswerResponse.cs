using System.Reflection;


namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        private void ReleaseLock(AskForResult aAnswer)
        {
            lock(SemaphoreSlim)
            {
                if(isFinish) return;
                isFinish = true;
                Response = aAnswer;
                SemaphoreSlim.Release();
            }
        }

        private bool AnswerLeaderWeaponArmorSkillCheck(
            string aUID, 
            string? aSkillName = null,
            string? aWeaponEffect = null,
            string? aArmorEffect = null
        )
        {
            var triggerCount = 0;
            if(!string.IsNullOrEmpty(aSkillName)) triggerCount++;
            if(!string.IsNullOrEmpty(aWeaponEffect)) triggerCount++;
            if(!string.IsNullOrEmpty(aArmorEffect)) triggerCount++;
            if(triggerCount > 1)
                throw new MultiTriggerError();
            if(!AskPlayers.Any(i => i.UID == aUID))
                if(ThrowWrongLeader)
                    throw new WrongLeaderError(string.Join(',', AskPlayers.Select(i => i.UID)), aUID, AskFor);
                else
                    return false;
            return true;
        } 

        private Tuple<PlayerBase, PlayerBase[], int[], Card.Suit, SkillBase?, CardEffect, CardEffect> ReformatArgs(
            string aUID, 
            int[]? aCardIDs = null, 
            string[]? aTargetUIDs = null, 
            string? aSkillName = null, 
            string? aSuit = null, 
            string? aWeaponEffect = null,
            string? aArmorEffect = null,
            Type? aCheckType = null
        )
        {
            var cardsSize = (aCardIDs ?? []).Where(i => i != InvisableHandCard).Distinct().Count() + (aCardIDs ?? []).Where(i => i == InvisableHandCard).Count();
            int[] cardIDs = [.. (aCardIDs ?? []).Where(i => i != InvisableHandCard).Distinct().ToArray(), .. (aCardIDs ?? []).Where(i => i == InvisableHandCard).ToArray()];
            if(!EnableCardIdArg && cardIDs.Length > 0) throw new ResponseForbiddenError();
            if(cardIDs.Length != (aCardIDs ?? []).Length) throw new ResponseForbiddenError();
            var player = Battlefield.Players.First(i => i.UID == aUID);
            if(player.Dead) throw new ResponseForbiddenError();
            var targets = (aTargetUIDs ?? []).Select(i => Battlefield.Players.First(j => j.UID == i)).ToArray();
            if(targets.Any(i => i.Dead)) throw new ResponseForbiddenError();
            var skillName = aSkillName ?? string.Empty;
            var suit = string.IsNullOrWhiteSpace(aSuit) ? Card.Suits[Battlefield.GetRandom(Card.Suits.Length)] : (Card.Suit)Enum.Parse(typeof(Card.Suit), aSuit);
            aSkillName = player.Skills.Any(i => i.SkillName == aSkillName) ? aSkillName : string.Empty;
            var skill = player.Skills.FirstOrDefault(i => i!.SkillName == skillName, null);
            var weaponEffect = player.WeaponEffect.ToString() == aWeaponEffect ? player.WeaponEffect : CardEffect.None;
            var armorEffect = player.ArmorEffect.ToString() == aArmorEffect ? player.ArmorEffect : CardEffect.None;
            if(aCheckType != null)
            {
                {
                    var attr = aCheckType.GetCustomAttribute<DisableWeaponAttribute>();
                    if(attr != null && weaponEffect != CardEffect.None) throw new ResponseForbiddenError();
                }
                {
                    var attr = aCheckType.GetCustomAttribute<DisableArmorAttribute>();
                    if(attr != null && armorEffect != CardEffect.None) throw new ResponseForbiddenError();
                }
                {
                    var attr = aCheckType.GetCustomAttribute<DisableSkillAttribute>();
                    if(attr != null && skill != null) throw new ResponseForbiddenError();
                }
            }
            return new Tuple<PlayerBase, PlayerBase[], int[], Card.Suit, SkillBase?, CardEffect, CardEffect>(player, targets, cardIDs, suit, skill, weaponEffect, armorEffect);
        }

        private Tuple<PlayerBase, bool, bool, int, bool> GetCardArgs(Type? aCheckType, PlayerBase aLeader, PlayerBase? aTarget, Type? aSkillType = null)
        {
            var chooseTarget = false;
            var exceptCardCount = 0;
            var enableEmpty = true;
            var asManyAsPossible = false;
            var cardsInPrivate = false;
            foreach(var t in new Type?[]{ aCheckType, aSkillType, })
            {
                if(t == null) continue;
                {
                    var attr = t.GetCustomAttribute<ChooseTargetCardsAttribute>();
                    if(attr != null)
                    {
                        if(Target == null) throw new ResponseForbiddenError();
                        chooseTarget = true;
                        exceptCardCount = attr.CardCount;
                        enableEmpty = attr.EnableEmpty;
                        asManyAsPossible = attr.AsManyAsPossible;
                        cardsInPrivate = attr.CardsInPrivate;
                    }
                }
                {
                    var attr = t.GetCustomAttribute<ChooseMyselfCardsAttribute>();
                    if(attr != null)
                    {
                        chooseTarget = false;
                        exceptCardCount = attr.CardCount;
                        enableEmpty = attr.EnableEmpty;
                        asManyAsPossible = attr.AsManyAsPossible;
                    }
                }
            }
            if(chooseTarget && aTarget == null) throw new ResponseForbiddenError();
            var target = chooseTarget ? aTarget! : aLeader;
            return new Tuple<PlayerBase, bool, bool, int, bool>(target, enableEmpty, asManyAsPossible, exceptCardCount, cardsInPrivate);
        }

        private static Tuple<bool, bool, bool, bool, bool, string> GetCardSourceArgs(Type aCheckType, Type? aSkillType = null)
        {
            foreach(var t in new Type?[]{aSkillType, aCheckType, })
            {
                if( t == null) continue;
                var attr = t.GetCustomAttribute<CardsFromAttribute>();
                if(attr != null)
                    return new Tuple<bool, bool, bool, bool, bool, string>(attr.Hand, attr.Weapon, attr.Armor, attr.Horse, attr.Trial, attr.Slot ?? string.Empty);
            }
            return new Tuple<bool, bool, bool, bool, bool, string>(false, false, false, false, false, string.Empty);
        }

        private static CardEffect AutoSetEffect(Card[] aCards, Type aCheckType)
        {
            var attr = aCheckType.GetCustomAttribute<DisableAutoEffectAttribute>();
            if(attr != null)
                return CardEffect.None;
            return aCards.Length == 1 ? aCards[0].CardEffect : CardEffect.None;
        }

        private Card[] AutoSelectCard(
            int aExceptCardCount, 
            int aAvailableCount, 
            bool aEnableEmpty, 
            PlayerBase aTarget,
            bool aHand = true,
            bool aWeapon = false,
            bool aArmor = false,
            bool aHorse = false,
            bool aTrial = false
        )
        {
            if(aEnableEmpty)
            {
                return [];
            }
            else
            {
                var target = aTarget;
                var autoSelect = new SortedSet<Card>();
                var handCards = new Queue<Card>(Battlefield.ShuffleList(target.Hands));
                var debuffCards = new Queue<Card>(Battlefield.ShuffleArray(target.Debuff.ToArray()));
                int selectMax = int.Min(aExceptCardCount, aAvailableCount);
                for(var i = 0; i < selectMax; i++)
                {
                    if(aHand && handCards.Count > 0)
                        autoSelect.Add(handCards.Dequeue());
                    else if(aWeapon && target.Weapon != null && !autoSelect.Contains(target.Weapon))
                        autoSelect.Add(target.Weapon);
                    else if(aArmor && target.Armor != null && !autoSelect.Contains(target.Armor))
                        autoSelect.Add(target.Armor);
                    else if(aHorse && target.Jia1Ma != null && !autoSelect.Contains(target.Jia1Ma))
                        autoSelect.Add(target.Jia1Ma);
                    else if(aHorse && target.Jian1Ma != null && !autoSelect.Contains(target.Jian1Ma))
                        autoSelect.Add(target.Jian1Ma);
                    else if(aTrial && debuffCards.Count > 0)
                        autoSelect.Add(debuffCards.Dequeue());
                }
                return [.. autoSelect];
            }
        }

        private static bool ShouldAutoSelect(int aExceptCardCount, int aRealCardCount, int aAvailableCount, bool aEnableEmpty, bool aAMAP)
        {
            var needAutoSelect = false;
            if(aExceptCardCount < aRealCardCount) // 出牌数量过大
            {
                needAutoSelect = true;
            }
            else if(aRealCardCount == 0 && !aEnableEmpty) // 不允许出牌为空
                {
                    needAutoSelect = true;
                }
            else if(aExceptCardCount > aRealCardCount) // 出牌数量过小
            {
                if(aRealCardCount == 0 && aEnableEmpty)
                {

                }
                else if(aAMAP && aRealCardCount < aAvailableCount)
                {
                    needAutoSelect = true;
                }
            }
            return needAutoSelect;
        }

        private static int CalcAvailableCardCount(
            PlayerBase aPlayer, 
            bool aHand,
            bool aWeapon,
            bool aArmor,
            bool aHorse,
            bool aTrial
        )
        {
            var availableCount = 0;
            if(aWeapon && aPlayer.Weapon != null)
                availableCount++;
            if(aArmor && aPlayer.Armor != null)
                availableCount++;
            if(aHorse && aPlayer.Jia1Ma != null)
                availableCount++;
            if(aHorse && aPlayer.Jian1Ma != null)
                availableCount++;
            if(aTrial)
                availableCount += aPlayer.Debuff.Count;
            if(aHand)
                availableCount += aPlayer.Hands.Count;
            return availableCount;
        }

        private Card[] CardsFromIDs(
            PlayerBase aPlayer, 
            int[] aCardIDs,
            bool aHand,
            bool aWeapon,
            bool aArmor,
            bool aHorse,
            bool aTrial,
            string? aSlot
        )
        {
            var cards = new List<Card>();
            var chooseTarget = aPlayer == Target;
            var handCards = chooseTarget ? new Queue<Card>(Battlefield.ShuffleList(aPlayer.Hands)) : [];
            foreach(var id in aCardIDs)
            {
                if(aWeapon && aPlayer.Weapon != null && aPlayer.Weapon.ID == id)
                    cards.Add(aPlayer.Weapon);
                else if(aArmor && aPlayer.Armor != null && aPlayer.Armor.ID == id)
                    cards.Add(aPlayer.Armor);
                else if(aHorse && aPlayer.Jia1Ma != null && aPlayer.Jia1Ma.ID == id)
                    cards.Add(aPlayer.Jia1Ma);
                else if(aHorse && aPlayer.Jian1Ma != null && aPlayer.Jian1Ma.ID == id)
                    cards.Add(aPlayer.Jian1Ma);
                else if(aTrial && aPlayer.Debuff.Any(i => i.ID == id))
                    cards.Add(aPlayer.Debuff.First(i => i.ID == id));
                else if(!chooseTarget && aHand && aPlayer.Hands.Any(i => i.ID == id))
                    cards.Add(aPlayer.Hands.First(i => i.ID == id));
                else if(chooseTarget && aHand && id == InvisableHandCard && handCards.Count > 0)
                    cards.Add(handCards.Dequeue());
                else if(!string.IsNullOrEmpty(aSlot) && aPlayer.Slots.HasName(aSlot) && aPlayer.Slots[aSlot].Cards.Any(i => i.ID == id))
                    cards.Add(aPlayer.Slots[aSlot].Cards.First(i => i.ID == id));
                else if(!string.IsNullOrEmpty(aSlot) && Battlefield.Slots.HasName(aSlot) && Battlefield.Slots[aSlot].Cards.Any(i => i.ID == id))
                    cards.Add(Battlefield.Slots[aSlot].Cards.First(i => i.ID == id));
                else
                    throw new ConvertCardMismatch(aPlayer, id);
            }
            return [.. cards];
        }
    }
}