using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Diagnostics;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Tests.VSTest
{
    [TestClass]
    public class VSTestExecutorTest : TestBase
    {
        private VSTestExecutorImpl target;
        private IProcessStartInfoFactory processStartInfoFactory;
        private IProcessFactory processFactory;
        private IProcessOutputReaderFactory processOutputReaderFactory;
        private IVSTestFileHelper fileHelper;

        [TestInitialize]
        public void SetUp()
        {
            target = new VSTestExecutorImpl();
            processStartInfoFactory = Stub<IProcessStartInfoFactory>();
            processFactory = Stub<IProcessFactory>();
            processOutputReaderFactory = Stub<IProcessOutputReaderFactory>();
            fileHelper = Stub<IVSTestFileHelper>();
            target.ProcessStartInfoFactory = processStartInfoFactory;
            target.ProcessFactory = processFactory;
            target.ProcessOutputReaderFactory = processOutputReaderFactory;
            target.FileHelper = fileHelper;
        }

        [TestMethod]
        public void Run()
        {
            RunData data = new RunData();
            ProcessStartInfo info = new ProcessStartInfo();
            IProcessOutputReader reader = Stub<IProcessOutputReader>();

            using (Ordered())
            {
                fileHelper.Expect(m => m.CreateSettingsFile(data));
                fileHelper.Expect(m => m.CreateExecutionFolder(data));
                processStartInfoFactory.Expect(m => m.CreateProcessStartInfo(data)).Return(info);
                fileHelper.Expect(m => m.CreateInputFile(data, info));
                processOutputReaderFactory.Expect(m => m.CreateReader(data)).Return(reader);
                processFactory.Expect(m => m.StartProcessAndWait(info, reader));
                fileHelper.Expect(m => m.CreateOutputFile(data));
            }

            VerifyTarget(() => target.Run(data));
        }
    }
}
