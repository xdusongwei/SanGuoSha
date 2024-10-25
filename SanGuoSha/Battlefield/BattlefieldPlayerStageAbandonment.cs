using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase
    {
        /// <summary>
        /// 弃牌问询
        /// </summary>
        /// <param name="aPlayer">需要弃牌的玩家</param>
        /// <returns>返回true表示弃牌正常</returns>
        private bool Abandonment(PlayerBase aPlayer)
        {
            bool first = true;
            var askForType = AskForEnum.AbandonmentCard;
            //确定玩家弃牌的要求是否达到
            while (aPlayer.Hands.Count > sbyte.Max(0, aPlayer.Health))
            {
                //问询武将出牌
                if (first)
                {
                    first = false;
                }
                else
                {
                    askForType = AskForEnum.AbandonmentCardNext;
                }
                using var aa = NewAsk();
                var r = aa.AskForCards(askForType, aPlayer);
                if (r.TimeOut || r.Cards.Length == 0)
                    r = new AskForResult(false, askForType, aPlayer, aPlayer, [], aPlayer.AutoAbandonment());
                //是否超过弃牌数
                if (aPlayer.Hands.Count - r.Cards.Length < sbyte.Max(0, aPlayer.Health))
                {
                    //超过了，不合法，自动选择
                    Card[] cards = aPlayer.AutoAbandonment();
                    if (!RemoveHand(aPlayer, cards)) return false;
                    r.OverwriteResponse(aPlayer, cards);
                    CardsHeap.AddCards(cards);
                }
                else
                {
                    if (!RemoveHand(aPlayer, r.Cards)) return false;
                    CardsHeap.AddCards(r.Cards);
                }
                CreateActionNode(new ActionNode(r));
                //若牌数满足上限,弃牌完成
                if (aPlayer.Hands.Count == aPlayer.Health)
                {
                    return true;
                }
            }
            //牌数小于限制,跳过弃牌
            return true;
        }
    }
}
