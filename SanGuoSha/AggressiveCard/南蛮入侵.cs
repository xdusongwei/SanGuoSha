using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.南蛮入侵)]
    internal class 南蛮入侵: ICardEffectBase
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
            //无懈可击的执行过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;

            if (target.Armor == null || ArmorProc.EnableFor(target.Armor.CardEffect, aNode.Cards, aNode.Effect))
            {
                //问询杀
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForCards(AskForEnum.杀, target);
                if (response.Effect == CardEffect.杀)
                {
                    using var collector = aBattlefield.NewCollector();
                    collector.DropPlayerReponse(response);
                    aBattlefield.CreateActionNode(new ActionNode(response));
                    foreach (var s in response.Leader.Skills)
                        s.OnUseEffect(response.Leader, CardEffect.杀, aBattlefield);
                }
                else
                {
                    //没有杀,费血
                    aBattlefield.DamageHealth(target, 1, source, aNode);
                }
            }
            return aNode;
        }
    }
}
