using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;

namespace ParallelTestRunner.Tests.Common
{
    [TestClass]
    public class RunDataEnumeratorTest : TestBase
    {
        private RunDataEnumeratorImpl target;
        private RunData runDataA;
        private RunData runDataB;
        private RunData runDataC;

        [TestInitialize]
        public void SetUp()
        {
            runDataA = new RunData();
            runDataB = new RunData();
            runDataC = new RunData();
            target = new RunDataEnumeratorImpl(new List<RunData>() { runDataA, runDataB, runDataC });
        }

        [TestMethod]
        public void PeekNext()
        {
            RunData actualA = target.PeekNext();
            Assert.AreEqual(runDataA, actualA);
            RunData actualB = target.PeekNext();
            Assert.AreEqual(runDataB, actualB);
            RunData actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);

            RunData nullData = target.PeekNext();
            Assert.IsNull(nullData);

            actualA = target.PeekNext();
            Assert.AreEqual(runDataA, actualA);
            actualB = target.PeekNext();
            Assert.AreEqual(runDataB, actualB);
            actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);
        }

        [TestMethod]
        public void MoveNext_MoveFirst()
        {
            RunData actualA = target.PeekNext();
            Assert.AreEqual(runDataA, actualA);
            target.MoveNext();
            RunData actualB = target.PeekNext();
            Assert.AreEqual(runDataB, actualB);
            RunData actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);

            RunData nullData = target.PeekNext();
            Assert.IsNull(nullData);

            actualB = target.PeekNext();
            Assert.AreEqual(runDataB, actualB);
            actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);
        }

        [TestMethod]
        public void MoveNext_MoveMiddle()
        {
            RunData actualA = target.PeekNext();
            Assert.AreEqual(runDataA, actualA);
            RunData actualB = target.PeekNext();
            Assert.AreEqual(runDataB, actualB);
            target.MoveNext();
            RunData actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);

            RunData nullData = target.PeekNext();
            Assert.IsNull(nullData);

            actualA = target.PeekNext();
            Assert.AreEqual(runDataA, actualA);
            actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);
        }

        [TestMethod]
        public void HasItems()
        {
            Assert.IsTrue(target.HasItems());

            RunData actualA = target.PeekNext();
            Assert.AreEqual(runDataA, actualA);
            target.MoveNext();
            Assert.IsTrue(target.HasItems());

            RunData actualB = target.PeekNext();
            Assert.AreEqual(runDataB, actualB);
            target.MoveNext();
            Assert.IsTrue(target.HasItems());

            RunData actualC = target.PeekNext();
            Assert.AreEqual(runDataC, actualC);
            target.MoveNext();
            Assert.IsFalse(target.HasItems());
        }
    }
}