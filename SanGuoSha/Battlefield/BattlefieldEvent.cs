using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield
    {
        public class EventRecordArgs(EventRecord aRecord)
        {
            public EventRecord Record { get; } = aRecord;
        }

        public delegate void NewEventRecordEventHandler(object sender, EventRecordArgs e);
        public event NewEventRecordEventHandler? NewEventRecordEvent;
        public delegate void AskEventHandler(object sender, AskAnswer.AskArgs e);
        public event AskEventHandler? AskEvent;
        public delegate void AnswerEventHandler(object sender, AskAnswer.AnswerArgs e);
        public event AnswerEventHandler? AnswerEvent;

        public delegate void CreateActionNodeHandler(object sender, ActionNode e);
        public event CreateActionNodeHandler? CreateActionNodeEvent;
    }
}
