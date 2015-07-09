using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class RunDataListBuilderTest : TestBase
    {
        private RunDataListBuilderImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new RunDataListBuilderImpl();
        }

        [TestMethod]
        public void Add()
        {
            RunData a = new RunData();
            RunData b = new RunData();
            RunData c = new RunData();
            RunData d = new RunData();
            IList<RunData> list1 = new List<RunData>() { a, b };
            IList<RunData> list2 = new List<RunData>() { c, d };
            target.Add(list1);
            target.Add(list2);

            IList<RunData> full = target.GetFull();
            Assert.AreEqual(a, full[0]);
            Assert.AreEqual(b, full[1]);
            Assert.AreEqual(c, full[2]);
            Assert.AreEqual(d, full[3]);
        }

        [TestMethod]
        public void GetEnumerator()
        {
            RunData a = new RunData();
            RunData b = new RunData();
            RunData c = new RunData();
            RunData d = new RunData();
            IList<RunData> list1 = new List<RunData>() { a, b };
            IList<RunData> list2 = new List<RunData>() { c, d };
            target.Add(list1);
            target.Add(list2);

            IRunDataEnumerator enumerator = target.GetEnumerator();
            Assert.IsNotNull(enumerator);
            Assert.AreEqual(a, enumerator.PeekNext());
        }
    }
}