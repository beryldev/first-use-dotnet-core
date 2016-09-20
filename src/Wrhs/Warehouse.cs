using System;
using System.Linq;
using System.Collections.Generic;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;

namespace Wrhs
{
    public class Warehouse
    {
        IAllocationService allocService;

        IStockCache cache;

        public Warehouse(IAllocationService allocService, IStockCache cache)
        {
            this.allocService = allocService;

            this.cache = cache;
        }

        public void ProcessOperation(DeliveryOperation operation)
        {
            
            operation.Perform(allocService);

            cache.Refresh(this);
        }

        public List<Stock> CalculateStocks()
        {
            var items = allocService.GetAllocations();

            return Calculate(items);
        }

        public List<Stock> CalculateStocks(string productCode)
        {
            var items = allocService.GetAllocations()
                .Where(m => m.ProductCode.Equals(productCode));

            return Calculate(items);
        }

        public List<Stock> ReadStocks()
        {
            return cache.Read().ToList();
        }

        public List<Stock> ReadStocksByProductCode(string productCode)
        {
            return cache.Read()
                .Where(item => item.ProductCode.Equals(productCode))
                .ToList();
        }

        public List<Stock> ReadStocksByLocation(string location)
        {
            return cache.Read()
                .Where(item => item.Location.Equals(location))
                .ToList();
        }

        protected List<Stock> Calculate(IEnumerable<Allocation> items)
        {
            return items
                .GroupBy(m => new { m.Location, m.ProductCode })
                .Select(item => new Stock()
                {
                    ProductCode = item.First().ProductCode,
                    Location = item.First().Location,
                    Quantity = item.Sum(i => i.Quantity)
                }).ToList();
        }
    }
}