using System.Linq;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    internal class SkillGuiCai : SkillBase
    {
        public SkillGuiCai()
            : base("鬼才", SkillEnabled.Passive, false)
        {

        }

        public override Card OnChiefJudgementCardShow_Turn(ChiefBase aJudgeChief, Card aJudgementCard, ChiefBase aThisChief, GlobalData aData)
        {
            if (aData.Game.GamePlayers[aThisChief].HasHand)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aThisChief, this));
                MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForCards(aThisChief, MessageCore.AskForEnum.TargetHand, aThisChief);
                if (res.Cards.Count() == 1)
                {
                    //旧的判定牌删去
                    aData.Game.DropCards(true, GlobalEvent.CardFrom.None, res.SkillName, new Card[] { aJudgementCard }, Card.Effect.None, aJudgeChief, null, null);
                    //将自己的这张手牌除去
                    aData.Game.DropCards(false, GlobalEvent.CardFrom.Hand, res.SkillName, res.Cards, Card.Effect.None, aThisChief, null, null);
                    //触发技能的消息
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aThisChief, this, new ChiefBase[] { aJudgeChief }, res.Cards));
                    //返回新的判定牌
                    return res.Cards[0];
                }
            }
            return aJudgementCard;
        }
    }

    internal class SkillFanKui : SkillBase
    {
        public SkillFanKui()
            : base("反馈", SkillEnabled.Passive, false)
        {

        }

        public override void OnChiefHarmed(GlobalEvent.EventRecoard aSourceEvent, ChiefBase aSource, ChiefBase aTarget, GlobalData aData, sbyte aDamage)
        {
            if (aSource != null && aData.Game.GamePlayers[aSource].HasCard)
            {
                aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeAskForSkillMessage(aTarget, this));
                MessageCore.AskForResult res = aData.Game.AsynchronousCore.AskForYN(aTarget);
                if (res.YN == true && aData.Game.GamePlayers[aSource].HasCard)
                {
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aTarget, this, new ChiefBase[] { aSource }, new Card[] { }));
                    res = aData.Game.AsynchronousCore.AskForCards(aTarget, MessageCore.AskForEnum.TargetCard, aSource);
                    if (res.Cards.Count() == 0)
                        if (aData.Game.GamePlayers[aSource].HasCard)
                            res = new MessageCore.AskForResult(false, aTarget, new ChiefBase[] { }, new Card[] { aData.Game.AutoSelect(aSource) }, Card.Effect.GuoHeChaiQiao, false, false, string.Empty);
                        else
                            return;
                    //检查牌的来源知否是正确
                    if (!aData.Game.GamePlayers[aSource].HasCardsInHandOrEquipage(res.Cards)) return;
                    aData.Game.AsynchronousCore.SendStealMessage(aSource, aTarget, res.Cards, aData.Game.GamePlayers);
                    aData.Game.Move(aTarget, aSource, res.Cards);
                }
            }
        }
    }
}
