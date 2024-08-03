using SanGuoSha.ServerCore.Contest.Data;
using SanGuoSha.ServerCore.Contest.Equipage;
using System.Linq;
using System.Xml.Linq;
using System;

namespace SanGuoSha.ServerCore.Contest.Global
{
    public partial class GlobalEvent
    {
        public void RefereeProc()
        {
            string[] winners = new string[0] { };
            string[] losers = new string[0] { };
            string[] draws = new string[0] { };
            switch (Mode)
            {
                case GameMode.FiveSTD:
                case GameMode.FiveJunZheng:
                case GameMode.EightJunZheng:
                case GameMode.EightSTD:
                    try
                    {
                        Player[] AllPlayers = GamePlayers.All;
                        Player[] MArr = AllPlayers.Where(i => i.Chief != null && i.Chief.ChiefStatus == ChiefBase.Status.Majesty).ToArray();
                        if (MArr.Count() == 1 && MArr[0].Dead) //主公数量有问题或者主公死了
                        {
                            //主公死,反贼还有活的,反贼赢
                            if (AllPlayers.Where(i => i.Chief.ChiefStatus == ChiefBase.Status.Insurgent && (!i.Dead && !i.IsEscaped && !i.IsOffline)).Count() > 0)
                            {
                                //反贼赢
                                winners = AllPlayers.Where(i => i.Chief.ChiefStatus == ChiefBase.Status.Insurgent).Select(i => i.UID).ToArray();
                                losers = AllPlayers.Where(i => i.Chief.ChiefStatus != ChiefBase.Status.Insurgent).Select(i => i.UID).ToArray();
                                throw new SanGuoSha.ServerCore.Contest.Data.GameException.ContestFinished(winners, losers, draws);
                            }
                            else
                            {
                                //内奸赢
                                winners = AllPlayers.Where(i => i.Chief.ChiefStatus == ChiefBase.Status.Spy).Select(i => i.UID).ToArray();
                                losers = AllPlayers.Where(i => i.Chief.ChiefStatus != ChiefBase.Status.Spy).Select(i => i.UID).ToArray();
                                throw new SanGuoSha.ServerCore.Contest.Data.GameException.ContestFinished(winners, losers, draws);
                            }
                        }
                        else //主公没死
                        {
                            //主公没死,全场没有内奸和反贼或者,主忠赢
                            if (AllPlayers.Where(i => (i.Chief.ChiefStatus == ChiefBase.Status.Spy || i.Chief.ChiefStatus == ChiefBase.Status.Insurgent) && (!i.Dead && !i.IsEscaped && !i.IsOffline)).Count() == 0)
                            {
                                //主忠赢
                                winners = AllPlayers.Where(i => i.Chief.ChiefStatus == ChiefBase.Status.Majesty || i.Chief.ChiefStatus == ChiefBase.Status.Loyalist).Select(i => i.UID).ToArray();
                                losers = AllPlayers.Where(i => i.Chief.ChiefStatus != ChiefBase.Status.Majesty && i.Chief.ChiefStatus != ChiefBase.Status.Loyalist).Select(i => i.UID).ToArray();
                                throw new SanGuoSha.ServerCore.Contest.Data.GameException.ContestFinished(winners, losers, draws);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Msg {0}\nStack{1}", e.Message, e.StackTrace);
                    }
                    break;
            }
        }
    }
}
