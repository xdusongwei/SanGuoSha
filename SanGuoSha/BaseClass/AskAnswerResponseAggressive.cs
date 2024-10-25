

namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        private void AskingAggressive(AnswerFunctionArgs aArgs)
        {
            Type? checkType = null;
            var (player, targets, cardIDs, suit, skill, weaponEffect, armorEffect) = 
                ReformatArgs(
                    aUID: aArgs.UID, 
                    aCardIDs: aArgs.CardIDs, 
                    aTargetUIDs: aArgs.TargetUIDs, 
                    aSkillName: aArgs.SkillName, 
                    aSuit: aArgs.Suit, 
                    aWeaponEffect: aArgs.WeaponEffect, 
                    aArmorEffect: aArgs.ArmorEffect
                );
            if(skill != null && IAnswer.AggressiveSkillCheckMap.TryGetValue(skill.SkillName, out checkType))
                (player, targets, cardIDs, suit, skill, weaponEffect, armorEffect) = 
                    ReformatArgs(
                        aUID: aArgs.UID, 
                        aCardIDs: aArgs.CardIDs, 
                        aTargetUIDs: aArgs.TargetUIDs, 
                        aSkillName: aArgs.SkillName, 
                        aSuit: aArgs.Suit, 
                        aWeaponEffect: aArgs.WeaponEffect, 
                        aArmorEffect: aArgs.ArmorEffect,
                        aCheckType: checkType
                    );
            var (hand, weapon, armor, horse, trial, slot) = (true, false, false, false, false, string.Empty);
            if(checkType != null)
                (hand, weapon, armor, horse, trial, slot) = GetCardSourceArgs(checkType);
            var cards = CardsFromIDs(
                aPlayer: player,
                aCardIDs: cardIDs,
                aHand: hand,
                aWeapon: weapon,
                aArmor: armor,
                aHorse: horse,
                aTrial: trial,
                aSlot: slot
            );
            if(skill != null)
                try
                {
                    if(!skill.AnswerCheck(cards, player, targets, AskFor, Battlefield)) throw new ResponseForbiddenError();
                }
                catch(IndexOutOfRangeException)
                {
                    throw new ResponseForbiddenError();
                }
            var effect = cards.Length == 1 ? cards[0].CardEffect : CardEffect.None;
            if(checkType != null) effect = AutoSetEffect(cards, checkType);
            var response = new AskForResult(
                aTimeout: false,
                aAskFor: AskFor,
                aLeader: player,
                aCardFrom: player,
                aTargets: targets,
                aCards: [.. cards],
                aEffect: effect,
                aYN: aArgs.YN,
                aSkill: skill,
                aSuit: suit,
                aWeaponEffect: weaponEffect,
                aArmorEffect: armorEffect
            );
            ReleaseLock(response);
        }
    }
}
