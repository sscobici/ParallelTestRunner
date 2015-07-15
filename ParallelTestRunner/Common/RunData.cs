using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelTestRunner.Common
{
    public class RunData
    {
        public string Executable { get; set; }

        public string AssemblyName { get; set; }

        public ISet<string> Groups { get; set; }

        public bool Exclusive { get; set; }

        public IList<TestFixture> Fixtures { get; set; }

        public string Root { get; set; }

        public Guid RunId { get; set; }

        public StringBuilder Output { get; set; }
    }
}