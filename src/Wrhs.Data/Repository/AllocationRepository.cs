using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wrhs.Core;
using Wrhs.Operations;
using Wrhs.Products;

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
            context.Allocations.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<Allocation> Get()
        {
            return context.Allocations.Include(x => x.Product);
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
            context.Allocations.Update(item);
            context.SaveChanges();
        }
    }
}