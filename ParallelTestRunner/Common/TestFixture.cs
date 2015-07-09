using System;

namespace ParallelTestRunner.Common
{
    public class TestFixture
    {
        public string Name { get; set; }
        
        public string Group { get; set; }
        
        public bool Exclusive { get; set; }
    }
}