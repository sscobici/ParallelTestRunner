using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Collections.Generic;
using System.IO;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Trx;
using ParallelTestRunner.VSTest;
using ParallelTestRunner.VSTest.Common;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Tests.VSTest
{
    [TestClass]
    public class VSTestCollectorTest : TestBase
    {
        private ITrxParser trxParser;
        private IVSTestFileHelper fileHelper;
        private VSTestCollectorImpl target;

        [TestInitialize]
        public void SetUp()
        {
            target = new VSTestCollectorImpl();
            fileHelper = Stub<IVSTestFileHelper>();
            trxParser = Stub<ITrxParser>();
            target.FileHelper = fileHelper;
            target.TrxParser = trxParser;
        }

        [TestMethod]
        public void Collect()
        {
            RunData a = new RunData();
            RunData b = new RunData();
            RunData c = new RunData();
            IList<RunData> items = new List<RunData> { a, b, c };
            ResultFile fileA = new ResultFile();
            ResultFile fileB = new ResultFile();
            ResultFile fileC = new ResultFile();
            Stream streamA = Stub<Stream>();
            Stream streamB = Stub<Stream>();
            Stream streamC = Stub<Stream>();
            
            using (Ordered())
            {
                fileHelper.Expect(m => m.OpenTrxFile(a)).Return(streamA);
                trxParser.Expect(m => m.Parse(streamA)).Return(fileA);
                fileHelper.Expect(m => m.OpenTrxFile(b)).Return(streamB);
                trxParser.Expect(m => m.Parse(streamB)).Return(fileB);
                fileHelper.Expect(m => m.OpenTrxFile(c)).Return(streamC);
                trxParser.Expect(m => m.Parse(streamC)).Return(fileC);
            }

            IList<ResultFile> actual = VerifyTarget(() => target.Collect(items));
            Assert.AreEqual(fileA, actual[0]);
            Assert.AreEqual(fileB, actual[1]);
            Assert.AreEqual(fileC, actual[2]);
        }
    }
}