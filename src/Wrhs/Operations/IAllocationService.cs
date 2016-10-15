using System.Collections.Generic;

namespace Wrhs.Operations
{
    public interface IAllocationService
    {
        void RegisterAllocation(Allocation allocation);

        void RegisterDeallocation(Allocation deallocation);

        IEnumerable<Allocation> GetAllocations();

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}