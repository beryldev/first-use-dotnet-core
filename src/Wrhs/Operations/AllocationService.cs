using System;
using System.Linq;
using System.Collections.Generic;

namespace Wrhs.Products.Operations
{
    public class AllocationService : IAllocationService
    {
        IRepository<Allocation> repo;

        bool isInTransaction = false;

        List<Allocation> transactionItems;

        public AllocationService(IRepository<Allocation> repo)
        {
            this.repo = repo;
        }

        public void RegisterAllocation(Allocation allocation)
        {
            Validate(allocation);

            if(allocation.Quantity < 0)
                throw new InvalidOperationException("Can't register allocation with negative quantity");

            if(isInTransaction)
                transactionItems.Add(allocation);
            else
                repo.Save(allocation);
        }

        public void RegisterDeallocation(Allocation deallocation)
        {
            Validate(deallocation);

             if(deallocation.Quantity > 0)
                throw new InvalidOperationException("Can't register allocation with positive quantity");

            VerifyExistResourceAtLocation(deallocation);

            if(isInTransaction)
                transactionItems.Add(deallocation);
            else
                repo.Save(deallocation);
        }
        
        public IEnumerable<Allocation> GetAllocations()
        {
            return repo.Get();
        }

        public void BeginTransaction()
        {
            if(isInTransaction)
                throw new InvalidOperationException("Transaction is active");

            transactionItems = new List<Allocation>();
            isInTransaction = true;
        }

        public void CommitTransaction()
        {
            foreach(var item in transactionItems)
                repo.Save(item);

            transactionItems = new List<Allocation>();
            isInTransaction = false;
        }

        public void RollbackTransaction()
        {
            transactionItems = new List<Allocation>();
            isInTransaction = false;
        }

        protected void Validate(Allocation allocation)
        {
            if(String.IsNullOrWhiteSpace(allocation.Location))
                throw new ArgumentException("Empty location. Must provide location");

            if(String.IsNullOrWhiteSpace(allocation.Product.Code))
                throw new ArgumentException("Empty product code. Must provide product code");
        }

        protected void VerifyExistResourceAtLocation(Allocation allocation)
        {
            var items = repo.Get().Where(item=>item.Product.Code.Equals(allocation.Product.Code) 
                && item.Location.Equals(allocation.Location));

            if(items.Count() == 0)
                throw new InvalidOperationException("Resource not found at location");

            if(items.Sum(item=>item.Quantity) < Math.Abs(allocation.Quantity))
                throw new InvalidOperationException("Not enough resource at location");
        }
    }
}