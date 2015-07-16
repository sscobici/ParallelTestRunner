using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UITests
{
    [TestClass]
    [TestGroup("Group1", false)]
    public class Class1
    {
        [TestMethod]
        public void Class1TestMethod1()
        {
            Thread.Sleep(1000);
            Assert.AreEqual(1, 1);
        }

        [Ignore]
        [TestMethod]
        public void Class1TestMethod2()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(2, 2);
        }

        [TestMethod]
        public void Class1TestMethod3()
        {
            Thread.Sleep(3000);
            Assert.AreEqual(3, 2);
        }
    }
}
