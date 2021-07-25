
using System.Collections.Generic;

namespace NetWork
{
    public class TcpSafeQueue<T>
    {
        private Queue<T> queue;

        public TcpSafeQueue(int capacity)
        {
            queue = new Queue<T>(capacity);
        }

        public void Enqueue(T item)
        {
            lock (this)
            {
                queue.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (this)
            {
                if (queue.Count > 0)
                    return queue.Dequeue();
            }
            return default(T);
        }
    }
}