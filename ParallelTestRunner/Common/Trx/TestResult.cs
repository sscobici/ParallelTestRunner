using System;

namespace ParallelTestRunner.Common.Trx
{
    public class TestResult
    {
        public Guid ExecutionId { get; set; }

        public Guid TestId { get; set; }

        public Guid TestListId { get; set; }

        public Guid RelativeResultDirectory { get; set; }

        public string TestName { get; set; }

        public string Outcome { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public string StackTrace { get; set; }
        
        public string Duration { get; set; }
        
        public string ClassName { get; set; }
        
        public string AdapterTypeName { get; set; }
        
        public string ComputerName { get; set; }
        
        public string StartTime { get; set; }
        
        public string EndTime { get; set; }
        
        public string Storage { get; set; }
        
        public string CodeBase { get; set; }
    }
}
