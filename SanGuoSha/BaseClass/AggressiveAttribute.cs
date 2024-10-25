

namespace SanGuoSha.BaseClass
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CardAttribute(CardEffect aType) : Attribute
    {
        public CardEffect Type = aType;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AggressiveSkillAttribute(string aSkillName) : Attribute
    {
        public string SkillName = aSkillName;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TransformSkillAttribute(string aSkillName) : Attribute
    {
        public string SkillName = aSkillName;
    }

    /// <summary>
    /// 必须有目标
    /// </summary>
    /// <param name="n">限制目标数固定值, -1表示至少1个目标</param>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class NeedTargetsAttribute(int n = -1) : Attribute
    {
        public readonly int Count = n;
    }

    /// <summary>
    /// 牌的数量限制
    /// </summary>
    /// <param name="n"></param>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class NeedCardsAttribute(int n = 1) : Attribute
    {
        public readonly int Count = n;
    }

    /// <summary>
    /// 需要来源玩家
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class NeedSourcePlayerAttribute() : Attribute
    {

    }

    /// <summary>
    /// 玩家不能重复
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class PlayersDistinctAttribute() : Attribute
    {

    }

    /// <summary>
    /// 目标玩家不能重复
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class TargetDistinctAttribute() : Attribute
    {

    }

    /// <summary>
    /// 来源玩家不能在目标玩家中
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SourceNotInTargetsAttribute() : Attribute
    {

    }

    /// <summary>
    /// 来源玩家必须在目标玩家中
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class SourceInTargetsAttribute() : Attribute
    {

    }

    /// <summary>
    /// 来源玩家和目标玩家都不能死亡
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class NoOneDeadAttribute() : Attribute
    {

    }

    /// <summary>
    /// 目标玩家在来源玩家锦囊的距离范围内
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class KitDistanceAttribute() : Attribute
    {

    }
}
