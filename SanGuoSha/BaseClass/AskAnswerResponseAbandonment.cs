

namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        private void AskingAbandonment(AnswerFunctionArgs aArgs)
        {
            var (player, _, cardIDs, _, _, _, _) = 
                ReformatArgs(
                    aUID: aArgs.UID, 
                    aCardIDs: aArgs.CardIDs, 
                    aTargetUIDs: aArgs.TargetUIDs, 
                    aSkillName: aArgs.SkillName, 
                    aSuit: aArgs.Suit, 
                    aWeaponEffect: aArgs.WeaponEffect, 
                    aArmorEffect: aArgs.ArmorEffect
                );
            var cards = CardsFromIDs(
                aPlayer: player,
                aCardIDs: cardIDs,
                aHand: true,
                aWeapon: false,
                aArmor: false,
                aHorse: false,
                aTrial: false,
                aSlot: null
            );
            var response = new AskForResult(
                aTimeout: false,
                aAskFor: AskFor,
                aLeader: player,
                aCardFrom: player,
                aCards: cards
            );
            ReleaseLock(response);
        }
    }
}
