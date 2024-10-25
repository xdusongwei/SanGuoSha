using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.八卦阵)]
    [Card(CardEffect.藤甲)]
    [Card(CardEffect.仁王盾)]
    [Card(CardEffect.白银狮子)]
    internal class 防具: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(0)]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            aBattlefield.NewEventNode(new EventRecord(aLeader, aLeader, aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var source = aNode.Source!;
            if (source.Dead || aNode.Cards.Length != 1) return aNode;
            var card = aNode.Cards[0];
            if(collector.Pick(aNode.Cards[0]))
            {
                source.LoadArmor(card, aBattlefield);
            }
            return aNode;
        }
    }
}
