using SanGuoSha.BaseClass;


namespace SanGuoSha.Skill
{
    internal class SRenDe: AdvSkill, IAggressiveStyleSkill
    {
        public SRenDe(): base(aSkillName: "仁德", aEnabled: SkillEnabled.Disable, aIsMajestySkill: false) 
        {
            AggressiveMaxTimes = -1;
            AutoCollectAggressiveResponse = false;
        }

        private int TotalCount = 0;

        private bool HasRegainHealth = false;

        public override void BeforeTurnStart(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            TotalCount = 0;
            HasRegainHealth = false;
        }

        public override void AfterTurnEnd(PlayerBase aPlayer, BattlefieldBase aBattlefield)
        {
            TotalCount = 0;
            HasRegainHealth = false;
        }


        public bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            return true;
        }

        public void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            var target = aAnswer.Targets[0];
            var cards = aAnswer.Cards;
            if(aBattlefield.Move(aAnswer.Leader, target, cards))
            {
                TotalCount += cards.Length;
                if (TotalCount > 1 && !HasRegainHealth)
                {
                    HasRegainHealth = true;
                    aBattlefield.RegainHealth(aAnswer.Leader, 1);
                }
            }
        }
    }
}
