using System.Diagnostics;
using System.IO;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Common
{
    public interface IVSTestFileHelper
    {
        void CreateSettingsFile(RunData data);

        void DeleteSettingsFile(RunData data);

        void CreateInputFile(RunData data, ProcessStartInfo processStartInfo);

        void CreateExecutionFolder(RunData data);

        void CreateOutputFile(RunData data);

        void CleanRootFolder(RunData data);

        Stream OpenTrxFile(RunData data);
    }
}