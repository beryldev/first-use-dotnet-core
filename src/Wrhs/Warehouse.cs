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
            return allocRepo.Get()
                .GroupBy(m=> new { m.Location, m.ProductCode })
                .Select(item => new Stock()
                {
                    ProductCode = item.First().ProductCode,
                    Location = item.First().Location,
                    Quantity = item.Sum(i=>i.Quantity)
                }).ToList();
        }

        public List<Stock> CalculateStocks(string productCode)
        {
            return new List<Stock>();
        }
    }
}