using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.顺手牵羊)]
    internal class 顺手牵羊: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(1)]
        [SourceNotInTargets]
        [PlayersDistinct]
        [KitDistance]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //对方要有牌
            if (!aTargets[0].HasCardWithTrialZone) throw new EffectPrepareError();
            bool enable = true;
            foreach (var s in aTargets[0].Skills)
                enable = s.EffectFeasible(aEffect, aTargets[0], enable, aBattlefield);
            if (!enable) throw new EffectPrepareError();
            //添加进子事件队列
            aBattlefield.NewEventNode(new EventRecord(aLeader, aTargets[0], aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var source = aNode.Source!;
            var target = aNode.Target!;
            //玩家自己和对方不能死亡,并且对方有牌
            if (source.Dead || target.Dead || !target.HasCardWithTrialZone) return aNode;
            //无懈可击的过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            //问询玩家选择对方一张牌
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForCards(AskForEnum.顺手牵羊抽牌, source, target);
            var cards = response.Cards;
            if (cards.Length != 0)
            {
                aBattlefield.Move(target, source, cards);
                aBattlefield.CreateActionNode(new ActionNode(response));
            }
            return aNode;
        }
    }
}
