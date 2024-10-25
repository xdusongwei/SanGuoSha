using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SJiJiang: AdvSkill, IAggressiveStyleSkill, ITransformStyleSkill
    {
        public SJiJiang(): base(aSkillName: "激将", aEnabled: SkillEnabled.Disable, aIsMajestySkill: true) {}

        public bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            if(aPlayer.Role != PlayerRole.Majesty) return false;
            if(aAskFor != AskForEnum.杀) return false;
            if(!aBattlefield.Players.Any(i => i != aPlayer && !i.Dead && i.Chief.ChiefCamp == ChiefBase.Camp.蜀)) return false;
            return true;
        }

        bool ITransformStyleSkill.Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return aCards.Length == 0;
        }

        public AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            return ShaAskLoop(aAnswer.Leader, aBattlefield) ?? aAnswer;
        }

        bool IAggressiveStyleSkill.Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return true;
        }

        public bool Prepare(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var shaType = aBattlefield.AggressiveCards[CardEffect.杀];
            return aBattlefield.CheckLeadingAnswer(shaType, aAnswer);
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var shaAnswer = ShaAskLoop(aAnswer.Leader, aBattlefield);
            if(shaAnswer == null || shaAnswer.Effect != CardEffect.杀) return;
            using var collector = aBattlefield.NewCollector();
            if(collector.DropPlayerReponse(shaAnswer))
            {
                foreach(var t in aAnswer.Targets)
                {
                    var er = new EventRecord(
                        aSource: aAnswer.Leader, 
                        aTarget: t,
                        aEffect: CardEffect.杀, 
                        aCards: aAnswer.Cards,
                        aSkill: this,
                        aIgnoreWuXie: true
                    );
                    aBattlefield.NewEventNode(er);
                }
            }
        }

        private static AskForResult? ShaAskLoop(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            var myself = aPlayer;
            var player = myself;
            while (aBattlefield.Players.NextAliveUntilNullOrStop(ref player, myself))
            {
                if (player.Chief.ChiefCamp != ChiefBase.Camp.蜀) continue;
                using var aa = aBattlefield.NewAsk();
                var response = aa.AskForCards(AskForEnum.杀, player);
                if(response.Effect == CardEffect.杀)
                    return response;
            }
            return null;
        }
    }
}
