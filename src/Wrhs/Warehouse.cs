using System;
using System.Linq;
using System.Collections.Generic;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;

namespace Wrhs
{
    public class Warehouse
    {
        IRepository<Allocation> allocRepo;

        IStockCache cache;

        public Warehouse(IRepository<Allocation> allocRepo, IStockCache cache)
        {
            this.allocRepo = allocRepo;

            this.cache = cache;
        }
        
        public void ProcessOperation(DeliveryOperation operation)
        {
            operation.Perform();

            cache.Refresh(this);
        }

        public List<Stock> CalculateStocks()
        {
            var items =  allocRepo.Get();

            return Calculate(items);
        }

        public List<Stock> CalculateStocks(string productCode)
        {
            var items = allocRepo.Get()
                .Where(m=>m.ProductCode.Equals(productCode));

            return Calculate(items);  
        }

        public List<Stock> ReadStocks()
        {
            return cache.Read().ToList();
        }

        protected List<Stock> Calculate(IEnumerable<Allocation> items)
        {
            return items
                .GroupBy(m=> new { m.Location, m.ProductCode })
                .Select(item => new Stock()
                {
                    ProductCode = item.First().ProductCode,
                    Location = item.First().Location,
                    Quantity = item.Sum(i=>i.Quantity)
                }).ToList();
        }
    }
}