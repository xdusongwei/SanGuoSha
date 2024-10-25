using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.万箭齐发)]
    internal class 万箭齐发: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(0)]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //下面是把出牌玩家以后的其他玩家依次装入子事件列表中
            var player = aLeader;
            while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, aLeader))
            {
                aBattlefield.NewEventNode(new EventRecord(aLeader, player, aCards, aEffect, aSkill));
            }
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var source = aNode.Source!;
            var target = aNode.Target!;
            //对方不能死亡
            if (target.Dead) return aNode;
            //进入无懈可击的过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            if (target.Armor == null || ArmorProc.EnableFor(target.Armor.CardEffect, aNode.Cards, aNode.Effect))
            {
                var askEffect = CardEffect.闪;
                //问询闪
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForCards(AskForEnum.闪, target, true);
                if (response.Effect == askEffect)
                {
                    using var collector = aBattlefield.NewCollector();
                    collector.DropPlayerReponse(response);
                    aBattlefield.CreateActionNode(new ActionNode(response));
                    foreach (var s in response.Leader.Skills)
                        s.OnUseEffect(response.Leader, askEffect, aBattlefield);
                }
                else
                {
                    //没有出闪费血
                    aBattlefield.DamageHealth(target, 1, source, aNode);
                }
            }
            return aNode;
        }
    }
}
