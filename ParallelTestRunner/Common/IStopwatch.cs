using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common
{
    public interface IStopwatch
    {
        void Start();

        void Stop();

        TimeSpan Elapsed();

        DateTime GetStartDateTime();

        DateTime GetStopDateTime();
    }
}
