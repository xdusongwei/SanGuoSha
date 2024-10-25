using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.五谷丰登)]
    internal class 五谷丰登: ICardEffectBase
    {
        [NeedSourcePlayer]
        [NoOneDead]
        [NeedTargets(0)]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            var gameSlots = aBattlefield.Slots;
            var wgfdSlot = gameSlots[BattlefieldBase.WGFDSlotName];
            var player = aLeader;
            do
            {
                aBattlefield.NewEventNode(new EventRecord(aLeader, player, aCards, aEffect, aSkill));
            } while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, aLeader));
            if(wgfdSlot.Cards.Count > 0) throw new EffectPrepareError();
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var gameSlots = aBattlefield.Slots;
            var wgfdSlot = gameSlots[BattlefieldBase.WGFDSlotName];
            var target = aNode.Target!;
            //子事件的目标必须存在且目标不能死亡
            if (target.Dead) return aNode;
            if(aNode.Source == aNode.Target)
            {
                var slotCards = collector.PopCards(aBattlefield.Players.AliveCount);
                wgfdSlot.Cards.AddRange(slotCards);
            }
            //开始无懈可击的处理
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            //获取问询结果
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForCards(AskForEnum.五谷丰登选牌, target);
            var cards = response.Cards;
            
            //回应的牌不正确,则自动选择一张
            if(response.Cards.Length != 1 || !wgfdSlot.CardInSlotAndNoOneChoose(cards[0]))
            {
                var card = target.AutoSelectSlot(wgfdSlot);
                if(card != null && wgfdSlot.MarkPlayerChoose(target, card)) 
                    cards = [card];
                else
                    cards = [];
                response.OverwriteResponse(target, cards);
            }
            else
            {
                if(wgfdSlot.MarkPlayerChoose(target, response.Cards[0])) 
                    cards = response.Cards;
                else
                    cards = [];
            }
            if(cards.Length == 1)
                collector.Pick(cards[0], target);
            response.Cards = cards;
            aBattlefield.CreateActionNode(new ActionNode(response));
            return aNode;
        }
    }
}
