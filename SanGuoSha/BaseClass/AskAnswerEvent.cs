

namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer
    {
        public class AskArgs
        {
            public AskArgs(PlayerBase[] aPlayers, AskForEnum aAskFor) { LeadingPlayers = aPlayers; AskFor = aAskFor; }
            public PlayerBase[] LeadingPlayers { get; }
            public AskForEnum AskFor { get; }
        }

        public class AnswerArgs
        {
            public AnswerArgs(AskForResult aAnswer) { AskForResult = aAnswer; }
            public AskForResult AskForResult { get; }
        }

        public delegate void BeforeAskEventHandler(object sender, AskArgs e);
        public event BeforeAskEventHandler? BeforeAskEvent;

        public delegate void AferAnswerEventHandler(object sender, AnswerArgs e);
        public event AferAnswerEventHandler? AfterAnswerEvent;
    }
}
