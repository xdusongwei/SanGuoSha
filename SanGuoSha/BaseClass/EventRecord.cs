

namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 创建事件节点
    /// </summary>
    /// <param name="aSource">事件源</param>
    /// <param name="aTarget">事件目标</param>
    /// <param name="aTarget2">事件的第二个目标</param>
    /// <param name="aCards">源出的牌</param>
    /// <param name="aEffect">事件的效果</param>
    /// <param name="aSkill">发动的技能</param>
    public class EventRecord(
        PlayerBase? aSource = null, 
        PlayerBase? aTarget = null, 
        PlayerBase? aTarget2 = null, 
        Card[]? aCards = null, 
        CardEffect aEffect = CardEffect.None, 
        SkillBase? aSkill = null,
        bool aIgnoreWuXie = false
    )
    {
        /// <summary>
        /// 事件的源
        /// </summary>
        public PlayerBase? Source = aSource;
        /// <summary>
        /// 事件的目标
        /// </summary>
        public PlayerBase? Target = aTarget;
        /// <summary>
        /// 事件的第二个目标
        /// </summary>
        public PlayerBase? Target2 = aTarget2;
        //事件使用的牌
        public Card[] Cards = aCards ?? [];
        //事件产生的效果
        public CardEffect Effect = aEffect;

        public SkillBase? Skill = aSkill;

        public readonly bool IgnoreWuXie = aIgnoreWuXie;
        

        /// <summary>
        /// 创建事件的节点
        /// </summary>
        /// <param name="aSource">事件源</param>
        /// <param name="aTarget">事件目标</param>
        /// <param name="aCards">源出的牌</param>
        /// <param name="aEffect">事件的效果</param>
        /// <param name="aSkill">所发动的技能</param>
        public EventRecord(
            PlayerBase? aSource, 
            PlayerBase? aTarget, 
            Card[] aCards, 
            CardEffect aEffect, 
            SkillBase? aSkill
        )
            : this(aSource, aTarget, null, aCards, aEffect, aSkill)
        {

        }

        public override string ToString()
        {
            return $"<ER {Source}=>[{Target},{Target2}] Cards:[{string.Join(',', Cards.Select(i => i.ToString()))}], Effect:{Effect}, Skill:{Skill}>";
        }
    }
}
