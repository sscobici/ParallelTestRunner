using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ParallelTestRunner.Common.Impl;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class BreakerTest : TestBase
    {
        private BreakerImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new BreakerImpl();
        }

        [TestMethod]
        public void Break()
        {
            Assert.IsFalse(target.IsBreakReceived());
            target.Break();
            Assert.IsTrue(target.IsBreakReceived());
        }
    }
}