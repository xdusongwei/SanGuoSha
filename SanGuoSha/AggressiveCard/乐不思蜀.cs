using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.乐不思蜀)]
    internal class 乐不思蜀: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [SourceNotInTargets]
        [NeedTargets(1)]
        [PlayersDistinct]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //目标不能有乐不思蜀debuff
            if (aTargets[0].HasDebuff(aEffect)) throw new EffectPrepareError();
            bool enable = true;
            foreach (var s in aTargets[0].Skills)
                enable = s.EffectFeasible(aEffect, aTargets[0], enable, aBattlefield);
            if (!enable) throw new EffectPrepareError();
            //添加进子事件队列
            aBattlefield.NewEventNode(new EventRecord(aLeader, aTargets[0], aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var target = aNode.Target!;
            //玩家不能是自己并且对方不能死亡
            if (target.Dead) return aNode;
            //对方不能有乐不思蜀的debuff
            if (!target.HasDebuff(aNode.Effect) && aNode.Cards.Length == 1)
            {
                if(collector.Pick(aNode.Cards[0]))
                    target.PushTrialCard(aNode.Cards[0], aNode.Effect);
            }
            return aNode;
        }
    }
}
