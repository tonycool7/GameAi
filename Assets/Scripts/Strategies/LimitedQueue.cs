using System.Collections;
using System.Collections.Generic;

// A data structure that allows adding at one end and removing at the other
// It is limited to queueMax elements
// Using LinkedList instead of Queue, as it allows easier access to both ends of data – a deque
public class LimitedQueue<T> : LinkedList<T>
{
    int queueMax = 3;

    public void Enqueue(T item)
    {
        if (this.Count >= queueMax)
        {
            this.RemoveLast();
        }
        base.AddFirst(item);
    }

    public T Dequeue()
    {
        T last = this.Last.Value;
        this.RemoveLast();
        return last;
    }
}
