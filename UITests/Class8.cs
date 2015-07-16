using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestGroup("Group2")]
    public class Class8
    {
        [TestMethod]
        public void Class8TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void Class8TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class8TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }
    }
}
