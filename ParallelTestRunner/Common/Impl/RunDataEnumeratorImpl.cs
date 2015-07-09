using System.Collections.Generic;
using System.Linq;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.Common.Impl
{
    public class RunDataEnumeratorImpl : IRunDataEnumerator
    {
        private LinkedList<RunData> items;
        private LinkedListNode<RunData> current;
        private LinkedListNode<RunData> lastPeeked;

        public RunDataEnumeratorImpl(IList<RunData> items)
        {
            this.items = new LinkedList<RunData>(items);
            current = this.items.First;
        }

        public RunData PeekNext()
        {
            if (null == current)
            {
                current = items.First;
                return null;
            }

            lastPeeked = current;
            current = current.Next;
            return lastPeeked.Value;
        }

        public void MoveNext()
        {
            items.Remove(lastPeeked);
        }

        public bool HasItems()
        {
            return items.Count > 0;
        }
    }
}