using System.Threading.Tasks;

namespace ParallelTestRunner.Common
{
    public interface IExecutorThread
    {
        void Launch(IThreadEnder ender);

        IExecutor GetExecutor();

        Task GetTask();

        RunData GetRunData();
    }
}