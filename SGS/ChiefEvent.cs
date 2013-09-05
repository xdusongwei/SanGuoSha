using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGS.Data;
using SGS.Global;
namespace SGS.Events
{
    public abstract class ChiefEvent
    {
        /* 以下是由状态更换产生的托管，事件，触发事件的方法 */
        //public delegate void BeforeRoundStartDelegate(ChiefBase aChief);
        //public delegate void AfterRoundStartDelegate(ChiefBase aChief);
        //public delegate void BeforeJudgmentDelegate(ChiefBase aChief);
        //public delegate void AfterJudgmentDelegate(ChiefBase aChief);
        //public delegate void BeforeTakeingCardDelegate(ChiefBase aChief, ref bool JumpState);
        //public delegate void AfterTakingCardDelegate(ChiefBase aChief);
        //public delegate void BeforeLeadDelegate(ChiefBase aChief);
        //public delegate void AfterLeadDelegate(ChiefBase aChief);
        //public delegate void BeforeAbandonmentDelegate(ChiefBase aChief , ref bool JumpState);
        //public delegate void AfterAbandonmentDelegate(ChiefBase aChief);

        //public event BeforeRoundStartDelegate BeforeRoundStart;
        //public event AfterRoundStartDelegate AfterRoundStart;
        //public event BeforeJudgmentDelegate BeforeJudgment;
        //public event AfterJudgmentDelegate AfterJudgment;
        //public event BeforeTakeingCardDelegate BeforeTakeingCard;
        //public event AfterTakingCardDelegate AfterTakeingCard;
        //public event BeforeLeadDelegate BeforeLead;
        //public event AfterLeadDelegate AfterLead;
        //public event BeforeAbandonmentDelegate BeforeAbandonment;
        //public event AfterAbandonmentDelegate AfterAbanddonment;

        //public void RsiseBeforeRoundStart(ChiefBase aChief)
        //{
        //    if (BeforeRoundStart == null) return;
        //    BeforeRoundStart(aChief);
        //}
        //public void RaiseAfterRoundStartEvent(ChiefBase aChief)
        //{
        //    if (AfterRoundStart == null) return;
        //    AfterRoundStart(aChief);
        //}
        //public void RaiseBeforeJudgment(ChiefBase aChief)
        //{
        //    if (BeforeJudgment == null) return;
        //    BeforeJudgment(aChief);
        //}
        //public void RaiseAfterJudgment(ChiefBase aChief)
        //{
        //    if (AfterJudgment == null) return;
        //    AfterJudgment(aChief);
        //}
        //public void RaiseBeforeTakeingCard(ChiefBase aChief ,ref bool JumpState)
        //{
        //    JumpState = false;
        //    if (BeforeTakeingCard == null) return;
        //    BeforeTakeingCard(aChief, ref JumpState);
        //}
        //public void RaiseAfterTakeingCard(ChiefBase aChief)
        //{
        //    if (AfterTakeingCard == null) return;
        //    AfterTakeingCard(aChief);
        //}
        //public void RaiseBeforeLead(ChiefBase aChief)
        //{
        //    if (BeforeLead == null) return;
        //    BeforeLead(aChief);
        //}
        //public void RaiseAfterLead(ChiefBase aChief)
        //{
        //    if (AfterLead == null) return;
        //    AfterLead(aChief);
        //}
        //public void RaiseBeforeAbandonment(ChiefBase aChief , ref bool JumpState)
        //{
        //    if (BeforeAbandonment == null) return;
        //    BeforeAbandonment(aChief , ref JumpState);
        //}
        //public void RaiseAfterAbandonment(ChiefBase aChief)
        //{
        //    if (AfterAbanddonment == null) return;
        //    AfterAbanddonment(aChief);
        //}

        ///* 以下是一些特殊状况产生的委托、事件，触发事件的方法 */
        //public delegate void JugdmentForShanDelegate(ChiefBase aChief, ref bool Shan, Card JudgmentCard);
        //public delegate void BeforeJudgmentValidDelegate(ChiefBase aChief , ref Card aJCard , Players aPlayers);
        //public delegate void ArmorDelegate(ChiefBase aChief, Card.Effect aSourceEffect, bool? aPlayerAuthorized);

        //public event JugdmentForShanDelegate JudgmentForShan;
        //public event BeforeJudgmentValidDelegate BeforeJudgmentValid;
        //public event ArmorDelegate ArmorEvent;


        //public void RaiseJugdmentForShan(ChiefBase aChief, ref bool Shan , Card JudgmentCard)
        //{
        //    if (JudgmentForShan == null)
        //    {
        //        Shan = false;
        //        return;
        //    }
        //    JudgmentForShan(aChief, ref Shan , JudgmentCard);
        //}

        //public void RaiseBeforeJudgmentValid(ChiefBase aChief, ref Card aJCard, Players aPlayers)
        //{
        //    if (BeforeJudgmentValid == null)
        //    {
        //        return;
        //    }
        //    BeforeJudgmentValid(aChief, ref aJCard, aPlayers);
        //}

        //public void RaiseArmorEvent(ChiefBase aChief, Card.Effect aSourceEffect, bool? aPlayerAuthorized)
        //{
        //    if (ArmorEvent == null)
        //        return;
        //    ArmorEvent(aChief, aSourceEffect,aPlayerAuthorized);
        //}

        
    }
}
