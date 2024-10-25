

namespace SanGuoSha.BaseClass
{
    public interface ITransformStyleSkill
    {
        virtual bool Trigger(PlayerBase aPlayer, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            throw new NotImplementedException();
        }

        virtual bool Check(Card[] aCards, PlayerBase aPlayer, PlayerBase[] aTargets, AskForEnum aAskFor, BattlefieldBase aBattlefield)
        {
            throw new NotImplementedException();
        }

        virtual AskForResult Transform(AskForResult aAnswer, BattlefieldBase aBattlefield)
        {
            throw new NotImplementedException();
        }
    }
}
