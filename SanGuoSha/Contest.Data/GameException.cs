using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanGuoSha.Contest.Data.GameException
{
    /// <summary>
    /// 不能发牌的异常
    /// </summary>
    public class NoMoreCard : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NoMoreCard(Players aPlayers)
            : base("牌堆没有牌可以给出")
        {
            GamePlayers = aPlayers;
        }

        public Players GamePlayers
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public class ContestFinished : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ContestFinished(string[] aWinnerUIDs , string[] aLoserUIDs ,string[] aDrawUIDs)
            : base("游戏结束")
        {
            Winners = aWinnerUIDs;
            Losers = aLoserUIDs;
            Draws = aDrawUIDs;
        }

        public string[] Winners
        {
            get;
            private set;
        }

        public string[] Losers
        {
            get;
            private set;
        }

        public string[] Draws
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 技能触发无效.无关的技能被调用或者之前的技能逻辑处理正常需要跳出时抛出。温和的
    /// </summary>
    public class SkillInvalid : Exception
    {
        /// <summary>
        /// 构造异常
        /// </summary>
        public SkillInvalid()
            : base("技能触发无效")
        {

        }
    }

    /// <summary>
    /// 技能触发的致命错误，通常用于用户错误的触发。严重的
    /// </summary>
    public class SkillFatalError : Exception
    {
        /// <summary>
        /// 构造异常
        /// </summary>
        public SkillFatalError()
            : base("技能逻辑错误")
        {

        }
    }
}
