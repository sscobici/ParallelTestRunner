using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestGroup("Group2", Exclusive = true)]
    public class Class2
    {
        [TestMethod]
        public void Class2TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class2TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class2TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }
    }
}
