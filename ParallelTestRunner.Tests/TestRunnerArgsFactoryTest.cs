using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelTestRunner.Impl;

namespace ParallelTestRunner.Tests
{
    [TestClass]
    public class TestRunnerArgsFactoryTest : TestBase
    {
        private TestRunnerArgsFactoryImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new TestRunnerArgsFactoryImpl();
        }

        [TestMethod]
        public void ParseArgs_Provider()
        {
            string[] input = new string[] { "abc.dll", "provider:THE_BEST_PROVIDER_EVER" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual("THE_BEST_PROVIDER_EVER", args.Provider);
        }

        [TestMethod]
        public void ParseArgs_ThreadCount()
        {
            string[] input = new string[] { "abb.dll", "threadcount:8" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual(8, args.ThreadCount);
        }

        [TestMethod]
        public void ParseArgs_Root()
        {
            string[] input = new string[] { "bbc.dll", "root:THE_ROOT_OF_THE_HAVEN" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual("THE_ROOT_OF_THE_HAVEN", args.Root);
        }

        [TestMethod]
        public void ParseArgs_Root_EndsWithSlash()
        {
            string[] input = new string[] { "bbc.dll", "root:THE_ROOT_OF_THE_HAVEN/" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual("THE_ROOT_OF_THE_HAVEN", args.Root);
        }

        [TestMethod]
        public void ParseArgs_Out()
        {
            string[] input = new string[] { "cab.dll", "out:THE_EPIC_OUTPUT" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual("THE_EPIC_OUTPUT", args.Output);
        }

        [TestMethod]
        public void ParseArgs_PLevel()
        {
            string[] input = new string[] { "cab.dll", "plevel:testmethod" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual(PLevel.TestMethod, args.PLevel);
        }

        [TestMethod]
        public void ParseArgs_AssemblyList()
        {
            string[] input = new string[] { "cab.dll", "abc.dll", "ccb.dll" };
            ITestRunnerArgs args = target.ParseArgs(input);
            Assert.AreEqual(3, args.AssemblyList.Count);
        }
    }
}