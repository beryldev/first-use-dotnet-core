using System;

namespace Warehouse.Operations
{
    public interface IAllocationService
    {
        void RegisterAllocation(Allocation allocation);
    }
}