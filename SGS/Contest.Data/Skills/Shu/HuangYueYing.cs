using System.Linq;
using System.Xml.Linq;
using SGS.ServerCore.Contest.Global;

namespace SGS.ServerCore.Contest.Data
{
    internal class SkillJiZhi : SkillBase
    {
        public SkillJiZhi()
            : base("集智")
        {

        }

        public override void OnUseEffect(ChiefBase aChief, Card.Effect aEffect, GlobalData aData)
        {
            switch (aEffect)
            {
                case Card.Effect.JueDou:
                case Card.Effect.WanJianQiFa:
                case Card.Effect.NanManRuQin:
                case Card.Effect.TaoYuanJieYi:
                case Card.Effect.GuoHeChaiQiao:
                case Card.Effect.ShunShouQianYang:
                case Card.Effect.HuoGong:
                case Card.Effect.JieDaoShaRen:
                case Card.Effect.TieSuoLianHuan:
                case Card.Effect.WuGuFengDeng:
                case Card.Effect.WuXieKeJi:
                case Card.Effect.WuZhongShengYou:
                    aData.Game.AsynchronousCore.SendMessage(MessageCore.MakeTriggerSkillMesssage(aChief, this, new ChiefBase[] { }, new Card[] { }));
                    aData.Game.TakeingCards(aChief, 1);
                    break;
            }
        }
    }

    internal class SkillQiCai : SkillBase
    {
        public SkillQiCai()
            : base("奇才", SkillEnabled.Passive, false)
        {

        }

        public override byte CalcKitDistance(ChiefBase aChief, byte aOldRange, GlobalData aData)
        {
            return 100; //是的,这就是距离无限 :)
        }
    }
}
