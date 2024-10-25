using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.无中生有)]
    internal class 无中生有: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(0)]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //添加进子事件队列
            aBattlefield.NewEventNode(new EventRecord(aLeader, aLeader, aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var target = aNode.Target!;
            //玩家不能死亡
            if (target.Dead) return aNode;
            //进入无懈可击的过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            //从牌堆拿出来两张牌
            aBattlefield.TakingCards(target, 2);
            return aNode;
        }
    }
}
