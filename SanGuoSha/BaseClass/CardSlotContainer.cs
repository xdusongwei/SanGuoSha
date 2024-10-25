using System.Collections;


namespace SanGuoSha.BaseClass
{
    /// <summary>
    /// 牌槽容器类
    /// </summary>
    public class CardSlotContainer: IEnumerable<CardSlot>
    {
        public List<CardSlot> Slots
        {
            get;
            private set;
        } = [];

        public CardSlotContainer()
        {
            
        }

        public bool HasName(string aName) => Slots.Any(c => c.SlotName == aName);

        public CardSlot this[string aName] => Slots.Find(c => c.SlotName == aName)!;

        public IEnumerator<CardSlot> GetEnumerator()
        {
            return Slots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
