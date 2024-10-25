using SanGuoSha.BaseClass;


namespace SanGuoSha.TrialProc
{
    [Card(CardEffect.兵粮寸断)]
    internal class 兵粮寸断: ITrialProcBase
    {
        public void Proc(PlayerBase aPlayer, Card aCard, CardEffect aEffect, BattlefieldBase aBattlefield)
        {
            using var collector = aBattlefield.NewCollector();
            var apd = aBattlefield.ActionPlayerData;
            //无懈可击的过程
            if (aBattlefield.WuXieProc(apd.CurrentPlayer, aEffect)) return;
            //获取判定牌
            var sentenceCard = collector.PopSentenceByCard(apd.CurrentPlayer, aEffect);
            //对判定牌的处理
            if (sentenceCard.CardSuit != Card.Suit.Club)
                apd.Take = false;
            else
                apd.Take = true;
        }
    }
}
