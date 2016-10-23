using System.Collections.Generic;

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
            return context.StocksCache;
        }

        public void Refresh(IWarehouse warehouse)
        {
            context.StocksCache.RemoveRange(context.StocksCache);
            context.StocksCache.AddRange(warehouse.CalculateStocks());
            context.SaveChanges();
        }
    }
}