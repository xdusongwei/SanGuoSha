using System.Collections.Generic;
using System.Linq;
using SanGuoSha.Contest.Global;

namespace SanGuoSha.Contest.Data
{
    internal class SkillTuXi : SkillBase
    {
        public SkillTuXi()
            : base("突袭" , SkillEnabled.Passive , false)
        {

        }

        public override void TakingCards(ChiefBase aChief, GlobalData aData)
        {
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aChief, this));
            MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aChief);
            if (res.YN)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], []));
                res = aData.Game.AsynchronousCore.AskForCardsWithCount(aChief, 0);
                if (res.Targets.Count() > 2 || res.Targets.Count() == 0)
                {
                    return;
                }
                else
                {
                    foreach (ChiefBase c in res.Targets)
                        if (c.IsMe(aChief)||aData.Game.GamePlayers[c].Dead) return;
                }
                aData.TakeCardsCount = 0;
                foreach (ChiefBase c in res.Targets)
                {
                    Card item = aData.Game.AutoSelect(c);
                    aData.Game.AsynchronousCore.SendStealMessage(c, aChief, [item], aData.Game.GamePlayers);
                    aData.Game.Move(c, aChief, [item]);
                    //aData.Game.EventNode(false, GlobalEvent.CardFrom.Hand, SkillName, new Card[] { item }, Card.Effect.None, c, null, null);
                    //aData.Game.GamePlayers[aChief].Hands.Add(item.GetOriginalCard());
                }

            }
        }
    }
}
