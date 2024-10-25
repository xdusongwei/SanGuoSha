

namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        /// <summary>
        /// 从别的玩家哪里选择牌的方法
        /// </summary>
        /// <param name="aAskFor"></param>
        /// <param name="aPlayer"></param>
        /// <param name="aTarget"></param>
        /// <returns></returns>
        public AskForResult AskForCards(AskForEnum aAskFor, PlayerBase aPlayer, PlayerBase aTarget)
        {
            EnableCardIdArg = true;
            AskFor = aAskFor;
            AskPlayers = [aPlayer];
            Target = aTarget;
            WaitTask();
            return Response;
        }

        /// <summary>
        /// 向所有玩家发送带有表决的出牌问询
        /// </summary>
        /// <param name="aAskFor">问询内容</param>
        /// <param name="aAbstention">表决字典</param>
        /// <returns>返回问询结果</returns>
        public AskForResult AskForCards(AskForEnum aAskFor, Dictionary<PlayerBase, bool> aAbstention)
        {
            EnableCardIdArg = true;
            AskFor = aAskFor;
            var players = aAbstention.Keys.Where(i => !aAbstention[i]).Distinct().ToArray();
            Array.Sort(players.Select(i => i.UID).ToArray(), players);
            AskPlayers = players;
            WaitTask();
            return Response;
        }

        /// <summary>
        /// 问询玩家出牌
        /// </summary>
        /// <param name="aPlayer">被问询的玩家</param>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aWrapper"></param>
        /// <param name="aData"></param>
        /// <returns>返回问询结果</returns>
        public AskForResult AskForCards(AskForEnum aAskFor, PlayerBase aPlayer)
        {
            EnableCardIdArg = true;
            AskFor = aAskFor;
            AskPlayers = [aPlayer];
            WaitTask();
            return Response;
        }

        /// <summary>
        /// 问询玩家出牌
        /// </summary>
        /// <param name="aPlayer">被问询的玩家</param>
        /// <param name="aAskFor">问询的内容</param>
        /// <param name="aActiveArmor">指示是否让防具来参与问询的回应</param>
        /// <param name="aWrapper"></param>
        /// <param name="aData"></param>
        /// <returns>返回问询结果</returns>
        public AskForResult AskForCards(AskForEnum aAskFor, PlayerBase aPlayer, bool aActiveArmor)
        {
            EnableCardIdArg = true;
            AskFor = aAskFor;
            AskPlayers = [aPlayer];
            WaitTask();
            return Response;
        }

        /// <summary>
        /// 问询玩家是否
        /// </summary>
        /// <param name="aPlayer">玩家的角色</param>
        /// <returns>返回问询结果</returns>
        public AskForResult AskForYN(AskForEnum aAskFor, PlayerBase aPlayer)
        {
            AskFor = aAskFor;
            AskPlayers = [aPlayer];
            WaitTask();
            return Response;
        }

        /// <summary>
        /// 问询玩家是否
        /// </summary>
        /// <param name="aPlayer">玩家的角色</param>
        /// <returns>返回问询结果</returns>
        public AskForResult AskForYN(AskForEnum aAskFor, PlayerBase aPlayer, PlayerBase aTarget)
        {
            AskFor = aAskFor;
            AskPlayers = [aPlayer];
            Target = aTarget;
            WaitTask();
            return Response;
        }

        public AskForResult AskForChief(PlayerBase aPlayer)
        {
            AskFor = AskForEnum.PlayerChief;
            AskPlayers = [aPlayer];
            WaitTask();
            return Response;
        }

        public AskForResult AskForSuit(AskForEnum aAskFor, PlayerBase aPlayer)
        {
            AskFor = aAskFor;
            AskPlayers = [aPlayer];
            WaitTask();
            return Response;
        }

        public AskForResult AskForChiefs(PlayerBase aPlayer, int n)
        {
            AskFor = AskForEnum.PlayerChief;
            AskPlayers = [aPlayer];
            WaitTask();
            return Response;
        }

        private void WaitTask()
        {
            var playerIDs = string.Join(',', AskPlayers.Select(i => i.UID));
            var stackItem = new Tuple<string, AskForEnum>(playerIDs, AskFor);
            if(IAnswer.AskStack.Contains(stackItem)) throw new Exception($"对玩家{playerIDs}的问询{AskFor}在栈中重复");
            IAnswer.AskStack.Push(stackItem);
            if(isFinish) throw new Exception("不能重复使用AskAnswer对象来继续发起问询");
            foreach(var player in AskPlayers)
                foreach(var skill in player.Skills)
                    skill.BeforeAskfor(player, AskFor, Battlefield);
            BeforeAskEvent?.Invoke(this, new AskArgs(AskPlayers, AskFor));
            var maxTimeout = int.Min(30_000, MaxTimeout);
            maxTimeout = int.Max(0, maxTimeout);
            Battlefield.UpdateSnapshot(this);
            try
            {
                if(maxTimeout > 0)
                    Task.WaitAll([SemaphoreSlim.WaitAsync(maxTimeout)]);
            }
            catch(AggregateException)
            {
                
            }
            finally
            {
                if(Battlefield.AnswerCooldown > 0)
                    Thread.Sleep(Battlefield.AnswerCooldown);
                isFinish = true;
            }
            Response = IAnswer.NewAnswer(Response);
            foreach(var player in AskPlayers)
                foreach(var skill in player.Skills)
                    skill.FinishAskfor(player, AskFor, Battlefield);
            AfterAnswerEvent?.Invoke(this, new AnswerArgs(Response));
            IAnswer.AskStack.Pop();
        }
    }
}
