using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common;

namespace ParallelTestRunner.VSTest.Impl
{
    public class VSTestExecutorImpl : IExecutor
    {
        public IProcessStartInfoFactory ProcessStartInfoFactory { get; set; }
        
        public IProcessFactory ProcessFactory { get; set; }
        
        public IProcessOutputReaderFactory ProcessOutputReaderFactory { get; set; }
        
        public IVSTestFileHelper FileHelper { get; set; }

        public void Run(RunData data)
        {
            FileHelper.CreateSettingsFile(data);
            FileHelper.CreateExecutionFolder(data);
            var info = ProcessStartInfoFactory.CreateProcessStartInfo(data);
            FileHelper.CreateInputFile(data, info);
            var processReader = ProcessOutputReaderFactory.CreateReader(data);
            ProcessFactory.StartProcessAndWait(info, processReader);
            FileHelper.CreateOutputFile(data);
        }
    }
}