using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.杀)]
    internal class 杀: ICardEffectBase
    {
        [NeedSourcePlayer]
        [NeedTargets]
        [SourceNotInTargets]
        [PlayersDistinct]
        [NoOneDead]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //若出杀有限制且已经没有机会杀了不能执行
            if (!aBattlefield.ActionPlayerData.ShaNoLimit && aBattlefield.ActionPlayerData.ShaRemain < 1) throw new EffectPrepareError();
            //杀的目标数量高于最大值不能执行
            if (aLeader.CalcMaxShaTargets(aCards, aBattlefield) < aTargets.Length) throw new EffectPrepareError();

            //遍历目标集合,如果目标有自己或者目标已死亡或者 够不到对方不能执行
            foreach (var target in aTargets)
            {
                if (target == aLeader || target.Dead || !aBattlefield.Players.WithinShaRange(aLeader, target, aBattlefield)) throw new EffectPrepareError();
                bool enable = true;
                foreach (var s in target.Skills)
                    enable = s.EffectFeasible(aEffect, target, enable, aBattlefield);
                if (!enable) throw new EffectPrepareError();
            }

            //设置杀计数器
            if (aBattlefield.ActionPlayerData.ShaRemain < 2)
                aBattlefield.ActionPlayerData.ShaRemain = 0;
            else
                --aBattlefield.ActionPlayerData.ShaRemain;
            aBattlefield.ActionPlayerData.ShaTimes++;
            if(aSkill == null || aSkill.EnableCreateEventNode)
            {
                //将杀事件按目标分解成一个个小事件,并装进子事件队列
                //即每个子事件都是玩家杀单独的玩家的处理事件
                foreach (var target in aTargets)
                    aBattlefield.NewEventNode(new EventRecord(aLeader, target, aCards, aEffect, aSkill));
            }
        }

        private static Card.ElementType DefaultShaElement(Card[] aCards)
        {
            var shaElement = Card.ElementType.None;
            if(aCards.Length != 1)
                shaElement = Card.ElementType.None;
            else
                shaElement = aCards[0].Element;
            return shaElement;
        }

        private static Card.CardColor DefaultShaColor(Card[] aCards)
        {
            var shaColor = Card.CardColor.Unknown;
            if(aCards.Length < 1)
                shaColor = Card.CardColor.Unknown;
            else if(aCards.All(i => i.Color == Card.CardColor.Black))
                shaColor = Card.CardColor.Black;
            else if(aCards.All(i => i.Color == Card.CardColor.Red))
                shaColor = Card.CardColor.Red;
            else
                shaColor = Card.CardColor.Unknown;
            return shaColor;
        }

        private static bool AskShan(PlayerBase aSource, PlayerBase aTarget, EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var enableTargetArmor = WeaponProc.EnableTargetArmor(aSource);
            bool enableDamage = true;
            int times = 1;
            foreach (var s in aSource.Skills)
                times = s.CalcAskforTimes(aSource, aTarget, CardEffect.杀, times, aBattlefield);
            var allResponse = true;
            for (int i = 0; i < times; i++)
            {
                var askForEffect = CardEffect.闪;
                var responseShan = false;
                var skip = false;
                if (enableTargetArmor)
                {
                    skip = ArmorProc.WhenAskingShan(aTarget, aBattlefield);
                    if(skip) responseShan = true;
                }
                if(!skip)
                {
                    //问询 闪
                    using var aa = aBattlefield.NewAsk();
                    var response = aa.AskForCards(AskForEnum.闪, aTarget, aSource);
                    //问询结果放入子事件处理列表
                    using var collector = aBattlefield.NewCollector();
                    collector.DropPlayerReponse(response);
                    if (response.Effect == askForEffect)
                    {
                        responseShan = true;
                        aBattlefield.CreateActionNode(new ActionNode(response));
                    }
                }
                if (responseShan)
                {
                    foreach (var s in aTarget.Skills)
                        s.OnUseEffect(aTarget, askForEffect, aBattlefield);
                }
                else
                {
                    allResponse = false;
                    break;
                }
            }
            if (times > 0 && allResponse)
                enableDamage = false;
            if(!enableDamage && aSource.WeaponEffect != CardEffect.None)
                enableDamage = WeaponProc.TargetShan(aSource.WeaponEffect, aSource, aTarget, aNode, aBattlefield);
            return enableDamage;
        }

        private static void ExecuteDamage(PlayerBase aSource, PlayerBase aTarget, EventRecord aNode, Card.ElementType aElement, BattlefieldBase aBattlefield)
        {
            if(WeaponProc.EnableShaDamage(aSource, aTarget, aBattlefield))
            {
                sbyte cost = 1;
                cost = WeaponProc.CalcDamage(aSource.WeaponEffect, aTarget, aNode.Effect, cost);
                foreach (var s in aSource.Skills)
                    cost = s.CalcDamage(aSource, aNode.Effect, cost, aBattlefield);
                if (WeaponProc.EnableTargetArmor(aSource))
                    cost = ArmorProc.CalcDamage(cost, aNode.Effect, aNode.Cards, aTarget.ArmorEffect, aElement);
                var element = Card.ElementType.None;
                aBattlefield.DamageHealth(aTarget, cost, aSource, aNode, element);
            }
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var source = aNode.Source!;
            var target = aNode.Target!;
            foreach (var s in target!.Skills)
                s.BeforeProcessingEvent(target, ref aNode, aBattlefield);
            target = aNode.Target!;
            //这里再检查一次玩家是否死亡是怕玩家在以前的子事件中挂掉了,如果真挂了就忽略掉这次事件
            if (target.Dead) return aNode;
            var procContine = WeaponProc.WhenShaAccpeted(aNode.Cards, source, target, aBattlefield);
            if(!procContine) return aNode;
            var shaElement = DefaultShaElement(aNode.Cards);
            var shaColor = DefaultShaColor(aNode.Cards);
            shaElement = WeaponProc.ShaElement(aNode.Cards, source, shaElement, aBattlefield);

            var targetHasArmor = target.Armor != null;
            var enableTargetArmor = WeaponProc.EnableTargetArmor(source);
            var targetShouldResponse = ArmorProc.EnableFor(target.ArmorEffect, aNode.Cards, aNode.Effect, shaElement, shaColor);
            if (!targetHasArmor || !enableTargetArmor || (enableTargetArmor && targetShouldResponse))
            {
                var enableDamage = AskShan(source, target, aNode, aBattlefield);
                if (enableDamage)
                {
                    ExecuteDamage(source, target, aNode, shaElement, aBattlefield);
                }
            }
            return aNode;
        }
    }
}
