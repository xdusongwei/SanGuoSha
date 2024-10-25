using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.借刀杀人)]
    internal class 借刀杀人: 杀, ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [TargetDistinct]
        [NeedTargets(2)]
        [NeedCards(1)]
        public new void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //第一个目标不能是自己
            if (aTargets[0] == aLeader) throw new EffectPrepareError();
            //第一个目标必须有武器
            if (aTargets[0].WeaponEffect == CardEffect.None) throw new EffectPrepareError();
            //必须能能够到对方
            if (!aBattlefield.Players.WithinShaRange(aTargets[0], aTargets[1], aBattlefield)) throw new EffectPrepareError();
            //添加进子事件队列
            aBattlefield.NewEventNode(new EventRecord(aLeader, aTargets[0], aTargets[1], aCards, aEffect, aSkill));
        }

        public new EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var source = aNode.Target!;
            var target = aNode.Target2!;
            //玩家自己和两个目标不能死亡,目标不能没有武器
            if (aNode.Source!.Dead || source.Dead || target.Dead || source.WeaponEffect == CardEffect.None) return aNode;
            //无懈可击
            if (aBattlefield.WuXieProc(source, aNode.Effect, aNode)) return aNode;
            //向目标问询杀
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForCards(AskForEnum.杀, source);
            if (response.Effect == CardEffect.杀)
            {
                collector.DropPlayerReponse(response);
                aBattlefield.CreateActionNode(new ActionNode(response));
                //执行杀的子事件
                var shaEvent = new EventRecord(source, target, response.Cards, response.Effect, response.Skill);
                base.Proc(shaEvent, aBattlefield);
            }
            else
            {
                //获得对方的武器对象
                var weapon = source.Weapon;
                if(weapon != null) aBattlefield.Move(source, target, [weapon]);
            }
            return aNode;
        }
    }
}
