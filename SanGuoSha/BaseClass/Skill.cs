

namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 技能的基类
    /// </summary>
    public abstract class SkillBase
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

        public bool AutoCollectAggressiveResponse = true;
        public bool EnableCreateEventNode = false;

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
        /// <param name="aStatus">新的状态</param>
        protected void SwitchSkillStatus(SkillEnabled aStatus)
        {
            SkillStatus = aStatus;
        }

        /// <summary>
        /// 玩家已创建的事件
        /// </summary>
        /// <param name="aPlayer">相关玩家</param>
        public virtual void OnCreate(PlayerBase aPlayer)
        {

        }

        /// <summary>
        /// 玩家使用了某项技能,并在异步层进行检查
        /// </summary>
        /// <param name="aCards">玩家此时的出牌</param>
        /// <param name="aPlayer">该玩家的玩家</param>
        /// <param name="aTargets">所选的目标玩家</param>
        /// <param name="aAskFor">此时系统对该玩家的问询内容</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        /// <returns>返回true表示技能处理正常或者跳过，false表示技能不能通过检验</returns>
        public virtual bool AnswerCheck(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return false;
        }

        /// <summary>
        /// 在玩家进入摸牌阶段前执行的方法
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void BeforeTakeCards(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            
        }

        /// <summary>
        /// 玩家在摸牌阶段时执行的方法
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void TakingCards(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }


        /// <summary>
        /// 玩家在首次进入出牌阶段时执行的方法,与问询出牌无关
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void BeforeLeading(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 再进入弃牌阶段前执行的方法
        /// </summary>
        /// <param name="aPlayer">需要进入弃牌阶段的玩家对象</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void BeforeAbandonment(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 轮到玩家回合前执行的方法
        /// </summary>
        /// <param name="aPlayer">即将进入自己回合的玩家对象</param>
        /// <param name="aBattlefield">游戏全局对象</param>
        public virtual void BeforeTurnStart(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 玩家回合结束后执行的方法
        /// </summary>
        /// <param name="aPlayer">结束回合的玩家对象</param>
        /// <param name="aBattlefield">全局游戏数据对象</param>
        public virtual void AfterTurnEnd(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 玩家受到‘伤害’时执行的方法,被伤害玩家会被触发该方法.
        /// 如果被害玩家发生濒死, 被救活之后触发此方法.
        /// </summary>
        /// <param name="aSourceEvent">产生伤害的事件节点</param>
        /// <param name="aSource">伤害的来源玩家，若伤害来源非玩家造成，系统置此参数为null</param>
        /// <param name="aTarget">受到伤害的目标</param>
        /// <param name="aBattlefield">全局游戏对象</param>
        /// <param name="aDamage">伤害量</param>
        public virtual void OnPlayerHarmed(EventRecord aSourceEvent, PlayerBase? aSource, PlayerBase aTarget, BattlefieldBase aBattlefield, sbyte aDamage)
        {

        }

        /// <summary>
        /// 某位玩家的判定牌放置到场上，轮询事件
        /// </summary>
        /// <param name="aTrialPlayer">判定牌的玩家所有者</param>
        /// <param name="aSentenceCard">判定牌</param>
        /// <param name="aAskForPlayer">当前被轮询此事件的玩家</param>
        /// <param name="aBattlefield">游戏全局数据对象</param>
        /// <returns>若需要更改判定牌，请将新的牌对象返回，并自行处理好旧的牌</returns>
        public virtual Card OnSentenceCardShow(PlayerBase aTrialPlayer, Card aSentenceCard, PlayerBase aAskForPlayer, BattlefieldBase aBattlefield)
        {
            return aSentenceCard;
        }

        public virtual AskForResult OnNewAnswer(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            return aAnswer;
        }

        public virtual bool OnPrepareAggressive(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            return false;
        }

        public virtual void OnAggressive(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            
        }

        /// <summary>
        /// 当针对某个玩家的判定牌生效后执行的方法
        /// </summary>
        /// <param name="aPlayer">判定牌所属玩家对象</param>
        /// <param name="aCard">判定牌</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void OnSentenceCardTakeEffect(PlayerBase aPlayer, Card aCard, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 玩家使用了某种效果
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="aEffect">效果</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void OnUseEffect(PlayerBase aPlayer, CardEffect aEffect, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 计算需要重复问询的次数
        /// </summary>
        /// <param name="aPlayer">造成伤害来源的玩家对象</param>
        /// <param name="aTarget">计算问询重复次数的目标玩家对象</param>
        /// <param name="aEffect">来源效果</param>
        /// <param name="aOldTimes">原来计算的重复次数</param>
        /// <param name="aBattlefield">游戏全局数据</param>
        /// <returns>重复次数</returns>
        public virtual int CalcAskforTimes(PlayerBase aPlayer, PlayerBase aTarget,  CardEffect aEffect, int aOldTimes , BattlefieldBase aBattlefield)
        {
            return aOldTimes;
        }

        /// <summary>
        /// 计算伤害量
        /// </summary>
        /// <param name="aPlayer">造成伤害的玩家对象</param>
        /// <param name="aEffect">伤害效果</param>
        /// <param name="aDamage">原来的伤害量</param>
        /// <param name="aBattlefield">游戏全局数据</param>
        /// <returns>计算出的伤害量</returns>
        public virtual sbyte CalcDamage(PlayerBase aPlayer, CardEffect aEffect, sbyte aDamage, BattlefieldBase aBattlefield)
        {
            return aDamage;
        }

        /// <summary>
        /// 玩家失去牌，包括其判定区的牌
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="aBattlefield">游戏数据对象</param>
        public virtual void OnRemoveCards(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 问询开始前通知技能，一般用于技能状态调整
        /// </summary>
        /// <param name="aPlayer">被问询的玩家对象</param>
        /// <param name="aAskFor">问询的效果</param>
        /// <param name="aBattlefield">游戏数据</param>
        public virtual void BeforeAskfor(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 问询结束后通知技能，一般用于技能状态改变
        /// </summary>
        /// <param name="aPlayer">被问询的玩家对象</param>
        /// <param name="aAskFor">问询的效果</param>
        /// <param name="aBattlefield">游戏数据</param>
        public virtual void FinishAskfor(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 玩家的武器发生变化
        /// </summary>
        /// <param name="aPlayer">玩家对象</param>
        /// <param name="aWeapon">现在武器区域的对象</param>
        /// <param name="aBattlefield">游戏数据</param>
        public virtual void WeaponUpdated(PlayerBase aPlayer, Card? aWeapon, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 玩家失去装备区的牌
        /// </summary>
        /// <param name="aPlayer">玩家</param>
        /// <param name="aBattlefield">游戏数据</param>
        public virtual void DropEquipage(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {

        }

        public virtual void AggressiveUsingEffect(PlayerBase aPlayer, CardEffect aEffect, BattlefieldBase aBattlefield)
        {

        }

        /// <summary>
        /// 计算玩家杀攻击距离的事件
        /// </summary>
        /// <param name="aPlayer">计算起点玩家</param>
        /// <param name="aOldRange">原来的范围</param>
        /// <param name="aBattlefield">游戏数据</param>
        /// <returns>如果不修改范围大小,返回aOldRange,否则返回设置好的大小</returns>
        public virtual byte CalcShaDistance(PlayerBase aPlayer, byte aOldRange, BattlefieldBase aBattlefield)
        {
            return aOldRange;
        }

        /// <summary>
        /// 计算玩家锦囊使用范围的事件
        /// </summary>
        /// <param name="aPlayer">计算起点玩家</param>
        /// <param name="aOldRange">原来的范围</param>
        /// <param name="aBattlefield">游戏数据</param>
        /// <returns>如果不修改范围大小,返回aOldRange,否则返回设置好的大小</returns>
        public virtual byte CalcKitDistance(PlayerBase aPlayer, byte aOldRange, BattlefieldBase aBattlefield)
        {
            return aOldRange;
        }

        /// <summary>
        /// 判断效果是否能用于当前(目标)玩家
        /// </summary>
        /// <param name="aTarget">目标玩家</param>
        /// <param name="aFeasible">效果可行性状态</param>
        /// <param name="aBattlefield">游戏数据</param>
        /// <returns>若不想影响效果,返回aFeasible原值,返回False表示此效果不能用于目标玩家</returns>
        public virtual bool EffectFeasible(CardEffect aEffect, PlayerBase aTarget, bool aFeasible, BattlefieldBase aBattlefield)
        {
            return aFeasible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPlayer"></param>
        /// <param name="aRescuer"></param>
        /// <param name="aEffect"></param>
        /// <param name="aOldPoint"></param>
        /// <param name="aBattlefield"></param>
        /// <returns></returns>
        public virtual sbyte CalcRescuePoint(PlayerBase aPlayer, PlayerBase aRescuer, CardEffect aEffect, sbyte aOldPoint, BattlefieldBase aBattlefield)
        {
            return aOldPoint;
        }

        /// <summary>
        /// 在处理事件前发生的事件, 将通知事件中目标玩家
        /// </summary>
        /// <param name="aTarget">目标玩家</param>
        /// <param name="aEvent"></param>
        /// <param name="aBattlefield"></param>
        public virtual void BeforeProcessingEvent(PlayerBase aTarget, ref EventRecord aEvent, BattlefieldBase aBattlefield)
        {
            
        }

        public virtual void OnPlayerDead(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            
        }
    }


    public abstract class AdvSkill(string aSkillName, SkillBase.SkillEnabled aEnabled, bool aIsMajestySkill) 
    : SkillBase(aSkillName: aSkillName, aEnabled: aEnabled, aIsMajestySkill: aIsMajestySkill)
    {
        protected int AggressiveMaxTimes = 1;
        private int RemainAggressiveTimes = 1;

        private bool IsAggressiveSkill
        {
            get
            {
                return typeof(IAggressiveStyleSkill).IsAssignableFrom(GetType());
            }
        }

        private bool IsTransformSkill
        {
            get
            {
                return typeof(ITransformStyleSkill).IsAssignableFrom(GetType());
            }
        }

        private ITransformStyleSkill ToTransform()
        {
            ITransformStyleSkill inter = (this as ITransformStyleSkill)!;
            return inter;
        }

        private IAggressiveStyleSkill ToAggressive()
        {
            IAggressiveStyleSkill inter = (this as IAggressiveStyleSkill)!;
            return inter;
        }

        public sealed override void BeforeAskfor(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if(aPlayer.Dead) return;
            if(IsTransformSkill && aAskFor != AskForEnum.Aggressive)
            {
                var inter = ToTransform();
                if (inter.Trigger(aPlayer, aAskFor, aBattlefield))
                    SwitchSkillStatus(SkillEnabled.Enable);
            }
            if(IsAggressiveSkill && aAskFor == AskForEnum.Aggressive)
            {
                SwitchSkillStatus(SkillEnabled.Enable);
            }
        }

        public sealed override bool AnswerCheck(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if(aPlayer.Dead) return false;
            if(IsTransformSkill && aAskFor != AskForEnum.Aggressive)
            {
                var inter = ToTransform();
                if (SkillStatus == SkillEnabled.Disable) return false;
                return inter.Check(aCards, aPlayer, aTargets, aAskFor, aBattlefield);
            }
            if(IsAggressiveSkill && aAskFor == AskForEnum.Aggressive)
            {
                if (SkillStatus == SkillEnabled.Disable) return false;
                var inter = ToAggressive();
                var result = inter.Check(aCards, aPlayer, aTargets, aAskFor, aBattlefield);
                if(result && RemainAggressiveTimes > 0)
                    RemainAggressiveTimes--;
                return result;
            }
            return false;
        }

        public sealed override AskForResult OnNewAnswer(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            if (IsTransformSkill && aAnswer.AskFor != AskForEnum.Aggressive && SkillStatus == SkillEnabled.Enable)
            {
                var inter = ToTransform();
                SwitchSkillStatus(SkillEnabled.Disable);
                var answer = aAnswer.ShallowCopy();
                return inter.Transform(answer, aBattlefield);
            }
            return aAnswer;
        }

        public sealed override void FinishAskfor(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if(IsTransformSkill && aAskFor != AskForEnum.Aggressive)
            {
                if (SkillStatus == SkillEnabled.Enable || aPlayer.Dead)
                    SwitchSkillStatus(SkillEnabled.Disable);
            }
            if(IsAggressiveSkill && aAskFor == AskForEnum.Aggressive)
            {
                BeforeAbandonment(aPlayer, aBattlefield);
            }
        }

        public sealed override void BeforeLeading(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            if(IsAggressiveSkill)
            {
                RemainAggressiveTimes = AggressiveMaxTimes;
            }
        }

        public sealed override bool OnPrepareAggressive(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            if(IsAggressiveSkill && aAnswer.AskFor == AskForEnum.Aggressive)
            {
                var inter = ToAggressive();
                return inter.Prepare(aAnswer, aBattlefield);
            }
            return base.OnPrepareAggressive(aAnswer, aBattlefield);
        }

        public sealed override void OnAggressive(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            if(IsAggressiveSkill && aAnswer.AskFor == AskForEnum.Aggressive)
            {
                if(RemainAggressiveTimes == 0) SwitchSkillStatus(SkillEnabled.Disable);
                var inter = ToAggressive();
                inter.Proc(aAnswer, aBattlefield);
            }
        }

        public sealed override void BeforeAbandonment(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            if(IsAggressiveSkill)
            {
                if (SkillStatus != SkillEnabled.Enable) return;
                if(!aPlayer.Dead && RemainAggressiveTimes != 0) return;
                SwitchSkillStatus(SkillEnabled.Disable);
            }
        }
    }
}
