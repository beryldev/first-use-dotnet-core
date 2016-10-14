
namespace Wrhs.Products.Operations
{
    public interface IOperation
    {
        OperationResult Perform(IAllocationService allocService);
    }
}