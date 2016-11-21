

namespace Wrhs.Operations
{
    public interface IOperation
    {
        OperationResult Perform(IAllocationService allocService);

    }
}