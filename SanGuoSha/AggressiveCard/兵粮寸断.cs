using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.兵粮寸断)]
    internal class 兵粮寸断: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [SourceNotInTargets]
        [NeedTargets(1)]
        [PlayersDistinct]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            if (aTargets[0].HasDebuff(aEffect)) throw new EffectPrepareError();
            aBattlefield.NewEventNode(new EventRecord(aLeader, aTargets[0], aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var target = aNode.Target!;
            //玩家不能是自己并且对方不能死亡
            if (target.Dead) return aNode;
            if (!target.HasDebuff(aNode.Effect) && aNode.Cards.Length == 1)
            {
                if(collector.Pick(aNode.Cards[0]))
                    target.PushTrialCard(aNode.Cards[0], aNode.Effect);
            }
            return aNode;
        }
    }
}
