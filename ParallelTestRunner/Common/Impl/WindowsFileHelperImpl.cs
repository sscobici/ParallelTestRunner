using System;
using System.IO;
using System.Reflection;

namespace ParallelTestRunner.Common.Impl
{
    public class WindowsFileHelperImpl : IWindowsFileHelper
    {
        public void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string GetFile(string folder, string ext)
        {
            string[] files = Directory.GetFiles(folder, "*.trx");
            if (files.Length == 0)
                throw new ResultNotfoundException();
            return files[0];
        }

        public Stream OpenFile(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public Stream CreateFile(string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        }

        public void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

        public Assembly GetAssembly(string path)
        {
            return Assembly.LoadFrom(path);
        }

        public Stream OpenResultFile(ITestRunnerArgs args)
        {
            if (Path.IsPathRooted(args.Output))
            {
                return CreateFile(args.Output);
            }

            return CreateFile(args.Root + "\\" + args.Output);
        }

        public bool FolderExist(string path)
        {
            return Directory.Exists(path);
        }
    }
}