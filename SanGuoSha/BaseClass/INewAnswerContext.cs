


namespace SanGuoSha.BaseClass
{
    public interface IAskAnswerContext
    {
        virtual AskForResult NewAnswer(AskForResult aAnswer)
        {
            throw new NotImplementedException();
        }

        virtual Stack<Tuple<string, AskForEnum>> AskStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IReadOnlyDictionary<string, Type> AggressiveSkillCheckMap
        {
            get;
        }

        IReadOnlyDictionary<string, Type> TransformSkillCheckMap
        {
            get;
        }

        IReadOnlyDictionary<AskForEnum, Type> AnswerCheckMap
        {
            get;
        }
    }
}