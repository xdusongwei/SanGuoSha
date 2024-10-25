using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield
    {
        /// <summary>
        /// 存储每个子事件的节点
        /// </summary>
        private readonly Queue<EventRecord> _queRecord = new();

        public override void NewEventNode(EventRecord aEvent)
        {
            _queRecord.Enqueue(aEvent);
            NewEventRecordEvent?.Invoke(this, new EventRecordArgs(aEvent));
        }

        private void ClearEventNodes()
        {
            _queRecord.Clear();
        }

        private EventRecord PopEventNode()
        {
            return _queRecord.Dequeue();
        }

        private int EventNodeSize()
        {
            return _queRecord.Count;
        }
    }
}
