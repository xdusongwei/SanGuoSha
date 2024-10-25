using SanGuoSha.BaseClass;
using SanGuoSha.EquipageProc;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield: BattlefieldBase, IAskAnswerContext
    {
        private readonly Stack<Tuple<string, AskForEnum>> _askStack = [];

        public override AskAnswer NewAsk()
        {
            var aa = new AskAnswer(
                this, 
                this,
                AnswerTimeout
            );
            aa.BeforeAskEvent += (s, e) => AskEvent?.Invoke(s, e);
            aa.AfterAnswerEvent += (s, e) => AnswerEvent?.Invoke(s, e);
            return aa;
        }

        public AskForResult NewAnswer(AskForResult aAnswer)
        {
            if(aAnswer.WeaponEffect != CardEffect.None)
                aAnswer = WeaponProc.OnNewAnswer(aAnswer, this);
            if(aAnswer.Skill != null)
                aAnswer = aAnswer.Skill.OnNewAnswer(aAnswer, this);
            return aAnswer;
        }

        public Stack<Tuple<string, AskForEnum>> AskStack
        {
            get
            {
                return _askStack;
            }
        }

        public IReadOnlyDictionary<string, Type> AggressiveSkillCheckMap
        {
            get
            {
                return AggressiveSkillCheck;
            }
        }

        public IReadOnlyDictionary<string, Type> TransformSkillCheckMap
        {
            get
            {
                return TransformSkillCheck;
            }
        }

        public IReadOnlyDictionary<AskForEnum, Type> AnswerCheckMap
        {
            get
            {
                return AnswerCheck;
            }
        }
    }
}