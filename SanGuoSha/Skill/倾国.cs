using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SQingGuo: AdvSkill, ITransformStyleSkill
    {
        public SQingGuo(): base(aSkillName: "倾国", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return aAskFor == AskForEnum.闪 && aPlayer.HasHand;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards.Length != 1 || !aPlayer.HasCardsInHand(aCards)) return false;
            return aAskFor == AskForEnum.闪 && aCards[0].Color == Card.CardColor.Black;
        }

        public AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            aAnswer.Effect = CardEffect.闪;
            return aAnswer;
        }
    }
}
