using System.Diagnostics;
using ParallelTestRunner;

namespace ParallelTestRunner.VSTest.Common.Impl
{
    public class ProcessFactoryImpl : IProcessFactory
    {
        public void StartProcessAndWait(ProcessStartInfo info, IProcessOutputReader processReader)
        {
            Process2.Process2 process = new Process2.Process2() { StartInfo = info };
            process.OutputDataReceived += (sender, args) => processReader.OnDataReceived(args.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }
    }
}