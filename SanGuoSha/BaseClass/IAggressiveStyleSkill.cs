

namespace SanGuoSha.BaseClass
{
    public interface IAggressiveStyleSkill
    {
        virtual bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            throw new NotImplementedException();
        }

        virtual bool Prepare(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            return true;
        }

        virtual void Proc(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            throw new NotImplementedException();
        }
    }
}
