using System.Text.Json;


namespace SanGuoSha.BaseClass
{
    public class ActionNode
    {
        public readonly PlayerBase Leader;
        public string Action;
        public Card[] Cards;
        public PlayerBase? CardFrom = null;
        public PlayerBase[] Targets;
        public SkillBase? Skill;
        public Card? Weapon;
        public Card? Armor;
        public int Value;
        public bool CardsInPrivate = false;

        public ActionNode(
            PlayerBase aLeader,
            string aAction,
            Card[]? aCards = null,
            PlayerBase[]? aTargets = null,
            SkillBase? aSkill = null,
            Card? aWeapon = null,
            Card? aArmor = null,
            bool aCardsInPrivate = false,
            int aValue = 0
        )
        {
            Leader = aLeader;
            Action = aAction;
            Cards = aCards ?? [];
            Targets = aTargets ?? [];
            Skill = aSkill;
            Weapon = aWeapon;
            Armor = aArmor;
            Value = aValue;
            CardsInPrivate = aCardsInPrivate;
        }

        public ActionNode(AskForResult aAnswer)
        {
            Leader = aAnswer.Leader;
            Action = aAnswer.AskFor == AskForEnum.Aggressive ? aAnswer.Effect.ToString() : aAnswer.AskFor.ToString();
            Cards = aAnswer.Cards ?? [];
            CardFrom = aAnswer.CardFrom;
            Targets = aAnswer.Targets ?? [];
            Skill = aAnswer.Skill;
            Weapon = aAnswer.WeaponEffect != CardEffect.None ? aAnswer.Leader.Weapon : null;
            Armor = aAnswer.ArmorEffect != CardEffect.None ? aAnswer.Leader.Armor : null;
            CardsInPrivate = aAnswer.CardsInPrivate;
        }
    }
}