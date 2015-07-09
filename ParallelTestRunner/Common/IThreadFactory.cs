using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelTestRunner.Common
{
    public interface IThreadFactory
    {
        IExecutorThread Create(IExecutor executor, RunData data, IThreadStarter starter);

        Task[] GetTaskArray(IList<IExecutorThread> items);

        bool CanLaunch(IList<IExecutorThread> threads, RunData runData);

        void WaitAny(Task[] array);

        void WaitAll(Task[] array);
    }

    public interface IThreadStarter
    {
        void OnStarted(IExecutorThread thread);
    }

    public interface IThreadEnder
    {
        void OnEnded(IExecutorThread thread);
    }
}