using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common.Impl
{
    public class StopwatchImpl : IStopwatch
    {
        private DateTime start;
        private DateTime stop;

        public void Start()
        {
            start = DateTime.UtcNow;
        }

        public void Stop()
        {
            stop = DateTime.UtcNow;
        }

        public TimeSpan Elapsed()
        {
            return stop - start;
        }

        public DateTime GetStartDateTime()
        {
            return start;
        }

        public DateTime GetStopDateTime()
        {
            return stop;
        }
    }
}
