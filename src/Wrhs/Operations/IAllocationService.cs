using System.Collections.Generic;

namespace Wrhs.Operations
{
    public interface IAllocationService
    {
        void RegisterAllocation(Allocation allocation);
        
        IEnumerable<Allocation> GetAllocations();
    }
}