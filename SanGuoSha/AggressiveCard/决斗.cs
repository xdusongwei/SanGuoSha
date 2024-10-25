using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.决斗)]
    internal class 决斗: ICardEffectBase
    {
        [NeedSourcePlayer]
        [NoOneDead]
        [TargetDistinct]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            if(aTargets.Length != 2) throw new EffectPrepareError();
            if(aSkill == null && aTargets[1] != aLeader) throw new EffectPrepareError();
            bool enable = true;
            foreach (var s in aTargets[0].Skills)
                enable = s.EffectFeasible(aEffect, aTargets[1], enable, aBattlefield);
            if (!enable) throw new EffectPrepareError();
            //安置到事件子队列
            if(aSkill == null || aSkill.EnableCreateEventNode)
                aBattlefield.NewEventNode(new EventRecord(aLeader , aTargets[0], aTargets[1], aCards, aEffect, aSkill));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var source = aNode.Target2!;
            var target = aNode.Target!;
            //玩家双方都不能死亡
            if (source.Dead || target.Dead) return aNode;
            //无懈可击的过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            while (true)
            {
                int times = 1;
                foreach (var s in source.Skills)
                    times = s.CalcAskforTimes(source, target, aNode.Effect, times, aBattlefield);
                for (int i = 0; i < times; i++)
                {
                    //先询问对方要出杀
                    using var aa = aBattlefield.NewAsk();
                    var response = aa.AskForCards(AskForEnum.杀, target);
                    if (response.Effect != CardEffect.杀)
                    {
                        //不出杀费血
                        sbyte cost = 1;
                        foreach (var s in source.Skills)
                            cost = s.CalcDamage(source, aNode.Effect, cost, aBattlefield);
                        if (target.Armor != null)
                            cost = ArmorProc.CalcDamage(cost, aNode.Effect, response.Cards, target.Armor.CardEffect);
                        aBattlefield.DamageHealth(target, cost, source, aNode);
                        return aNode;
                    }
                    else
                    {
                        //出杀加入子事件节点
                        collector.DropPlayerReponse(response);
                        aBattlefield.CreateActionNode(new ActionNode(response));
                        foreach (var s in target.Skills)
                            s.OnUseEffect(target, CardEffect.杀, aBattlefield);
                    }
                }
                (source, target) = (target, source);
            }
        }
    }
}
