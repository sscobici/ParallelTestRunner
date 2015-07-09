using System.Collections.Generic;

namespace ParallelTestRunner.Common.Impl
{
    public class RunDataListBuilderImpl : IRunDataListBuilder
    {
        private List<RunData> items = new List<RunData>();

        public void Add(IList<RunData> items)
        {
            this.items.AddRange(items);
        }

        public IList<RunData> GetFull()
        {
            return items;
        }

        public IRunDataEnumerator GetEnumerator()
        {
            return new RunDataEnumeratorImpl(items);
        }
    }
}