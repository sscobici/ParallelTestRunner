using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common.Impl;

namespace ParallelTestRunner.Tests.VSTest.Common
{
    [TestClass]
    public class ProcessOutputReaderTest : TestBase
    {
        private RunData runData;
        private ProcessOutputReaderImpl target;

        [TestInitialize]
        public void SetUp()
        {
            runData = new RunData();
            runData.Output = new StringBuilder();
            target = new ProcessOutputReaderImpl(runData);
        }

        [TestMethod]
        public void OnDataReceived()
        {
            string input = "AAAAAAAA BBBBB CCCCCCC";
            target.OnDataReceived(input);
            Assert.AreEqual(input + "\r\n", runData.Output.ToString());
        }

        [TestMethod]
        public void OnDataReceived_NullEmpty()
        {
            string input = null;
            target.OnDataReceived(input);
            Assert.AreEqual(string.Empty, runData.Output.ToString());

            input = string.Empty;
            target.OnDataReceived(input);
            Assert.AreEqual(string.Empty, runData.Output.ToString());
        }

        [TestMethod]
        public void OnDataReceived_Passed()
        {
            string input = "Passed  AAAAAAAA BBBBB CCCCCCC";
            target.OnDataReceived(input);
            Assert.AreEqual(input + "\r\n", runData.Output.ToString());
        }

        [TestMethod]
        public void OnDataReceived_Failed()
        {
            string input = "Failed  AAAAAAAA BBBBB CCCCCCC";
            target.OnDataReceived(input);
            Assert.AreEqual(input + "\r\n", runData.Output.ToString());
        }

        [TestMethod]
        public void OnDataReceived_Skipped()
        {
            string input = "Skipped  AAAAAAAA BBBBB CCCCCCC";
            target.OnDataReceived(input);
            Assert.AreEqual(input + "\r\n", runData.Output.ToString());
        }
    }
}