using SanGuoSha.BaseClass;


namespace SanGuoSha.CommonProc
{
    internal partial class CommonProc
    {
        public static void RefereeProc(BattlefieldBase aBattlefield)
        {
            var players = aBattlefield.Players;
            PlayerBase[] winners = [];
            PlayerBase[] losers = [];
            PlayerBase[] draws = [];
            foreach(var player in players)
            {
                if(player.Health == 0 && !player.Dead)
                {
                    player.Dead = true;
                }
            }
            switch (aBattlefield.Mode)
            {
                case GameMode.FiveSTD:
                case GameMode.FiveJunZheng:
                case GameMode.EightJunZheng:
                case GameMode.EightSTD:
                    PlayerBase[] MArr = players.Where(i => i.Role == PlayerRole.Majesty).ToArray();
                    if (MArr.All(i => i.Dead)) //主公死了
                    {
                        //主公死,反贼还有活的,反贼赢
                        if (players.Where(i => i.Role == PlayerRole.Insurgent && !i.Dead && !i.IsEscaped && !i.IsOffline).Any())
                        {
                            //反贼赢
                            winners = players.Where(i => i.Role == PlayerRole.Insurgent).ToArray();
                            losers = players.Where(i => i.Role != PlayerRole.Insurgent).ToArray();
                            throw new ContestFinished(winners, losers, draws);
                        }
                        else
                        {
                            //内奸赢
                            winners = players.Where(i => i.Role == PlayerRole.Spy).ToArray();
                            losers = players.Where(i => i.Role != PlayerRole.Spy).ToArray();
                            throw new ContestFinished(winners, losers, draws);
                        }
                    }
                    else //主公没死
                    {
                        //主公没死,全场没有内奸和反贼或者,主忠赢
                        if (!players.Where(i => (i.Role == PlayerRole.Spy || i.Role == PlayerRole.Insurgent) && !i.Dead && !i.IsEscaped && !i.IsOffline).Any())
                        {
                            //主忠赢
                            winners = players.Where(i => i.Role == PlayerRole.Majesty || i.Role == PlayerRole.Loyalist).ToArray();
                            losers = players.Where(i => i.Role != PlayerRole.Majesty && i.Role != PlayerRole.Loyalist).ToArray();
                            throw new ContestFinished(winners, losers, draws);
                        }
                    }
                    break;
            }
        }
    }
}
