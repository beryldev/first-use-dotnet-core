using System;
using System.Collections.Generic;

namespace Wrhs.Operations
{
    public class AllocationService : IAllocationService
    {
        IRepository<Allocation> repo;

        public AllocationService(IRepository<Allocation> repo)
        {
            this.repo = repo;
        }

        public void RegisterAllocation(Allocation allocation)
        {
            Validate(allocation);

            if(allocation.Quantity < 0)
                throw new InvalidOperationException("Can't register allocation with negative quantity");

            repo.Save(allocation);
        }

        public void RegisterDeallocation(Allocation deallocation)
        {
            Validate(deallocation);

             if(deallocation.Quantity > 0)
                throw new InvalidOperationException("Can't register allocation with positive quantity");

            repo.Save(deallocation);
        }
        
        public IEnumerable<Allocation> GetAllocations()
        {
            return repo.Get();
        }

        protected void Validate(Allocation allocation)
        {
            if(String.IsNullOrWhiteSpace(allocation.Location))
                throw new ArgumentException("Empty location. Must provide location");

            if(String.IsNullOrWhiteSpace(allocation.Product.Code))
                throw new ArgumentException("Empty produc code. Must provide product code");
        }
    }
}