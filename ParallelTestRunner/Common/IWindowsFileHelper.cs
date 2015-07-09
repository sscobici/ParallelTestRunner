using System;
using System.IO;
using System.Reflection;

namespace ParallelTestRunner.Common
{
    public interface IWindowsFileHelper
    {
        void WriteFile(string path, string content);

        void DeleteFile(string path);

        void CreateFolder(string path);

        string GetFile(string folder, string ext);

        Stream OpenFile(string path);

        Stream CreateFile(string path);

        void DeleteFolder(string path);

        Assembly GetAssembly(string path);

        Stream OpenResultFile(ITestRunnerArgs args);
    }
}