using System;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.Common
{
    public interface IRunDataEnumerator
    {
        RunData PeekNext();

        void MoveNext();

        bool HasItems();
    }
}