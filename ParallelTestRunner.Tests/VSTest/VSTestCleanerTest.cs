using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Collections.Generic;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Tests.VSTest
{
    [TestClass]
    public class VSTestCleanerTest : TestBase
    {
        private IVSTestFileHelper fileHelper;
        private VSTestCleanerImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new VSTestCleanerImpl();
            fileHelper = Stub<IVSTestFileHelper>();
            target.FileHelper = fileHelper;
        }

         [TestMethod]
         public void Clean()
         {
             RunData a = new RunData();
             RunData b = new RunData();
             IList<RunData> items = new List<RunData> { a, b };

             using (Ordered())
             {
                 fileHelper.Expect(m => m.CleanRootFolder(a));
                 fileHelper.Expect(m => m.CleanRootFolder(b));
             }

             VerifyTarget(() => target.Clean(items));
         }
    }
}