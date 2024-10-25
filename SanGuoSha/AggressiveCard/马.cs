using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.加1马)]
    [Card(CardEffect.减1马)]
    internal class 马: ICardEffectBase
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
                switch (aNode.Effect)
                {
                    case CardEffect.加1马:
                        source.LoadJia1(card, aBattlefield);
                        break;
                    case CardEffect.减1马:
                        source.LoadJian1(card, aBattlefield);
                        break;
                }
            }
            return aNode;
        }
    }
}
