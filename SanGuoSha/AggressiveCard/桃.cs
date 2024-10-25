using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.桃)]
    internal class 桃: ICardEffectBase
    {
        [NeedSourcePlayer]
        [NoOneDead]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            var player = aLeader;
            //血量不能大于等于体力上限
            if (player.Health == player.MaxHealth) throw new EffectPrepareError();
            //安置到事件子队列
            aBattlefield.NewEventNode(new EventRecord(player, player, aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            aBattlefield.RegainHealth(aNode.Source!, 1);
            return aNode;
        }
    }
}
