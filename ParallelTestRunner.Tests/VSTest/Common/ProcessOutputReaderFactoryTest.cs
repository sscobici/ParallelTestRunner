using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common;
using ParallelTestRunner.VSTest.Common.Impl;

namespace ParallelTestRunner.Tests.VSTest.Common
{
    [TestClass]
    public class ProcessOutputReaderFactoryTest : TestBase
    {
        private ProcessOutputReaderFactoryImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new ProcessOutputReaderFactoryImpl();
        }

        [TestMethod]
        public void CreateReader()
        {
            RunData input = new RunData();
            IProcessOutputReader reader = target.CreateReader(input);
            Assert.IsNotNull(reader);
        }
    }
}