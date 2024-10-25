

namespace SanGuoSha.BaseClass
{
    public partial class AskAnswer: IDisposable
    {
        public bool ThrowWrongLeader = true;

        public const int InvisableHandCard = -255;

        private readonly BattlefieldBase Battlefield;

        private readonly IAskAnswerContext IAnswer;

        public readonly int MaxTimeout;

        private readonly SemaphoreSlim SemaphoreSlim = new(0, 1);

        private volatile bool isFinish = false;
        
        private bool EnableCardIdArg = false;

        public PlayerBase[] AskPlayers = [];

        public AskForEnum AskFor
        {
            get;
            set;
        } = AskForEnum.None;

        public PlayerBase? Target = null;

        private AskForResult Response
        {
            get;
            set;
        }

        private struct AnswerFunctionArgs(string aUID)
        {
            public string UID = aUID;
            public int[]? CardIDs = null;
            public string[]? TargetUIDs = null;
            public string? SkillName = null;
            public bool YN = false;
            public string? Suit = null;
            public string[]? ChiefNames = null;
            public string? WeaponEffect = null;
            public string? ArmorEffect = null;
            public int[]? GuanXingTop = null;
            public int[]? GuanXingBottom = null;
        }

        public AskAnswer(
            BattlefieldBase aBattlefield, 
            IAskAnswerContext aIAnswer,
            int aMaxTimeout = 5000
        )
        {
            Battlefield = aBattlefield;
            IAnswer = aIAnswer;
            MaxTimeout = aMaxTimeout;
            Response = new AskForResult(
                aTimeout: true,
                aAskFor: AskFor,
                aLeader: null!,
                aCardFrom: null!,
                aTargets: [],
                aCards: [],
                aEffect: CardEffect.None,
                aYN: false,
                aSkill: null,
                aChiefs: [],
                aSuit: Card.Suits[Battlefield.GetRandom(Card.Suits.Length)]
            );
        }

        /// <summary>
        /// 回应方法.
        /// </summary>
        /// <param name="aUID"></param>
        /// <param name="aCardIDs"></param>
        /// <param name="aTargetUIDs"></param>
        /// <param name="aSkillName"></param>
        /// <param name="aYN"></param>
        /// <param name="aSuit"></param>
        /// <param name="aChiefNames"></param>
        /// <param name="aWeaponEffect"></param>
        /// <param name="aArmorEffect"></param>
        /// <exception cref="MultiTriggerError"></exception>
        /// <exception cref="WrongLeaderError"></exception>
        /// <exception cref="ConvertCardMismatch"></exception>
        public void Answer(
            string aUID, 
            int[]? aCardIDs = null, 
            string[]? aTargetUIDs = null, 
            string? aSkillName = null, 
            bool aYN = false, 
            string? aSuit = null, 
            string[]? aChiefNames = null,
            string? aWeaponEffect = null,
            string? aArmorEffect = null,
            int[]? aGuanXingTop = null,
            int[]? aGuanXingBottom = null
        )
        {
            if(isFinish) return;
            if(!AnswerLeaderWeaponArmorSkillCheck(aUID, aSkillName, aWeaponEffect, aArmorEffect)) return;
            var args = new AnswerFunctionArgs(aUID)
            {
                CardIDs = aCardIDs,
                TargetUIDs = aTargetUIDs,
                SkillName = aSkillName,
                YN = aYN,
                Suit = aSuit,
                ChiefNames = aChiefNames,
                WeaponEffect = aWeaponEffect,
                ArmorEffect = aArmorEffect,
                GuanXingTop = aGuanXingTop,
                GuanXingBottom = aGuanXingBottom,
            };
            
            if(AskFor == AskForEnum.AbandonmentCardNext || AskFor == AskForEnum.AbandonmentCard)
                AskingAbandonment(args);
            else if(AskFor == AskForEnum.Aggressive)
                AskingAggressive(args);
            else if(IAnswer.AnswerCheckMap.TryGetValue(AskFor, out Type? value))
                AskingAnswer(args, value!);
            else 
                throw new ResponseForbiddenError();
        }
    }
}
