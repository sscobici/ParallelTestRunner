namespace ParallelTestRunner
{
    public interface ITestRunner
    {
        void Parse();
        
        void Execute();
        
        void WriteTrx();
        
        void Clean();
    }
}