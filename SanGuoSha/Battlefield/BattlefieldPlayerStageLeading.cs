using System.Reflection;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase
    {
        /// <summary>
        /// 玩家出牌阶段的处理方法
        /// </summary>
        /// <param name="aAnswer">问询玩家产生的结果</param>
        /// <returns>如果出牌合法,那么将返回true</returns>
        protected bool LeadEvent(AskForResult aAnswer)
        {
            var effect = aAnswer.Effect;
            using var collector = NewCollector();

            //根据牌的效果确定游戏玩法
            //并将事件安置到子事件队列
            if(aAnswer.Skill != null)
            {
                if(AggressiveSkillCheck.TryGetValue(aAnswer.Skill.SkillName, out Type? value))
                {
                    var noError = CheckLeadingAnswer(value, aAnswer);
                    if(!noError) goto FAILED;
                }
                if(!aAnswer.Skill.OnPrepareAggressive(aAnswer, this)) return false;
                if(aAnswer.Skill.AutoCollectAggressiveResponse) collector.DropPlayerReponse(aAnswer);
                CreateActionNode(new ActionNode(aAnswer));
                aAnswer.Skill.OnAggressive(aAnswer, this);
            }
            else if(AggressiveCards.TryGetValue(effect, out Type? value))
            {
                var noError = CheckLeadingAnswer(value, aAnswer);
                if(noError) 
                    collector.DropPlayerReponse(aAnswer);
                else
                    goto FAILED;
                CreateActionNode(new ActionNode(aAnswer));
                foreach (var s in aAnswer.Leader.Skills)
                {
                    s.AggressiveUsingEffect(aAnswer.Leader, effect, this);
                    s.OnUseEffect(aAnswer.Leader, effect, this);
                }
            }
            else
            {
                return false;
            }
            //处理子事件
            while (EventNodeSize() != 0)
            {
                EventProc();
            }
            //清除事件队列
            ClearEventNodes();
            //执行成功
            return true;
        FAILED:
            //清除事件队列
            ClearEventNodes();
            //执行成功
            return false;
        }

        public override bool CheckLeadingAnswer(Type aType, AskForResult aAnswer)
        {
            var leader = aAnswer.Leader;
            var targets = aAnswer.Targets;
            var cards = aAnswer.Cards;
            try
            {
                var method = aType.GetMethod("Prepare");
                var needCardsAttribute = method == null ? aType.GetCustomAttribute<NeedCardsAttribute>() : method.GetCustomAttribute<NeedCardsAttribute>();
                if (needCardsAttribute != null)
                {
                    if(needCardsAttribute.Count > 0 && cards.Length != needCardsAttribute.Count) throw new EffectPrepareError();
                    if(needCardsAttribute.Count < 0 && cards.Length < 1) throw new EffectPrepareError();
                    if(needCardsAttribute.Count == 0 && cards.Length != 0) throw new EffectPrepareError();
                }
                var needTargetsAttribute = method == null ? aType.GetCustomAttribute<NeedTargetsAttribute>() : method.GetCustomAttribute<NeedTargetsAttribute>();
                if (needTargetsAttribute != null)
                {
                    if(needTargetsAttribute.Count > 0 && targets.Length != needTargetsAttribute.Count) throw new EffectPrepareError();
                    if(needTargetsAttribute.Count < 0 && targets.Length < 1) throw new EffectPrepareError();
                    if(needTargetsAttribute.Count == 0 && targets.Length != 0) throw new EffectPrepareError();
                }
                var needSourcePlayerAttribute = method == null ? aType.GetCustomAttribute<NeedSourcePlayerAttribute>() : method.GetCustomAttribute<NeedSourcePlayerAttribute>();
                if (needSourcePlayerAttribute != null)
                {
                    if(leader == null) throw new EffectPrepareError();
                }
                var playersDistinctAttribute = method == null ? aType.GetCustomAttribute<PlayersDistinctAttribute>() : method.GetCustomAttribute<PlayersDistinctAttribute>();
                if (playersDistinctAttribute != null)
                {
                    if(targets.Length != targets.Distinct().Count() || (leader != null && targets.Contains(leader))) throw new EffectPrepareError();
                }
                var targetDistinctAttribute = method == null ? aType.GetCustomAttribute<TargetDistinctAttribute>() : method.GetCustomAttribute<TargetDistinctAttribute>();
                if (targetDistinctAttribute != null)
                {
                    if(targets.Length != targets.Distinct().Count()) throw new EffectPrepareError();
                }
                var sourceNotInTargetsAttribute = method == null ? aType.GetCustomAttribute<SourceNotInTargetsAttribute>() : method.GetCustomAttribute<SourceNotInTargetsAttribute>();
                if (sourceNotInTargetsAttribute != null)
                {
                    if(targets.Any(i => i == leader)) throw new EffectPrepareError();
                }
                var sourceInTargetsAttribute = method == null ? aType.GetCustomAttribute<SourceInTargetsAttribute>() : method.GetCustomAttribute<SourceInTargetsAttribute>();
                if (sourceInTargetsAttribute != null)
                {
                    if(!targets.Any(i => i == leader)) throw new EffectPrepareError();
                }
                var noOneDeadAttribute = method == null ? aType.GetCustomAttribute<NoOneDeadAttribute>() : method.GetCustomAttribute<NoOneDeadAttribute>();
                if (noOneDeadAttribute != null)
                {
                    if(targets.Any(i => i.Dead) || (leader != null && leader.Dead)) throw new EffectPrepareError();
                }
                var kitDistanceAttribute = method == null ? aType.GetCustomAttribute<KitDistanceAttribute>() : method.GetCustomAttribute<KitDistanceAttribute>();
                if (kitDistanceAttribute != null)
                {
                    if(leader == null || Players.Any(i => !Players.WithinKitRange(leader!, i, this))) throw new EffectPrepareError();
                }
                if(method != null)
                {
                    var obj = Activator.CreateInstance(aType);
                    var iCardEffect = (obj as ICardEffectBase)!;
                    iCardEffect.Prepare(leader!, targets, aAnswer.Effect, cards, aAnswer.Skill, this);
                }
                return true;
            }
            catch(EffectPrepareError)
            {
                return false;
            }
        }
    }
}
