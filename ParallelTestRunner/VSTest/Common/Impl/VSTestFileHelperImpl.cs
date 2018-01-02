using System.Diagnostics;
using System.IO;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common.Impl
{
    public class VSTestFileHelperImpl : IVSTestFileHelper
    {
        public IWindowsFileHelper WindowsFileHelper { get; set; }

        public void CreateSettingsFile(RunData data)
        {
            string settingFileName = string.Concat(data.Root, "\\", data.RunId, ".settings");
            string platform = System.Environment.Is64BitProcess ? "x64" : "x86";
            string settingsFileContent = string.Concat(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><RunSettings><RunConfiguration><ResultsDirectory>",
                data.RunId,
                "</ResultsDirectory><TargetPlatform>"+platform+"</TargetPlatform></RunConfiguration></RunSettings>");

            WindowsFileHelper.WriteFile(settingFileName, settingsFileContent);
        }

        public void DeleteSettingsFile(RunData data)
        {
            WindowsFileHelper.DeleteFile(
                string.Concat(data.Root, "\\", data.RunId, ".settings"));
        }

        public void CreateInputFile(RunData data, ProcessStartInfo processStartInfo)
        {
            WindowsFileHelper.WriteFile(
                string.Concat(data.Root, "\\", data.RunId, "\\cmd.txt"),
                string.Concat(processStartInfo.FileName, " ", processStartInfo.Arguments));
        }

        public void CreateExecutionFolder(RunData data)
        {
            WindowsFileHelper.CreateFolder(
                string.Concat(data.Root, "\\", data.RunId));
        }

        public void CreateOutputFile(RunData data)
        {
            WindowsFileHelper.WriteFile(
                string.Concat(data.Root, "\\", data.RunId, "\\output.txt"),
                data.Output.ToString());
        }

        public Stream OpenTrxFile(RunData data)
        {
            string trxPath = WindowsFileHelper.GetFile(string.Concat(data.Root, "\\", data.RunId), "*.trx");
            return WindowsFileHelper.OpenFile(trxPath);
        }

        public void CleanRootFolder(RunData data)
        {
            WindowsFileHelper.DeleteFolder(string.Concat(data.Root, "\\", data.RunId));
            WindowsFileHelper.DeleteFile(string.Concat(data.Root, "\\", data.RunId, ".settings"));
        }
    }
}