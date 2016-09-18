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

        public Warehouse(IRepository<Allocation> allocRepo)
        {
            this.allocRepo = allocRepo;
        }
        
        public void ProcessOperation(DeliveryOperation operation)
        {
            operation.Perform();
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