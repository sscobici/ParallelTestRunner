using System.Collections.Generic;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common;

namespace ParallelTestRunner.VSTest.Impl
{
    public class VSTestCleanerImpl : ICleaner
    {
        public IVSTestFileHelper FileHelper { get; set; }

        public void Clean(IList<RunData> items)
        {
            foreach (var item in items)
            {
                FileHelper.CleanRootFolder(item);
            }
        }
    }
}