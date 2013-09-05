/*
 *  GlobalData
 *  Namespace SGS.ServerCore.Contest.Data
 *  游戏动态控制数据
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    public class GlobalData
    {
        /// <summary>
        /// 指示该轮武将是否可以进入拿牌阶段
        /// </summary>
        public bool Take = true;
        /// <summary>
        /// 指示进入回合的武将是否能进入出牌阶段
        /// </summary>
        public bool Lead = true;
        /// <summary>
        /// 指示进入回合的武将是否能进入弃牌阶段
        /// </summary>
        public bool Abandonment = true;
        /// <summary>
        /// 游戏的对象
        /// </summary>
        public GlobalEvent Game;
        /// <summary>
        /// 指示进入回合的玩家能不能无限使用‘杀’
        /// </summary>
        public bool ShaNoLimit = false;

        /// <summary>
        /// 将事件内的经常改动的数据复位操作
        /// </summary>
        public void Reset()
        {
            //EnableArmor = true;
            MaxKillTarget = 1;
        }

        ///// <summary>
        ///// 指示在一些动作中能否使用对方的防具方法
        ///// </summary>
        //public bool EnableArmor = true;
        /// <summary>
        /// 剩余可以杀的次数
        /// </summary>
        public int KillRemain = 1;
        /// <summary>
        /// 进入回合的武将杀的目标数量最大值
        /// </summary>
        public int MaxKillTarget = 1;
        /// <summary>
        /// 进入回合的武将对象
        /// </summary>
        public volatile ChiefBase Active = null;
        /// <summary>
        /// 进入回合武将的新判定区堆栈
        /// </summary>
        public Stack<Card> stkNewBuff = new Stack<Card>();
        /// <summary>
        /// 进入回合的武将在拿牌阶段可以拿的牌数量
        /// </summary>
        public int TakeCardsCount = 2;

        /// <summary>
        /// 当前武将的执行阶段
        /// </summary>
        public volatile string ChiefStatus = "turnStart";
    }
}
