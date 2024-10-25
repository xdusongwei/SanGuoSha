

namespace SanGuoSha.BaseClass
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AnswerMetaAttribute(AskForEnum aType) : Attribute
    {
        public readonly AskForEnum Type = aType;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisableWeaponAttribute : Attribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisableArmorAttribute : Attribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisableSkillAttribute : Attribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ChooseTargetCardsAttribute(int aCardCount, bool aEnableEmpty, bool aAsManyAsPossible, bool aCardsInPriate = false) : Attribute
    {
        public readonly int CardCount = aCardCount;

        public readonly bool EnableEmpty = aEnableEmpty;

        public readonly bool AsManyAsPossible = aAsManyAsPossible;

        public readonly bool CardsInPrivate = aCardsInPriate;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ChooseMyselfCardsAttribute(int aCardCount = 1, bool aEnableEmpty = true, bool aAsManyAsPossible = false) : Attribute
    {
        public readonly int CardCount = aCardCount;

        public readonly bool EnableEmpty = aEnableEmpty;

        public readonly bool AsManyAsPossible = aAsManyAsPossible;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EffectFilterAttribute(params CardEffect[] aEffects) : Attribute
    {
        public CardEffect[] Effects = aEffects;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisableAutoEffectAttribute : Attribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CardsFromAttribute(bool aHand = true, bool aWeapon = false, bool aArmor = false, bool aHorse = false, bool aTrial = false, string? aSlot = null) : Attribute
    {
        public readonly bool Hand = aHand;

        public readonly bool Weapon = aWeapon;

        public readonly bool Armor = aArmor;

        public readonly bool Horse = aHorse;

        public readonly bool Trial = aTrial;

        public readonly string? Slot = aSlot;

        public CardsFromAttribute(): this(false) {}
    }
}