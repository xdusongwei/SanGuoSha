using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.闪电)]
    internal class 闪电: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(0)]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //玩家不能有闪电debuff
            if (aLeader.HasDebuff(aEffect)) throw new EffectPrepareError();
            //添加进子事件队列
            aBattlefield.NewEventNode(new EventRecord(aLeader, aLeader, aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var source = aNode.Source!;
            //玩家本人不能死亡
            if (source.Dead) return aNode;
            //本人不能有闪电debuff
            if (!source.HasDebuff(aNode.Effect) && aNode.Cards.Length == 1)
            {
                if(collector.Pick(aNode.Cards[0]))
                    source.PushTrialCard(aNode.Cards[0], aNode.Effect);
            }
            return aNode;
        }
    }
}
