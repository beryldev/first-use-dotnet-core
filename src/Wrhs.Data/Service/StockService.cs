using System;
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
            throw new NotImplementedException();
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