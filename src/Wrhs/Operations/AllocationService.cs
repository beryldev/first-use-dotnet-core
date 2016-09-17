using System;

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

           repo.Save(allocation);
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