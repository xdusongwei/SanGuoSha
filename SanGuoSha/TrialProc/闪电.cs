using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.TrialProc
{
    [Card(CardEffect.闪电)]
    internal class 闪电: ITrialProcBase
    {
        private static void SetNewPlayerTurn(PlayerBase aPlayer, Card aCard, BattlefieldBase aBattlefield)
        {
            var apd = aBattlefield.ActionPlayerData;
            //闪电需要挂到下一位武将的判定区,如果下一位的判定区有闪电,那就是再下一位...直到处理到该玩家为之
            var nextPlayer = apd.CurrentPlayer;
            //指示闪电是否安置好了
            bool handled = false;
            while(aBattlefield.Players.NextAliveUntilNullOrStop(ref nextPlayer, apd.CurrentPlayer))
            {
                if (nextPlayer.HasDebuff(CardEffect.闪电)) continue;
                nextPlayer.PushTrialCard(aCard, CardEffect.闪电);
                //闪电已安置
                handled = true;
                break;
            }
            //如果闪电没有安置,那就挂到玩家的新debuff栈中
            using var collector = aBattlefield.NewCollector();
            if (!handled && collector.Pick(aCard))
                aPlayer.Slots[PlayerBase.NewDebuffSlot].AddCards([new DebuffNode(aCard, CardEffect.闪电)]);
        }

        public void Proc(PlayerBase aPlayer, Card aCard, CardEffect aEffect, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var apd = aBattlefield.ActionPlayerData;
            //无懈可击的过程
            if (aBattlefield.WuXieProc(apd.CurrentPlayer, aEffect))
            {
                SetNewPlayerTurn(aPlayer, aCard, aBattlefield);
                return;
            }
            //取出判定牌
            var sentenceCard = collector.PopSentenceByCard(apd.CurrentPlayer, aEffect);
            //费血
            if (sentenceCard.CardSuit == Card.Suit.Spade && sentenceCard.CardIndex >= 2 && sentenceCard.CardIndex <= 9)
            {
                var damage = ArmorProc.CalcDamage(3, aEffect, [aCard], apd.CurrentPlayer.ArmorEffect);
                var er = new EventRecord(null, apd.CurrentPlayer, null, [aCard], aEffect, null);
                aBattlefield.DamageHealth(apd.CurrentPlayer, damage, null, er);
            }
            else
            {
                SetNewPlayerTurn(aPlayer, aCard, aBattlefield);
            }
        }
    }
}
