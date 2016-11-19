using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Wrhs.Data
{
    public class DbStockCache : IStockCache
    {
        WrhsContext context;

        public DbStockCache(WrhsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Stock> Read()
        {
            return context.StocksCache.Include(s => s.Product);
        }

        public void Refresh(IWarehouse warehouse)
        {
            context.StocksCache.RemoveRange(context.StocksCache);
            var stocks = warehouse.CalculateStocks();
            stocks.ForEach(s => s.Product = context.Products.Where(p => p.Id == s.Product.Id).FirstOrDefault());
            context.StocksCache.AddRange(stocks);
            context.SaveChanges();
        }
    }
}