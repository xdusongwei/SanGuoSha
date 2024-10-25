using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    /// <summary>
    /// 玩家
    /// </summary>
    public partial class Player
    {
        public override Card[] AutoAbandonment()
        {
            if (Hands.Count <= Health) return [];
            return [.. Hands.GetRange(0, Hands.Count - Health)];
        }

        public override Card? AutoSelectSlot(CardSlot aSlot)
        {
            foreach(var c in aSlot.Cards)
                if(aSlot.CardInSlotAndNoOneChoose(c)) return c;
            return null;
        }
    }
}
