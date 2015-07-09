using System;
using System.Collections.Generic;

namespace ParallelTestRunner.Common
{
    public class TestAssembly
    {
        public string Name { get; set; }

        public IList<TestFixture> Fixtures { get; set; }
    }
}