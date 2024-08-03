using System.Linq;
using System.Xml.Linq;
using SanGuoSha.ServerCore.Contest.Global;
using BeaverMarkupLanguage;

namespace SanGuoSha.ServerCore.Contest.Data
{
    internal class SkillGangLie : SkillBase
    {
        public SkillGangLie()
            : base("刚烈", SkillEnabled.Passive, false)
        {

        }

        public override void OnChiefHarmed(GlobalEvent.EventRecoard aSourceEvent, ChiefBase aSource, ChiefBase aTarget, GlobalData aData, sbyte aDamage)
        {
            if (aSource != null)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aTarget, this));
                MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aTarget);
                if (res.YN == true)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aTarget, this, new ChiefBase[] { aSource }, new Card[] { }));
                    Card judgement = aData.Game.popJudgementCard(aTarget, Card.Effect.GangLie);
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.JudgementCard, res.SkillName, new Card[] { judgement }, Card.Effect.GangLie, aTarget, aSource, null);

                    if (judgement.CardHuaSe != Card.Suit.Heart)
                    {
                        if (aData.Game.GamePlayers[aSource].Hands.Count < 2)
                        {
                            aData.Game.DamageHealth(aSource, 1, aTarget, new GlobalEvent.EventRecoard(aTarget, aSource, new Card[] { }, Card.Effect.None, res.SkillName));
                        }
                        else
                        {
                            aData.Game.AsynchronousCore.SendMessage(new Beaver("askfor.ganglie.cards" , aTarget.ChiefName , aSource.ChiefName).ToString());// new XElement("askfor.ganglie.cards", new XElement("target", aTarget.ChiefName), new XElement("target2", aSource.ChiefName)));
                            res = aData.Game.AsynchronousCore.AskForCardsWithCount(aSource, 2);
                            if (res.Cards.Count() != 2)
                            {
                                aData.Game.DamageHealth(aSource, 1, aTarget, new GlobalEvent.EventRecoard(aTarget, aSource, new Card[] { }, Card.Effect.None, res.SkillName));
                            }
                            else
                            {
                                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeDropMessage(aSource, aSource, res.Cards));
                                aData.Game.DropCards(true, GlobalEvent.CardFrom.Hand, res.SkillName, res.Cards, Card.Effect.None, aSource, null, null);
                            }
                        }
                    }
                }
            }
        }
    }
}
