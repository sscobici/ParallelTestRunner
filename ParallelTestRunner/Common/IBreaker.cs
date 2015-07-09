using System;

namespace ParallelTestRunner.Common
{
    public interface IBreaker
    {
        bool IsBreakReceived();

        void Break();
    }
}