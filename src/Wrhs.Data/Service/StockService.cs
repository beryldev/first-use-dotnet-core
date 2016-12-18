using System.Collections.Generic;
using System.Linq;
using Wrhs.Common;

namespace Wrhs.Data.Service
{
    public class StockService : BaseData, IStockService
    {
        public StockService(WrhsContext context) : base(context)
        {
        }

        public IEnumerable<Stock> GetProductStock(int productId)
        {
            var items = context.Shifts
                .Where(s => s.ProductId == productId && s.Confirmed)
                .GroupBy(s => s.Location)
                .Select(s => new Stock
                {
                    ProductId = productId,
                    Location = s.First().Location,
                    Quantity = s.Sum(x=>x.Quantity)
                }).ToList();

            return items;
        }

        public Stock GetStockAtLocation(int productId, string location)
        {
            var sum = context.Shifts
                .Where(s => s.Confirmed && s.ProductId == productId && s.Location == location)
                .Sum(s => s.Quantity);

            return new Stock
            {
                ProductId = productId,
                Location = location,
                Quantity = sum,
                Product = context.Products.Find(productId)
            };
        }
    }
}