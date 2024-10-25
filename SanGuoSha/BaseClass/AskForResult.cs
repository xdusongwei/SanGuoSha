
namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 问询结果
    /// </summary>
    public class AskForResult(
        bool aTimeout, 
        AskForEnum aAskFor,
        PlayerBase aLeader, 
        PlayerBase aCardFrom,
        PlayerBase[]? aTargets = null, 
        Card[]? aCards = null, 
        CardEffect aEffect = CardEffect.None, 
        bool aYN = false, 
        SkillBase? aSkill = null, 
        ChiefBase[]? aChiefs = null, 
        Card.Suit aSuit = Card.Suit.Spade,
        CardEffect aWeaponEffect = CardEffect.None,
        CardEffect aArmorEffect = CardEffect.None,
        Card[]? aGuanXingTop = null,
        Card[]? aGuanXingBottom = null,
        bool aCardsInPrivate = false
    )
    {
        public readonly bool CardsInPrivate = aCardsInPrivate;
        public readonly AskForEnum AskFor = aAskFor;
        /// <summary>
        /// 事件的产生者
        /// </summary>
        public PlayerBase Leader = aLeader;

        public readonly PlayerBase CardFrom = aCardFrom;

        /// <summary>
        /// 事件的目标
        /// </summary>
        public readonly PlayerBase[] Targets = aTargets ?? [];

        /// <summary>
        /// 事件目标回应的牌
        /// </summary>
        public Card[] Cards = aCards ?? [];

        /// <summary>
        /// 回应的牌的效果
        /// </summary>
        public CardEffect Effect = aEffect;

        /// <summary>
        /// 是否超时
        /// </summary>
        public readonly bool TimeOut = aTimeout;

        /// <summary>
        /// 回复的是否结果
        /// </summary>
        public readonly bool YN = aYN;

        /// <summary>
        /// 回复的武将
        /// </summary>
        public readonly ChiefBase[] Chiefs = aChiefs ?? [];

        /// <summary>
        /// 回复的花色
        /// </summary>
        public readonly Card.Suit Suit = aSuit;

        /// <summary>
        /// 若回应中武将技能参与，这里保存发动的武将技能
        /// </summary>
        public readonly SkillBase? Skill = aSkill;

        public CardEffect WeaponEffect = aWeaponEffect;

        public CardEffect ArmorEffect = aArmorEffect;

        public readonly Card[] GuanXingTop = aGuanXingTop ?? [];

        public readonly Card[] GuanXingBottom = aGuanXingBottom ?? [];

        public void OverwriteResponse(PlayerBase aPlayer, Card[] aCards)
        {
            Leader = aPlayer;
            Cards = aCards;
        }

        public override string ToString()
        {
            return $"{Leader}, Cards:[{string.Join(',', Cards.Select(i => i.ToString()))}], YN: {YN}, Timeout:{TimeOut}";
        }

        public AskForResult ShallowCopy()
        {
            return (AskForResult)MemberwiseClone();
        }
    }
}
