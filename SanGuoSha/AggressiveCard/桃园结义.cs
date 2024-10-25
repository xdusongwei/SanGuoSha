using SanGuoSha.BaseClass;


namespace SanGuoSha.AggressiveCard
{
    [Card(CardEffect.桃园结义)]
    internal class 桃园结义: ICardEffectBase
    {
        [NoOneDead]
        [NeedSourcePlayer]
        [NeedTargets(0)]
        [NeedCards(1)]
        public void Prepare(PlayerBase aLeader, PlayerBase[] aTargets, CardEffect aEffect, Card[] aCards, SkillBase? aSkill, BattlefieldBase aBattlefield)
        {
            //下面是把从出牌玩家开始的所有玩家依次装入子事件列表中
            var player = aLeader;
            do
            {
                if (player.Injured)
                    aBattlefield.NewEventNode(new EventRecord(aLeader, player, aCards, aEffect, aSkill));
            } while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, aLeader));
        }

        public EventRecord Proc(EventRecord aNode, BattlefieldBase aBattlefield)
        {
            var target = aNode.Target!;
            //玩家不能死亡,并且血没有满
            if (!target.Injured) return aNode;
            //进入无懈可击的过程
            if (aBattlefield.WuXieProc(target, aNode.Effect, aNode)) return aNode;
            //玩家血量增加
            aBattlefield.RegainHealth(target, 1);
            return aNode;
        }
    }
}
