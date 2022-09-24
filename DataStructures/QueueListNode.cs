namespace DataStructures
{
    public class QueueListNode<T>
    {
        internal QueueList<T> list;
        internal QueueListNode<T> next;
        internal QueueListNode<T> prev;

        public T Value { get; set; }

        public QueueListNode(T value)
        {
            Value = value;
        }
    }
}
