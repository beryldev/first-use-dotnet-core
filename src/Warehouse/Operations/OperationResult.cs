namespace Warehouse.Operations
{
    public class OperationResult : IOperationResult
    {
        public IOperationDocument OperationDocument { get; set; }

        public ResultStatus Status { get; set; }
    }
}