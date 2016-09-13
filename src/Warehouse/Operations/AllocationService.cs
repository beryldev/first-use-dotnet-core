using System;

namespace Warehouse.Operations
{
    public class AllocationService : IAllocationService
    {
        public void RegisterAllocation(Allocation allocation)
        {
           Validate(allocation);
        }

        protected void Validate(Allocation allocation)
        {
             if(String.IsNullOrWhiteSpace(allocation.Location))
                throw new ArgumentException("Empty location. Must provide location");

            if(String.IsNullOrWhiteSpace(allocation.ProductCode))
                throw new ArgumentException("Empty produc code. Must provide product code");
        }
    }
}