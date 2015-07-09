using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelTestRunner.Autofac;
using ParallelTestRunner.Impl;

namespace ParallelTestRunner.Tests.Autofac
{
    [TestClass]
    public class AutofacContainerTest
    {
        private TestRunnerArgsImpl args;

        [TestInitialize]
        public void Setup()
        {
            args = new TestRunnerArgsImpl();
        }

        [TestMethod]
        public void TestVSTest()
        {
            args.Provider = "VSTEST_2012";
            IContainer container = AutofacContainer.RegisterTypes(args);
        }

        [TestMethod]
        public void TestNunit()
        {
            args.Provider = "NUNIT_1.0";
            IContainer container = AutofacContainer.RegisterTypes(args);
        }
    }
}