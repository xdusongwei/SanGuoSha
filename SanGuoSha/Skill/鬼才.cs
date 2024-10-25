using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SGuiCai: SkillBase
    {
        public SGuiCai(): base(aSkillName: "鬼才", aEnabled: SkillEnabled.Passive, aIsMajestySkill: false) {}

        public override Card OnSentenceCardShow(PlayerBase aTrialPlayer, Card aSentenceCard, PlayerBase aAskForPlayer, BattlefieldBase aBattlefield)
        {
            if (!aAskForPlayer.HasHand) return aSentenceCard;
            using var aa = aBattlefield.NewAsk();
            var response = aa.AskForCards(AskForEnum.鬼才改判, aAskForPlayer);
            if (response.Cards.Length != 1) return aSentenceCard;
            var newSentenceCard = response.Cards.First();
            using var collector = aBattlefield.NewCollector();
            // 最终的新判定牌只能被框架回收, 重复的牌进入牌回收器会引起异常, 而且判定牌不涉及私密, 
            // 所以新判定牌让牌回收器收了再捞出, 期间执行了从玩家那里移除牌的逻辑
            if(collector.DropPlayerReponse(response))
            {
                collector.Pick(newSentenceCard);
                collector.ForceDropCard(aSentenceCard);
                return newSentenceCard;
            }
            return aSentenceCard;
        }
    }
}
