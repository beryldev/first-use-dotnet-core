
namespace Warehouse.Operations
{
    public interface IOperationResult
    {
        IOperationDocument OperationDocument { get; set; }

        ResultStatus Status { get; set; }
    }


    public enum ResultStatus
    {
        OK = 1,
        Error = 2
    }   
}
