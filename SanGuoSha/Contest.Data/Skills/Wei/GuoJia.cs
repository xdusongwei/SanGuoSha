using System.Linq;
using System.Xml.Linq;
using SanGuoSha.Contest.Global;
using BeaverMarkupLanguage;

namespace SanGuoSha.Contest.Data
{
    internal class SkillTianDu : SkillBase
    {
        public SkillTianDu()
            : base("天妒" , SkillEnabled.Passive , false)
        {

        }
        public override void OnChiefJudgementCardTakeEffect(ChiefBase aChief, Card aCard, ref bool aEnableSendToBin, GlobalData aData)
        {
            aEnableSendToBin = false;
            aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, [], [aCard]));
            aData.Game.GamePlayers[aChief].Hands.Add(aCard.GetOriginalCard());
        }
    }

    internal class SkillYiJi : SkillBase
    {
        public SkillYiJi()
            : base("遗计", SkillEnabled.Passive, false)
        {

        }

        public override void OnCreate(ChiefBase aChief)
        {
            aChief.SlotsBuffer.Slots.Add(new Slot(SkillName, false, false));
        }

        public override void OnChiefHarmed(GlobalEvent.EventRecoard aSourceEvent, ChiefBase? aSource, ChiefBase aTarget, GlobalData aData, sbyte aDamage)
        {
            int i = 0;
            for (i = 0; i < aDamage; i++)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aTarget, this));
                MessageCore.AskForResult? res = aData.Game.AsynchronousCore.AskForYN(aTarget);
                if (res.YN)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aTarget, this, [], []));
                    Card[] cards = aData.Game.TakingCards(aTarget, 2);
                    aTarget.SlotsBuffer[SkillName].Cards.Clear();
                    aTarget.SlotsBuffer[SkillName].Cards.AddRange(cards);
                    int times = 0;
                    while (aTarget.SlotsBuffer[SkillName].Cards.Count > 0 && times < 2)
                    {
                        aData.Game.AsynchronousCore.SendPrivateMessageWithOpenMessage(aTarget,
                            new Beaver("askfor.yiji.cards", aTarget.ChiefName , Card.Cards2Beaver("cards" , [.. aTarget.SlotsBuffer[SkillName].Cards])).ToString(),
                            //new XElement("askfor.yiji.cards",
                            //    new XElement("target", aTarget.ChiefName),
                            //    Card.Cards2XML("cards", aTarget.SlotsBuffer[SkillName].Cards.ToArray())
                            //    ),
                            new Beaver("askfor.yiji.cards", aTarget.ChiefName).ToString(),
                            //new XElement("askfor.yiji.cards",
                            //    new XElement("target", aTarget.ChiefName)
                            //    ),
                                aData.Game.GamePlayers);
                        res = aData.Game.AsynchronousCore.AskForCardsWithCount(aTarget, -1);
                        if (res.Targets.Count() != 1 || res.Targets[0] == aTarget || aData.Game.GamePlayers[res.Targets[0]].Dead) break;
                        foreach (Card c in res.Cards)
                        {
                            if (!aTarget.SlotsBuffer[SkillName].Cards.Contains(c)) break;
                        }
                        aData.Game.AsynchronousCore.SendGiveMessage(aTarget, res.Targets[0], res.Cards, aData.Game.GamePlayers);
                        aData.Game.DropCards(false, GlobalEvent.CardFrom.Hand, SkillName, res.Cards, Card.Effect.None, aTarget, null, null);
                        aData.Game.GamePlayers[res.Targets[0]].Hands.AddRange(res.Cards);
                        foreach (Card c in res.Cards)
                        {
                            aTarget.SlotsBuffer[SkillName].Cards.Remove(c);
                        }
                        times++;
                    }
                    aTarget.SlotsBuffer[SkillName].Cards.Clear();
                }
                else
                    break;
            }
        }
    }
}
