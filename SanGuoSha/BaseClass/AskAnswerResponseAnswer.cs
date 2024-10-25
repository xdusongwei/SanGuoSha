using System.Reflection;


namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        private void AskingAnswer(AnswerFunctionArgs aArgs, Type aCheckType)
        {
            var (player, targets, cardIDs, suit, skill, weaponEffect, armorEffect) = 
                ReformatArgs(
                    aUID: aArgs.UID, 
                    aCardIDs: aArgs.CardIDs, 
                    aTargetUIDs: aArgs.TargetUIDs, 
                    aSkillName: aArgs.SkillName, 
                    aSuit: aArgs.Suit, 
                    aWeaponEffect: aArgs.WeaponEffect, 
                    aArmorEffect: aArgs.ArmorEffect,
                    aCheckType: aCheckType
                );
            var pureCardEffect = skill == null && weaponEffect == CardEffect.None && armorEffect == CardEffect.None;
            if(pureCardEffect)
            {
                if(AskFor == AskForEnum.观星)
                {
                    GuanXingAnswer(player, aArgs);
                    return;
                }
                Card[] cards = [];
                var (target, enableEmpty, asManyAsPossible, exceptCardCount, cardsInPrivte) = GetCardArgs(aCheckType, player, Target);
                var (hand, weapon, armor, horse, trial, slot) = GetCardSourceArgs(aCheckType);
                var needAutoSelect = false;
                try
                {
                    cards = CardsFromIDs(
                        aPlayer: target,
                        aCardIDs: cardIDs,
                        aHand: hand,
                        aWeapon: weapon,
                        aArmor: armor,
                        aHorse: horse,
                        aTrial: trial,
                        aSlot: slot
                    );
                }
                catch(ConvertCardMismatch)
                {
                    needAutoSelect = true;
                }
                var availableCount = CalcAvailableCardCount(
                    aPlayer: target,
                    aHand: hand,
                    aWeapon: weapon,
                    aArmor: armor,
                    aHorse: horse,
                    aTrial: trial
                );
                
                if(!needAutoSelect)
                    needAutoSelect = ShouldAutoSelect(exceptCardCount, cards.Length, availableCount, enableEmpty, asManyAsPossible);
                if(needAutoSelect)
                    cards = [.. AutoSelectCard(exceptCardCount, availableCount, enableEmpty, target, hand, weapon, armor, horse, trial)];
                var effect = AutoSetEffect(cards, aCheckType);
                {
                    var attr = aCheckType.GetCustomAttribute<EffectFilterAttribute>();
                    if(attr != null && effect != CardEffect.None)
                        if(!attr.Effects.Any(i => i == effect)) throw new ResponseForbiddenError();
                }
                var response = new AskForResult(
                    aTimeout: false,
                    aAskFor: AskFor,
                    aLeader: player,
                    aCardFrom: target,
                    aTargets: targets,
                    aCards: [.. cards],
                    aEffect: effect,
                    aYN: aArgs.YN,
                    aSkill: skill,
                    aSuit: suit,
                    aWeaponEffect: weaponEffect,
                    aArmorEffect: armorEffect,
                    aCardsInPrivate: cardsInPrivte
                );
                ReleaseLock(response);
            }
            else if(weaponEffect != CardEffect.None)
            {
                try
                {
                    var (target, enableEmpty, asManyAsPossible, exceptCardCount, cardsInPrivte) = GetCardArgs(aCheckType, player, Target);
                    var (hand, weapon, armor, horse, trial, slot) = GetCardSourceArgs(aCheckType);
                    var cards = CardsFromIDs(
                        aPlayer: target,
                        aCardIDs: cardIDs,
                        aHand: hand,
                        aWeapon: weapon,
                        aArmor: armor,
                        aHorse: horse,
                        aTrial: trial,
                        aSlot: slot
                    );
                    var response = new AskForResult(
                        aTimeout: false,
                        aAskFor: AskFor,
                        aLeader: player,
                        aCardFrom: target,
                        aTargets: targets,
                        aCards: [.. cards],
                        aYN: aArgs.YN,
                        aWeaponEffect: weaponEffect,
                        aCardsInPrivate: cardsInPrivte
                    );
                    ReleaseLock(response);
                }
                catch(ConvertCardMismatch)
                {
                    throw;
                }
            }
            else if(skill != null)
            {
                try
                {
                    var (target, enableEmpty, asManyAsPossible, exceptCardCount, cardsInPrivte) = GetCardArgs(aCheckType, player, Target);
                    var (hand, weapon, armor, horse, trial, slot) = GetCardSourceArgs(aCheckType);
                    if(IAnswer.TransformSkillCheckMap.TryGetValue(skill.SkillName, out Type? aSkillCheckType))
                    {
                        (target, enableEmpty, asManyAsPossible, exceptCardCount, cardsInPrivte) = GetCardArgs(aCheckType, player, Target, aSkillCheckType);
                        (hand, weapon, armor, horse, trial, slot) = GetCardSourceArgs(aCheckType, aSkillCheckType);
                    }
                    var cards = CardsFromIDs(
                        aPlayer: target,
                        aCardIDs: cardIDs,
                        aHand: hand,
                        aWeapon: weapon,
                        aArmor: armor,
                        aHorse: horse,
                        aTrial: trial,
                        aSlot: slot
                    );
                    if(!skill.AnswerCheck(cards, player, targets, AskFor, Battlefield)) throw new ResponseForbiddenError();
                    var response = new AskForResult(
                        aTimeout: false,
                        aAskFor: AskFor,
                        aLeader: player,
                        aCardFrom: target,
                        aTargets: targets,
                        aCards: [.. cards],
                        aYN: aArgs.YN,
                        aSkill: skill,
                        aSuit: suit,
                        aCardsInPrivate: cardsInPrivte
                    );
                    ReleaseLock(response);
                }
                catch(ConvertCardMismatch)
                {
                    throw;
                }
            }
        }

        public static readonly string GuanXingSlotName = "观星";

        private void GuanXingAnswer(PlayerBase aPlayer, AnswerFunctionArgs aArgs)
        {
            if(aArgs.GuanXingTop == null || aArgs.GuanXingBottom == null) throw new ResponseForbiddenError();
            if(!aPlayer.Slots.HasName(GuanXingSlotName)) throw new ResponseForbiddenError();
            var cards = aPlayer.Slots[GuanXingSlotName].Cards;
            var hits = new List<int>();
            var top = new List<Card>();
            var bottom = new List<Card>();
            foreach(var cardID in aArgs.GuanXingTop.Distinct())
                if(cards.Any(i => i.ID == cardID))
                {
                    top.Add(cards.First(i => i.ID == cardID));
                    hits.Add(cardID);
                }
                else
                    throw new ResponseForbiddenError();
            foreach(var cardID in aArgs.GuanXingBottom.Distinct())
                if(cards.Any(i => i.ID == cardID))
                {
                    bottom.Add(cards.First(i => i.ID == cardID));
                    hits.Add(cardID);
                }
                else
                    throw new ResponseForbiddenError();
            if(hits.Count != cards.Count) throw new ResponseForbiddenError();
            var response = new AskForResult(
                aTimeout: false,
                aAskFor: AskFor,
                aLeader: aPlayer,
                aCardFrom: aPlayer,
                aGuanXingTop: [.. top],
                aGuanXingBottom: [.. bottom]
            );
            ReleaseLock(response);
        }
    }
}
