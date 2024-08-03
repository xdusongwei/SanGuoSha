using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanGuoSha.Contest.Data;
using SanGuoSha.Contest.Data.GameException;
/*
 * 这里算是个开头吧,现在这里说点东西 
 * 这是个三国杀游戏的核心部分,游戏的所有请求处理和反馈信息都在这里完成
 * 而这个类库的功能,就是服务从这里去开启游戏,并通过给定的玩家接口与回调完成各类操作
 * 玩家访问接口中的方法提交请求,系统从玩家的回调中返回XML消息让玩家了解游戏的动作信息
*/
//SanGuoSha.Contest.Global命名空间几乎是用来容纳游戏三大服务层面的类的,例如下面的,最顶层的Game类
namespace SanGuoSha.Contest.Global
{
    /// <summary>
    /// 游戏对象,控制游戏的开始并进入游戏循环中
    /// </summary>
    public sealed class Game : GlobalEvent
    {
        /// <summary>
        /// 创建环境的委托方法
        /// </summary>
        /// <param name="aHeap">牌堆</param>
        /// <param name="aPlayers">玩家集合</param>
        /// <remarks>当调用对象Create方法时,OnCreate事件会发生,此时需要通过委托完成牌堆成分和玩家成分的设定</remarks>
        public delegate void CreateDelegate(CardHeap aHeap, Players aPlayers);

        /// <summary>
        /// 创建环境事件
        /// </summary>
        public event CreateDelegate OnCreate = null;

        /// <summary>
        /// 游戏的构造函数
        /// <param name="aLatency">等待时间,毫秒,不得小于1000的毫秒,也不能超过60秒,否则将按照最接近的可行值设置</param>
        /// <param name="aMode">游戏的模式</param>
        /// <param name="aPacks">游戏使用的扩展包</param>
        /// </summary>
        public Game(int aLatency, GameMode aMode, GamePack[] aPacks)
        {
            WaittingData.Latency = aLatency > 999 ? aLatency : 1000;
            if (aLatency > 60000) WaittingData.Latency = 600000;
            Mode = aMode;
            GamePacks = aPacks;
        }

        /// <summary>
        /// 按照完整的过程开始游戏。
        /// 在使用此方法前，请使用Create方法和OnCreate事件配置相关环境,系统会忽略对牌堆的设置和玩家武将的设置，按照游戏模式设置牌堆,且要求玩家数量必须等于游戏模式中指定的数量
        /// </summary>
        /// <exception cref="ContestFinished">游戏正常结束的异常</exception>
        /// <exception cref="NoMoreCard">牌堆没有牌可用于分配的异常</exception>
        /// <exception cref="Exception">游戏故障异常,将返回堆栈调用信息</exception>
        public void StartGame()
        {
            try
            {
                AsynchronousCore.SendEnvironmentMessage();
                if (base.SelectChiefs())
                    ActiveLogic(false);
            }
            catch (ContestFinished e)
            {
                throw e;
            }
            catch (NoMoreCard e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new Exception(e.StackTrace);
            }
        }

        /// <summary>
        /// 通知对象创建一个运行环境,并触发 OnCreate 事件以开始配置游戏主要参数
        /// </summary>
        public void Create()
        {
            if (OnCreate != null) OnCreate(CardsHeap, GamePlayers);
        }

        /// <summary>
        /// 开启逻辑泵,此后进入处理循环。此方法仅配合Create方法创建自定义环境.必须存在玩家且牌堆有8张以上的牌
        /// </summary>
        /// <param name="aIgnoreTakeCards">是否忽略一开始对玩家每人发4张牌的过程</param>
        /// <remarks>这是一个测试方法,用于测试游戏的逻辑正确,所以,OnCreate事件设置的所有信息不会被忽略,并按照所给定的参数直接进行执行</remarks>
        public void ActiveLogic(bool aIgnoreTakeCards)
        {
            try
            {
                //选择是主公身份的玩家
                Player[] start = GamePlayers.All.Where(c => c.Chief != null & c.Chief.ChiefStatus == ChiefBase.Status.Majesty).ToArray();
                ChiefBase s = null;
                //存在主公身份的玩家,那么设置该玩家是开始进入回合.否则设置第一个没有死亡的玩家
                if (start.Count() == 1 && !start[0].Dead)
                    s = start[0].Chief;
                else
                    s = GamePlayers[-1].Chief;
                if (GamePlayers.All.Count() > 0 && CardsHeap.TotalCards > 8 && s != null)
                    LogicLoop(s, aIgnoreTakeCards);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获得玩家对象的方法
        /// </summary>
        /// <param name="aUID"></param>
        /// <returns></returns>
        public Player GetPlayerByUID(string aUID)
        {
            if (GamePlayers != null)
            {
                Player[] arr = GamePlayers.All.Where(i => i.UID == aUID).ToArray();
                if (arr.Length == 1)
                {
                    return arr[0];
                }
            }
            return null;
        }
    }
}
