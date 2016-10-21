using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Operations;

namespace Wrhs.Data.Repository
{
    public class AllocationRepository : IRepository<Allocation>
    {
        WrhsContext context;

        public AllocationRepository(WrhsContext context)
        {
            this.context = context;
        }

        public void Delete(Allocation item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Allocation> Get()
        {
            return context.Allocations;
        }

        public Allocation GetById(int id)
        {
            return context.Allocations
                .Where(item=>item.Id == id)
                .FirstOrDefault();
        }

        public Allocation Save(Allocation item)
        {
            context.Allocations.Add(item);
            context.SaveChanges();

            return item;
        }

        public void Update(Allocation item)
        {
            throw new NotImplementedException();
        }
    }
}