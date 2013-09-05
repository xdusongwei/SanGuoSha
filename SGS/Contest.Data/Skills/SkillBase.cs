/*
 * SkillBase ASkill
 * Namespace SGS.ServerCore.Contest.Data
 * 武将技能的定义
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGS.ServerCore.Contest.Global;
using System.Xml.Linq;
using BeaverMarkupLanguage;
namespace SGS.ServerCore.Contest.Data
{
    /// <summary>
    /// 技能的基类
    /// </summary>
    internal abstract class SkillBase :ASkill
    {
        /// <summary>
        /// 构造技能，技能初始状态是不可用,不是主公技
        /// </summary>
        /// <param name="aSkillName">技能名称</param>
        public SkillBase(string aSkillName)
            :this(aSkillName , SkillEnabled.Disable ,false)
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aSkillName">技能名称</param>
        /// <param name="aEnabled">技能初始状态</param>
        /// <param name="aIsMajestySkill">是否是主公技</param>
        public SkillBase(string aSkillName, SkillEnabled aEnabled, bool aIsMajestySkill)
        {
            SkillName = aSkillName;
            SkillStatus = aEnabled;
            IsMajestySkill = aIsMajestySkill;
        }


        /// <summary>
        /// 指示该技能是否是主公技，这是个只读成员
        /// </summary>
        public readonly bool IsMajestySkill;
            

        /// <summary>
        /// 技能的名称
        /// </summary>
        public readonly string SkillName;

        /// <summary>
        /// 技能的状态枚举
        /// </summary>
        public enum SkillEnabled { 
            /// <summary>
            /// 技能不可用
            /// </summary>
            Disable, 
            /// <summary>
            /// 技能可以使用
            /// </summary>
            Enable,
            /// <summary>
            /// 被动的技能
            /// </summary>
            Passive
        };

        /// <summary>
        /// 当前该技能的状态
        /// </summary>
        public SkillEnabled SkillStatus
        {
            get;
            private set;
        }

        /// <summary>
        /// 设置技能状态
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aStatus">新的状态</param>
        /// <param name="aData">全局数据对象</param>
        protected void SwitchSkillStatus(ChiefBase aChief, SkillEnabled aStatus, GlobalData aData)
        {
            SkillStatus = aStatus;
            aData.Game.AsynchronousCore.SendMessage( new ChiefBase[] {aChief} , 
                new Beaver("skill.status", SkillName , SkillStatus.ToString()).ToString()
                //new XElement("skill.status",
                //    new XElement("name" , SkillName ),
                //    new XElement("status" , SkillStatus )
                //    ) 
            , false 
                );
        }
    }

    /// <summary>
    /// 技能方法的抽象类
    /// </summary>
    internal abstract class ASkill
    {
        /// <summary>
        /// 武将已创建的事件
        /// </summary>
        /// <param name="aChief">创建的武将</param>
        public virtual void OnCreate(ChiefBase aChief)
        {

        }

        /// <summary>
        /// 玩家使用了某项技能,并在异步层进行处理，因为异步层不允许进行新的问询，一般用于不会再问询的技能，或者技能的条件检查
        /// </summary>
        /// <param name="aSkillName">玩家使用的技能名称</param>
        /// <param name="aCards">玩家此时的出牌</param>
        /// <param name="aChief">该玩家的武将</param>
        /// <param name="aTargets">所选的目标武将</param>
        /// <param name="aAskFor">此时系统对该武将的问询内容</param>
        /// <param name="aEffect">若技能条件成立，这里可以返回替代的效果</param>
        /// <param name="aData">游戏数据对象</param>
        /// <returns>返回true表示技能处理正常或者跳过，false表示技能不能通过检验</returns>
        public virtual bool ActiveSkill(string aSkillName, Card[] aCards, ChiefBase aChief, ChiefBase[] aTargets, MessageCore.AskForEnum aAskFor ,  ref Card.Effect aEffect, GlobalData aData)
        {
            return true; //技能通过检验
        }

        /// <summary>
        /// 在武将进入摸牌阶段前执行的方法
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void BeforeTakeCards(ChiefBase aChief, GlobalData aData)
        {
            
        }

        /// <summary>
        /// 武将在摸牌阶段时执行的方法
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void TakingCards(ChiefBase aChief, GlobalData aData)
        {

        }


        /// <summary>
        /// 武将在首次进入出牌阶段时执行的方法,与问询出牌无关
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void Leading(ChiefBase aChief, GlobalData aData)
        {

        }

        /// <summary>
        /// 再进入弃牌阶段前执行的方法
        /// </summary>
        /// <param name="aChief">需要进入弃牌阶段的武将对象</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void BeforeAbandonment(ChiefBase aChief, GlobalData aData)
        {

        }

        /// <summary>
        /// 轮到玩家回合前执行的方法
        /// </summary>
        /// <param name="aChief">即将进入自己回合的武将对象</param>
        /// <param name="aData">游戏全局对象</param>
        public virtual void BeforeTurnStart(ChiefBase aChief, GlobalData aData)
        {

        }

        /// <summary>
        /// 玩家回合结束后执行的方法
        /// </summary>
        /// <param name="aChief">结束回合的武将对象</param>
        /// <param name="aData">全局游戏数据对象</param>
        public virtual void AfterTurnEnd(ChiefBase aChief, GlobalData aData)
        {

        }

        /// <summary>
        /// 玩家受到‘伤害’时执行的方法,被伤害武将会被触发该方法
        /// </summary>
        /// <param name="aSourceEvent">产生伤害的事件节点</param>
        /// <param name="aSource">伤害的来源武将，若伤害来源非武将造成，系统置此参数为null</param>
        /// <param name="aTarget">受到伤害的目标</param>
        /// <param name="aData">全局游戏对象</param>
        /// <param name="aDamage">伤害量</param>
        public virtual void OnChiefHarmed(GlobalEvent.EventRecoard aSourceEvent, ChiefBase aSource, ChiefBase aTarget, GlobalData aData, sbyte aDamage)
        {

        }

        /// <summary>
        /// 某位武将的判定牌放置到场上，轮询事件
        /// </summary>
        /// <param name="aJudgeChief">判定牌的武将所有者</param>
        /// <param name="aJudgementCard">判定牌</param>
        /// <param name="aThisChief">当前被轮询此事件的武将</param>
        /// <param name="aData">游戏全局数据对象</param>
        /// <returns>若需要更改判定牌，请将新的牌对象返回，并自行处理好旧的牌</returns>
        public virtual Card OnChiefJudgementCardShow_Turn(ChiefBase aJudgeChief, Card aJudgementCard , ChiefBase aThisChief,GlobalData aData)
        {
            return aJudgementCard;
        }

        /// <summary>
        /// 武将被问询
        /// </summary>
        /// <param name="aChief">被问询的武将</param>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aData">全局游戏数据对象</param>
        /// <returns>成功处理返回effect不是none的askforresult对象,否则返回null,另外根据情形你可以返回两个异常类型</returns>
        public virtual MessageCore.AskForResult AskFor(ChiefBase aChief, MessageCore.AskForEnum aAskFor, GlobalData aData)
        {
            return null;
        }

        /// <summary>
        /// 当针对某个武将的判定牌生效后执行的方法
        /// </summary>
        /// <param name="aChief">判定牌所属武将对象</param>
        /// <param name="aCard">判定牌</param>
        /// <param name="aEnableSendToBin">判定牌是否需要弃置到弃牌堆，通常情况系统置true ,如果不需要，请更改此参数置false</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void OnChiefJudgementCardTakeEffect(ChiefBase aChief, Card aCard, ref bool aEnableSendToBin, GlobalData aData)
        {

        }

        /// <summary>
        /// 武将使用了某种效果
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aEffect">效果</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void OnUseEffect(ChiefBase aChief, Card.Effect aEffect, GlobalData aData)
        {

        }

        /// <summary>
        /// 计算需要重复问询的次数
        /// </summary>
        /// <param name="aChief">造成伤害来源的武将对象</param>
        /// <param name="aTarget">计算问询重复次数的目标武将对象</param>
        /// <param name="aEffect">来源效果</param>
        /// <param name="aOldTimes">原来计算的重复次数</param>
        /// <param name="aData">游戏全局数据</param>
        /// <returns>重复次数</returns>
        public virtual int CalcAskforTimes(ChiefBase aChief, ChiefBase aTarget ,  Card.Effect aEffect, int aOldTimes , GlobalData aData)
        {
            return aOldTimes;
        }

        /// <summary>
        /// 计算伤害量
        /// </summary>
        /// <param name="aChief">造成伤害的武将对象</param>
        /// <param name="aEffect">伤害效果</param>
        /// <param name="aDamage">原来的伤害量</param>
        /// <param name="aData">游戏全局数据</param>
        /// <returns>计算出的伤害量</returns>
        public virtual sbyte CalcDamage(ChiefBase aChief, Card.Effect aEffect, sbyte aDamage, GlobalData aData)
        {
            return aDamage;
        }

        /// <summary>
        /// 武将失去牌，包括其判定区的牌
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aData">游戏数据对象</param>
        public virtual void OnRemoveCards(ChiefBase aChief, GlobalData aData)
        {

        }

        /// <summary>
        /// 问询开始前通知技能，一般用于技能状态调整
        /// </summary>
        /// <param name="aChief">被问询的武将对象</param>
        /// <param name="aEffect">问询的效果</param>
        /// <param name="aData">游戏数据</param>
        public virtual void BeforeAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect, GlobalData aData)
        {

        }

        /// <summary>
        /// 问询结束后通知技能，一般用于技能状态改变
        /// </summary>
        /// <param name="aChief">被问询的武将对象</param>
        /// <param name="aEffect">问询的效果</param>
        /// <param name="aData">游戏数据</param>
        public virtual void FinishAskfor(ChiefBase aChief, MessageCore.AskForEnum aEffect,GlobalData aData)
        {

        }

        /// <summary>
        /// 武将的武器发生变化
        /// </summary>
        /// <param name="aChief">武将对象</param>
        /// <param name="aWeapon">现在武器区域的对象</param>
        /// <param name="aData">游戏数据</param>
        public virtual void WeaponUpdated(ChiefBase aChief, Card aWeapon ,GlobalData aData)
        {

        }

        /// <summary>
        /// 武将失去装备区的牌
        /// </summary>
        /// <param name="aChief">武将</param>
        /// <param name="aData">游戏数据</param>
        public virtual void DropEquipage(ChiefBase aChief, GlobalData aData)
        {

        }

        /// <summary>
        /// 计算玩家杀攻击距离的事件
        /// </summary>
        /// <param name="aChief">计算起点武将</param>
        /// <param name="aOldRange">原来的范围</param>
        /// <param name="aData">游戏数据</param>
        /// <returns>如果不修改范围大小,返回aOldRange,否则返回设置好的大小</returns>
        public virtual byte CalcShaDistance(ChiefBase aChief, byte aOldRange, GlobalData aData)
        {
            return aOldRange;
        }

        /// <summary>
        /// 计算玩家锦囊使用范围的事件
        /// </summary>
        /// <param name="aChief">计算起点武将</param>
        /// <param name="aOldRange">原来的范围</param>
        /// <param name="aData">游戏数据</param>
        /// <returns>如果不修改范围大小,返回aOldRange,否则返回设置好的大小</returns>
        public virtual byte CalcKitDistance(ChiefBase aChief, byte aOldRange, GlobalData aData)
        {
            return aOldRange;
        }

        /// <summary>
        /// 判断效果是否能用于当前(目标)武将
        /// </summary>
        /// <param name="aEffect">效果</param>
        /// <param name="aCards">产生效果的牌</param>
        /// <param name="aTarget">目标武将</param>
        /// <param name="aFeasible">效果可行性状态</param>
        /// <param name="aData">游戏数据</param>
        /// <returns>若不想影响效果,返回aFeasible原值,返回False表示此效果不能用于目标武将</returns>
        public virtual bool EffectFeasible(Card[] aCards , Card.Effect aEffect, ChiefBase aTarget, bool aFeasible , GlobalData aData)
        {
            return aFeasible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aChief"></param>
        /// <param name="aRescuer"></param>
        /// <param name="aEffect"></param>
        /// <param name="aOldPoint"></param>
        /// <param name="aData"></param>
        /// <returns></returns>
        public virtual sbyte CalcRescuePoint(ChiefBase aChief, ChiefBase aRescuer, Card.Effect aEffect, sbyte aOldPoint, GlobalData aData)
        {
            return aOldPoint;
        }

        /// <summary>
        /// 在处理子事件前发生的事件,将通知子事件中目标武将
        /// </summary>
        /// <param name="aTargetChief">目标武将</param>
        /// <param name="aEvent"></param>
        /// <param name="aData"></param>
        public virtual void PreprocessingSubEvent(ChiefBase aTargetChief,ref GlobalEvent.EventRecoard aEvent, GlobalData aData)
        {
            
        }
    }
}
