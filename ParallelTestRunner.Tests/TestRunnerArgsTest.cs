using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ParallelTestRunner.Impl;

namespace ParallelTestRunner.Tests
{
    [TestClass]
    public class TestRunnerArgsTest : TestBase
    {
        private TestRunnerArgsImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new TestRunnerArgsImpl();
            target.Provider = "THE_PROVIDER";
            target.ThreadCount = 555;
            target.Root = "THE_ROOT";
            target.Output = "THE_OUTPUT";
            target.AssemblyList = new List<string> { "assembly_A.dll", "assembly_B.dll" };
        }

        [TestMethod]
        public void GetExecutablePath()
        {
            string path = target.GetExecutablePath();
            Assert.AreEqual("THE_PROVIDER_PATH", path);
        }

        [TestMethod]
        public void IsValid()
        {
            bool valid = target.IsValid();
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void IsValid_NOASSEMBLY()
        {
            target.AssemblyList = new List<string>();
            bool valid = target.IsValid();
            Assert.IsFalse(valid);

            target.AssemblyList = null;
            valid = target.IsValid();
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IsValid_NOPROVIDER()
        {
            target.Provider = string.Empty;
            bool valid = target.IsValid();
            Assert.IsFalse(valid);

            target.Provider = null;
            valid = target.IsValid();
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IsValid_NOTHREADCOUNT()
        {
            target.ThreadCount = 0;
            bool valid = target.IsValid();
            Assert.IsFalse(valid);

            target.ThreadCount = -3;
            valid = target.IsValid();
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void IsValid_NOROOT()
        {
            target.Root = string.Empty;
            bool valid = target.IsValid();
            Assert.IsFalse(valid);

            target.Root = null;
            valid = target.IsValid();
            Assert.IsFalse(valid);
        }
    }
}