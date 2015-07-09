using System.Diagnostics;

namespace ParallelTestRunner.VSTest.Common
{
    public interface IProcessOutputReader
    {
        void OnDataReceived(string data);
    }
}