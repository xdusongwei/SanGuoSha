using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    internal class SkillLianYing : SkillBase
        {
            public SkillLianYing()
                : base("连营", SkillEnabled.Passive , false)
            {

            }

            public override void OnRemoveCards(ChiefBase aChief, GlobalData aData)
            {
                if (aData.Game.GamePlayers[aChief].Hands.Count == 0)
                {
                    aData.Game.AsynchronousCore.SendMessage( MessageCore.MakeTriggerSkillMesssage(aChief ,this , new ChiefBase[]{} , new Card[]{} ));
                    aData.Game.TakeingCards(aChief, 1);
                }
            }
        
    }

    internal class SkillQianXun : SkillBase
    {
        public SkillQianXun()
            : base("谦逊", SkillEnabled.Passive, false)
        {

        }

        public override bool EffectFeasible(Card[] aCards, Card.Effect aEffect, ChiefBase aTarget, bool aFeasible, GlobalData aData)
        {
            switch (aEffect)
            {
                case Card.Effect.ShunShouQianYang:
                case Card.Effect.LeBuSiShu:
                    return false;
            }
            return aFeasible;
        }
    }
}
