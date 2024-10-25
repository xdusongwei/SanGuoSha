using System.Reflection;
using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase, ICollectorData
    {
        public Battlefield()
        {
            Players = new Players();
            Slots.Slots.Add(new CardSlot(WGFDSlotName, true, false));

            var cardProcs = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttributes<CardAttribute>(false).Any() && typeof(ICardEffectBase).IsAssignableFrom(t));
            foreach (var type in cardProcs)
                foreach(var attr in type.GetCustomAttributes<CardAttribute>(false))
                    AggressiveCards.Add(attr.Type, type);

            var aggressiveSkillCheck = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttributes<AggressiveSkillAttribute>(false).Any());
            foreach (var type in aggressiveSkillCheck)
                foreach(var attr in type.GetCustomAttributes<AggressiveSkillAttribute>(false))
                    AggressiveSkillCheck.Add(attr.SkillName, type);

            var transformSkillCheck = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttributes<TransformSkillAttribute>(false).Any());
            foreach (var type in transformSkillCheck)
                foreach(var attr in type.GetCustomAttributes<TransformSkillAttribute>(false))
                    TransformSkillCheck.Add(attr.SkillName, type);

            var trialProcs = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttributes<CardAttribute>(false).Any() && typeof(ITrialProcBase).IsAssignableFrom(t));
            foreach (var type in trialProcs)
                foreach(var attr in type.GetCustomAttributes<CardAttribute>(false))
                    TrialProcs.Add(attr.Type, type);

            var answerProcs = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttributes<AnswerMetaAttribute>(false).Any());
            foreach (var type in answerProcs)
                foreach(var attr in type.GetCustomAttributes<AnswerMetaAttribute>(false))
                    AnswerCheck.Add(attr.Type, type);

            var chiefs = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttributes<CardAttribute>(false).Any() && typeof(ChiefBase).IsAssignableFrom(t));
        }

        public CollectorData CollectorData
        {
            get;
        } = new();

        /// <summary>
        /// 游戏事件所需的牌槽容器
        /// </summary>
        public override CardSlotContainer Slots
        {
            get;
            set;
        } = new();

        /// <summary>
        /// 获取或设置牌堆
        /// </summary>
        public override CardHeap CardsHeap
        {
            get;
            set;
        } = new CardHeap();

        public int MaxTurns
        {
            get;
            set;
        } = 1000;

        public int TakingCardsCount
        {
            get;
            set;
        } = 2;

        /// <summary>
        /// 开启逻辑循环
        /// </summary>
        /// <param name="aPlayerStart">设置首个进入回合的玩家</param>
        /// <param name="aIgnoreTakeCards">是否忽略一开始对玩家每人发4张牌的过程</param>
        public void LogicLoop(PlayerBase aPlayerStart, bool aIgnoreTakeCards)
        {
            var target = aPlayerStart;
            var loop = aPlayerStart;
            do
            {
                foreach (var s in loop.Skills)
                    s.OnCreate(loop);
            } while (Players.NextAliveUntilNullOrStop(ref loop, target));

            if (!aIgnoreTakeCards)
            {
                loop = aPlayerStart;
                //轮询给每个武将4张牌
                do
                {
                    TakingCards(loop, 4);
                } while (Players.NextAliveUntilNullOrStop(ref loop, target));
            }
            RefereeProc();
            var actionPlayer = target;
            var remainTurns = MaxTurns;
            //游戏的轮询
            do
            {
                
                //复位游戏规则控制数据
                ActionPlayerData = new ActionPlayerData{
                    CurrentPlayer = actionPlayer,
                    TakeCardsCount = TakingCardsCount,
                };
                //若武将有武器,尝试让该武将的武器配置玩家的某些进攻性属性
                if (actionPlayer.WeaponEffect != CardEffect.None)
                    WeaponProc.ActiveWeapon(actionPlayer.WeaponEffect, this);
                //改变武将状态-回合开始
                {
                    ActionPlayerData.PlayerStage = PlayerStageEnum.TurnStart;
                    CreateActionNode(new ActionNode(actionPlayer, ActionPlayerData.PlayerStage.ToString()));
                    using var collector = NewCollector();
                    //通知该武将的技能该武将进入回合开始阶段
                    foreach (var s in actionPlayer.Skills)
                        s.BeforeTurnStart(actionPlayer, this);
                    //事件结束
                    ClearSlotProc();
                }
                //改变武将状态-判定
                {
                    ActionPlayerData.PlayerStage = PlayerStageEnum.Trial;
                    CreateActionNode(new ActionNode(actionPlayer, ActionPlayerData.PlayerStage.ToString()));
                    using var collector = NewCollector();
                    //执行武将判定区的判定
                    Trial(actionPlayer);
                    //事件结束
                    ClearSlotProc();
                }
                foreach (var s in actionPlayer.Skills)
                    s.BeforeTakeCards(actionPlayer, this);
                if (ActionPlayerData.Take)
                {
                    //改变武将状态-拿牌
                    {
                        ActionPlayerData.PlayerStage = PlayerStageEnum.TakingCards;
                        CreateActionNode(new ActionNode(actionPlayer, ActionPlayerData.PlayerStage.ToString()));
                        foreach (var s in actionPlayer.Skills)
                            s.TakingCards(actionPlayer, this);
                        //从牌堆取 TakeCardsCount 张牌
                        TakingCards(actionPlayer, ActionPlayerData.TakeCardsCount);
                        //事件结束
                        ClearSlotProc();
                    }
                }
                //武将状态-出牌
                //要求 允许出牌且玩家未死亡
                if (ActionPlayerData.Lead && !actionPlayer.Dead)
                {
                    //改变武将状态-出牌
                    ActionPlayerData.PlayerStage = PlayerStageEnum.Leading;
                    CreateActionNode(new ActionNode(actionPlayer, ActionPlayerData.PlayerStage.ToString()));
                    foreach (var s in actionPlayer.Skills)
                        s.BeforeLeading(actionPlayer, this);
                    //这里是一个循环,不断问询玩家出牌,若玩家Effect为None,就跳过这个阶段
                    while (!actionPlayer.Dead)
                    {
                        using var collector = NewCollector();
                        //重置全局数据中的活动部分
                        ActionPlayerData.Reset();

                        //开始问询
                        try
                        {
                            using var aa = NewAsk();
                            var r = aa.AskForCards(AskForEnum.Aggressive, actionPlayer);
                            //是否跳过该阶段
                            if (r.Skill == null && r.Effect == CardEffect.None)
                                break;
                            //出牌进行处理,并反馈是否符合规则
                            if (!LeadEvent(r))
                            {
                                ClearSlotProc();
                                break;
                            }
                            else
                            {
                                ClearSlotProc();
                            }
                        }
                        finally
                        {
                            CreateActionNode(new ActionNode(actionPlayer, "CleanTable"));
                        }
                    }
                }

                //在进入弃牌阶段前,通知武将的技能
                foreach (var s in actionPlayer.Skills)
                    s.BeforeAbandonment(actionPlayer, this);

                //允许武将进入弃牌阶段
                if (ActionPlayerData.Abandonment)
                {
                    using var collector = NewCollector();
                    //改变武将状态-弃牌
                    {
                        ActionPlayerData.PlayerStage = PlayerStageEnum.Abandonment;
                        CreateActionNode(new ActionNode(actionPlayer, ActionPlayerData.PlayerStage.ToString()));
                        //玩家弃牌的问询
                        Abandonment(actionPlayer);
                        ClearSlotProc();
                    }
                }
                //玩家回合结束前通知武将的技能
                foreach (var s in actionPlayer.Skills)
                    s.AfterTurnEnd(actionPlayer, this);
                if(remainTurns > 0) remainTurns--;
                if(remainTurns == 0) return;
            } while (Players.NextAliveUntilNull(ref actionPlayer)); //actionPlayer=下一个玩家

            throw new ContestFinished([], [], [.. Players]);
        }

        public override void CreateActionNode(ActionNode aNode)
        {
            ActionLog.Add(aNode);
            CreateActionNodeEvent?.Invoke(this, aNode);
        }
    }
}
