using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.丈八蛇矛)]
    [Card(CardEffect.诸葛连弩)]
    [Card(CardEffect.古锭刀)]
    [Card(CardEffect.麒麟弓)]
    [Card(CardEffect.贯石斧)]
    [Card(CardEffect.青龙偃月刀)]
    [Card(CardEffect.青钢剑)]
    [Card(CardEffect.雌雄双股剑)]
    [Card(CardEffect.朱雀羽扇)]
    [Card(CardEffect.方天画戟)]
    [Card(CardEffect.寒冰箭)]
    internal class 武器: ICardEffectBase
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
                source.LoadWeapon(card, aBattlefield);
            }
            return aNode;
        }
    }
}
