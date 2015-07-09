using System;
using System.Collections.Generic;

namespace ParallelTestRunner.Common
{
    public interface IRunDataListBuilder
    {
        void Add(IList<RunData> items);
        
        IList<RunData> GetFull();
        
        IRunDataEnumerator GetEnumerator();
    }
}