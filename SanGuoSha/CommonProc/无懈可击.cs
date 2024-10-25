using SanGuoSha.BaseClass;


namespace SanGuoSha.CommonProc
{
    internal partial class CommonProc
    {
        public static bool WuXieProc(PlayerBase aTarget, CardEffect aEffect, bool aIgnoreWuXie, BattlefieldBase aBattlefield)
        {
            var effect = CardEffect.无懈可击;
            if(aIgnoreWuXie) return false;
            if(!Card.IsKit(aEffect) && !Card.IsDelayKit(aEffect)) return false;
            //这个量来表示场上是否有无懈可击存在
            bool WuXieExist = false;
            //表决字典
            //表决字典是用来记录玩家(键)的表态(值)
            //某个玩家的值为false代表没有表态,true表带已表态
            //通信层根据这个字典会对值为False的玩家进行并行问询
            //若其中有玩家问询时选择确认(一般指放弃),将其值定位true
            //若所有玩家都是true或超时,表决结束,问询返回None
            //若有玩家出 无懈可击 时,表决问询将直接结束,返回 WuXieKeJi问询
            Dictionary<PlayerBase, bool> abstention = [];
            var player = aTarget;
            //遍历场上的玩家
            do
            {
                if (!player.Dead && player.Hands.Any(c => c.CardEffect == effect))
                {
                    //这个玩家有手牌无懈可击,将他加入表决字典并置value为false,表示他可以表决
                    abstention.Add(player, false);
                    //无懈可击存在
                    WuXieExist = true;
                }
                else
                {
                    //这个玩家没有无懈可击,设置其值为true,表示已表决
                    abstention.Add(player, true);
                }
            } while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, aTarget));

            //无懈可击存在,启动问询
            if (WuXieExist)
            {
                //开始问询
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForCards(AskForEnum.无懈可击, abstention);
                //若有玩家打出无懈可击
                if (response.Effect == effect)
                {
                    using var collector = aBattlefield.NewCollector();
                    collector.DropPlayerReponse(response);
                    aBattlefield.CreateActionNode(new ActionNode(response));
                    //目标换成出牌者
                    aTarget = response.Leader;
                    //两个结果异或即本轮结果
                    return true ^ WuXieProc(aTarget, effect, aIgnoreWuXie, aBattlefield);
                }
            }
            //返回false
            return false;
        }
    }
}