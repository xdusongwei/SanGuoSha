using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.过河拆桥)]
    internal class 过河拆桥: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(1)]
        [SourceNotInTargets]
        [PlayersDistinct]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //对方要有牌
            if (!aTargets[0].HasCardWithTrialZone) throw new EffectPrepareError();
            if(aSkill == null || aSkill.EnableCreateEventNode)
            {
                //添加进子事件队列
                aBattlefield.NewEventNode(new EventRecord(aLeader, aTargets[0], aCards, aEffect, aSkill));
            }
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var source = aNode.Source!;
            var target = aNode.Target!;
            //双方不能有任何一方,并且对方有牌可以选择死亡
            if (source.Dead || target.Dead || !target.HasCardWithTrialZone) return aNode;
            //进入无懈可击的过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            //问询玩家选择对方一张牌
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForCards(AskForEnum.过河拆桥抽牌, source, target);
            var cards = response.Cards;
            if (cards.Length != 0)
            {
                //将对方的这张牌加入到弃置到打牌堆中
                using var collector = aBattlefield.NewCollector();
                collector.DropCards(target, cards);
                aBattlefield.CreateActionNode(new ActionNode(response));
            }
            return aNode;
        }
    }
}
