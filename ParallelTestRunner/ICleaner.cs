using System.Collections.Generic;
using ParallelTestRunner.Common;

namespace ParallelTestRunner
{
    public interface ICleaner
    {

        void Clean(IList<RunData> items);
    }
}
