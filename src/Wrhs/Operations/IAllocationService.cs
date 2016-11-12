using System.Collections.Generic;

namespace Wrhs.Operations
{
    public interface IAllocationService
    {
        void RegisterAllocation(Allocation allocation);

        void RegisterDeallocation(Allocation deallocation);

        IEnumerable<Allocation> GetAllocations();

        IEnumerable<Allocation> GetAllocations(int productId);

        IEnumerable<Allocation> GetAllocations(string code);

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}