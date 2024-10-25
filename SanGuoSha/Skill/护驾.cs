using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SHuJia: AdvSkill, ITransformStyleSkill
    {
        public SHuJia(): base(aSkillName: "护驾", aEnabled: SkillEnabled.Disable, aIsMajestySkill: true) {}

        public bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if(aPlayer.Role != PlayerRole.Majesty) return false;
            if(aAskFor != AskForEnum.闪) return false;
            if(!aBattlefield.Players.Any(i => i != aPlayer && !i.Dead && i.Chief.ChiefCamp == ChiefBase.Camp.魏)) return false;
            return true;
        }

        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return aCards.Length == 0;
        }

        public AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var myself = aAnswer.Leader;
            var player = myself;
            while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, myself))
            {
                if (player.Chief.ChiefCamp != ChiefBase.Camp.魏) continue;
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForCards(AskForEnum.闪, player);
                if(response.Effect == CardEffect.闪)
                    return response;
            }
            return aAnswer;
        }
    }
}
