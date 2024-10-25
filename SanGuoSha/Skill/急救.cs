using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SJiJiu: AdvSkill, ITransformStyleSkill
    {
        public SJiJiu(): base(aSkillName: "急救", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) {}

        public bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return (aAskFor == AskForEnum.桃 || aAskFor == AskForEnum.桃或酒) && aBattlefield.ActionPlayerData.CurrentPlayer != aPlayer && aPlayer.HasCard;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if (aCards[0].Color != Card.CardColor.Red) return false;
            return true;
        }

        public AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            aAnswer.Effect = CardEffect.桃;
            return aAnswer;
        }
    }
}
