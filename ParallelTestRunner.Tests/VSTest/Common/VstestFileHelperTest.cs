using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ParallelTestRunner.Common;
using ParallelTestRunner.VSTest.Common.Impl;

namespace ParallelTestRunner.Tests.VSTest.Common
{
    [TestClass]
    public class VSTestFileHelperTest : TestBase
    {
        private VSTestFileHelperImpl target;
        private RunData input;
        private IWindowsFileHelper fileHelper;

        [TestInitialize]
        public void SetUp()
        {
            fileHelper = Stub<IWindowsFileHelper>();
            target = new VSTestFileHelperImpl();
            target.WindowsFileHelper = fileHelper;
            input = new RunData()
            {
                Root = "ROOT",
                RunId = Guid.NewGuid(),
                Output = new StringBuilder("THE CONTENT OF THE FILE")
            };
        }

        [TestMethod]
        public void CreateSettingsFile()
        {
            string path = string.Concat(input.Root, "\\", input.RunId, ".settings");
            string content = string.Concat(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><RunSettings><RunConfiguration><ResultsDirectory>",
                input.RunId,
                "</ResultsDirectory></RunConfiguration></RunSettings>");

            fileHelper.Expect((m) => m.WriteFile(path, content));
            VerifyTarget(() => target.CreateSettingsFile(input));
        }

        [TestMethod]
        public void DeleteSettingsFile()
        {
            string path = string.Concat(input.Root, "\\", input.RunId, ".settings");
            fileHelper.Expect((m) => m.DeleteFile(path));
            VerifyTarget(() => target.DeleteSettingsFile(input));
        }

        [TestMethod]
        public void CreateInputFile()
        {
            string path = string.Concat(input.Root, "\\", input.RunId, "\\cmd.txt");
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "FILE_NAME",
                Arguments = "ARGUMENTS"
            };

            fileHelper.Expect((m) => m.WriteFile(path, "FILE_NAME ARGUMENTS"));
            VerifyTarget(() => target.CreateInputFile(input, startInfo));
        }

        [TestMethod]
        public void CreateExecutionFolder()
        {
            string path = string.Concat(input.Root, "\\", input.RunId);
            fileHelper.Expect((m) => m.CreateFolder(path));
            VerifyTarget(() => target.CreateExecutionFolder(input));
        }

        [TestMethod]
        public void CreateOutputFile()
        {
            string path = string.Concat(input.Root, "\\", input.RunId, "\\output.txt");
            string content = input.Output.ToString();
            fileHelper.Expect((m) => m.WriteFile(path, content));
            VerifyTarget(() => target.CreateOutputFile(input));
        }

        [TestMethod]
        public void OpenTrxFile()
        {
            string path = string.Concat(input.Root, "\\", input.RunId);
            string trxPath = "TRX_FILE_PATH.TRX";
            Stream stream = Stub<Stream>();
            fileHelper.Expect((m) => m.GetFile(path, "*.trx")).Return(trxPath);
            fileHelper.Expect((m) => m.OpenFile(trxPath)).Return(stream);
            VerifyTarget(() => target.OpenTrxFile(input));
        }

        [TestMethod]
        public void CleanRootFolder()
        {
            string folderPath = string.Concat(input.Root, "\\", input.RunId);
            string filePath = string.Concat(input.Root, "\\", input.RunId, ".settings");
            fileHelper.Expect((m) => m.DeleteFolder(folderPath));
            fileHelper.Expect((m) => m.DeleteFile(filePath));
            VerifyTarget(() => target.CleanRootFolder(input));
        }
    }
}