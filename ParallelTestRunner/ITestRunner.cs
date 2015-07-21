namespace ParallelTestRunner
{
    public interface ITestRunner
    {
        int ResultCode { get; }

        void Parse();
        
        void Execute();
        
        void WriteTrx();
        
        void Clean();
    }
}