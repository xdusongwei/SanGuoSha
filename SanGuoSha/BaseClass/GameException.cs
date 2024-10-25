

namespace SanGuoSha.BaseClass
{
    public class SanGuoShaException(string? message) : Exception(message);

    /// <summary>
    /// 不能发牌的异常
    /// </summary>
    public class NoMoreCard : SanGuoShaException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NoMoreCard()
            : base("牌堆没有牌可以给出")
        {
            
        }
    }

    public class EquipageLoadError : SanGuoShaException
    {
        public EquipageLoadError(Card aCard)
            : base($"无法安装{aCard}到装备区")
        {
            
        }
    }

    public class TrialPushError : SanGuoShaException
    {
        public TrialPushError(Card aCard)
            : base($"无法安装{aCard}到判定区")
        {
            
        }
    }

    /// <summary>
    /// 牌技能准备失败的异常。温和的
    /// </summary>
    public class EffectPrepareError : SanGuoShaException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EffectPrepareError()
            : base("牌技能准备失败")
        {
            
        }
    }

    /// <summary>
    /// 测试问询严格匹配出牌玩家时产生的异常
    /// </summary>
    public class WrongLeaderError : SanGuoShaException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WrongLeaderError(string aExcept, string aActual, AskForEnum aAskFor)
            : base(string.Format("问询回答的玩家不正确, 预期{0}, 实际{1}, 问询:{2}", aExcept, aActual, aAskFor))
        {
            
        }
    }

    /// <summary>
    /// 测试问询严格匹配出牌玩家时产生的异常
    /// </summary>
    public class MultiTriggerError : SanGuoShaException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiTriggerError()
            : base("问询的回应同时出现了多个技能/武器/装备的触发要求")
        {
            
        }
    }

    public class ConvertCardMismatch : SanGuoShaException
    {
        public ConvertCardMismatch(PlayerBase aPlayer, int aID)
            : base($"在{aPlayer}找不到{aID}的牌对象")
        {
            
        }
    }

    public class ResponseForbiddenError : SanGuoShaException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseForbiddenError()
            : base("问询回应的参数被禁止")
        {
            
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public class ContestFinished : SanGuoShaException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ContestFinished(PlayerBase[] aWinners , PlayerBase[] aLosers ,PlayerBase[] aDraws)
            : base("游戏结束")
        {
            Winners = aWinners;
            Losers = aLosers;
            Draws = aDraws;
        }

        public PlayerBase[] Winners
        {
            get;
            private set;
        }

        public PlayerBase[] Losers
        {
            get;
            private set;
        }

        public PlayerBase[] Draws
        {
            get;
            private set;
        }
    }

    public class DuplicateCardError: SanGuoShaException
    {
        public DuplicateCardError(Card aCard)
            : base($"{aCard}在牌收集器中存在")
        {
            
        }
    }

    public class EffectWithoutProcError: SanGuoShaException
    {
        public EffectWithoutProcError(CardEffect aEffect)
            : base($"无法在出牌阶段处理{aEffect}效果")
        {
            
        }
    }
}
