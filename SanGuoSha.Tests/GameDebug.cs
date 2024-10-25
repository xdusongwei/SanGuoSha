using SanGuoSha.BaseClass;
using SanGuoSha.Battlefield;


namespace SanGuoSha.Tests
{
    public class GameDebug: Battlefield.Battlefield
    {
        public GameDebug(
            Card[] aCards,
            IEnumerable<Action<AskAnswer>> aActions,
            int aPlayerCount = 2, 
            int aMaxTurns = 1,
            GameMode aMode = GameMode.FiveSTD,
            sbyte aInitHealth = 2
        ): base()
        {
            Mode = aMode;
            MaxTurns = aMaxTurns;
            AnswerTimeout = 1;
            if(aPlayerCount < 2 || aPlayerCount > 8) throw new Exception("玩家数量范围不支持");

            char startLetter = 'A';
            for (int i = 0; i < aPlayerCount; i++)
            {
                char letter = (char)(startLetter + i);
                var player = new Player(letter.ToString())
                {
                    MaxHealth = aInitHealth,
                    Health = aInitHealth,
                    Role = i == 0 ? PlayerRole.Majesty : PlayerRole.Insurgent,
                };
                Players.AddPlayer(player);
            }

            CardsHeap.ShuffleTrashHeap = false;
            CardsHeap.AddCards(aCards);
            Answers = new Queue<Action<AskAnswer>>(aActions);

            TakingCardsCount = 0;
        }

        public Queue<Action<AskAnswer>> Answers{
            get;
            set;
        } = [];

        public bool ThrowWrongLeader = true;

        public bool IgnoreResponseForbidden = true;

        public override AskAnswer NewAsk()
        {
            if(Answers.Count == 0) 
                throw new Exception("应答队列为空");
            var callback = Answers.Dequeue();
            var aa = base.NewAsk();
            aa.ThrowWrongLeader = ThrowWrongLeader;
            aa.BeforeAskEvent += (s, e) => 
            {
                try
                {
                    callback(aa);
                }
                catch(ResponseForbiddenError)
                {
                    if(!IgnoreResponseForbidden)
                        throw;
                }
                catch(ConvertCardMismatch)
                {
                    if(!IgnoreResponseForbidden)
                        throw;
                }
            };
            return aa;
        }

        public new void LogicLoop(PlayerBase aPlayerStart, bool aIgnoreTakeCards)
        {
            base.LogicLoop(aPlayerStart, aIgnoreTakeCards);
            if(Answers.Count > 0)
            {
                throw new Exception($"应答队列仍有{Answers.Count}项元素");
            }
        }

        public static Action<AskAnswer> FinishLeading(string aUID)
        {
            return (aa) => aa.Answer(
                aUID: aUID,
                aCardIDs: [],
                aTargetUIDs: []
            );
        }

        public static Action<AskAnswer> DoNothing(string aUID)
        {
            return FinishLeading(aUID);
        }

        public static Action<AskAnswer> Yes(string aUID)
        {
            return (aa) => aa.Answer(
                aUID: aUID,
                aYN: true
            );
        }
    }
}