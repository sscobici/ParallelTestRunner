using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestClassGroup("Group1")]
    public class Class7
    {
        [TestMethod]
        public void Class7TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class7TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class7TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }
    }
}
