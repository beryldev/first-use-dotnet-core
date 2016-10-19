using System.Linq;
using System.Collections.Generic;
using Wrhs.Operations;
using System;

namespace Wrhs
{
    public class Warehouse : IWarehouse
    {
        IAllocationService allocService;

        IStockCache cache;

        public Warehouse(IAllocationService allocService, IStockCache cache)
        {
            this.allocService = allocService;

            this.cache = cache;
        }

        public void ProcessOperation(IOperation operation)
        {
            try
            {
                allocService.BeginTransaction();
                operation.Perform(allocService);
                allocService.CommitTransaction();    
            }
            catch(Exception)
            {
                allocService.RollbackTransaction();
                throw;
            }
            finally
            {
                cache.Refresh(this);
            }
            
        }

        public List<Stock> CalculateStocks()
        {
            var items = allocService.GetAllocations();

            return Calculate(items);
        }

        public List<Stock> CalculateStocks(string productCode)
        {
            var items = allocService.GetAllocations()
                .Where(m => m.Product.Code.Equals(productCode));

            return Calculate(items);
        }

        public List<Stock> ReadStocks()
        {
            return cache.Read().ToList();
        }

        public List<Stock> ReadStocksByProductCode(string productCode)
        {
            return cache.Read()
                .Where(item => item.Product.Code.Equals(productCode))
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
                .GroupBy(m => new { m.Location, m.Product })
                .Select(item => new Stock()
                {
                    Product = item.First().Product,
                    Location = item.First().Location,
                    Quantity = item.Sum(i => i.Quantity)
                }).ToList();
        }
    }
}