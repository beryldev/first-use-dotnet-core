using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace Wrhs.Data.Tests
{
    public class DbStockCacheTests : TestsBase
    {
        WrhsContext context;

        public DbStockCacheTests()
        {
            context = CreateContext();
        }

        [Fact]
        public void ShouldReturnStocksWhenRead()
        {
            for(var i=0; i<4; i++)
            {
                var prod = CreateProduct(context, $"PROD{i+1}", $"Product {i+1}", $"1111{i+1}", $"11{i+1}");
                context.StocksCache.Add(new Stock
                {
                    Product = prod,
                    Quantity = 10+i,
                    Location = $"LOC-001-0{i+1}"
                });
            }
            context.SaveChanges();

            var cache = new DbStockCache(context);

            var result = cache.Read();

            Assert.Equal(4, result.Count());
            Assert.Equal(46, result.Sum(s=>s.Quantity));
        }

        [Fact]
        public void ShouldRefreshCachedStocks()
        {
            for(var i=0; i<4; i++)
            {
                var prod = CreateProduct(context, $"PROD{i+1}", $"Product {i+1}", $"1111{i+1}", $"11{i+1}");
                context.StocksCache.Add(new Stock
                {
                    Product = prod,
                    Quantity = 10+i,
                    Location = $"LOC-001-0{i+1}"
                });
            }
            context.SaveChanges();
            var warehouse = new Mock<IWarehouse>();
            warehouse.Setup(m=>m.CalculateStocks())
                .Returns(new List<Stock>{
                    new Stock { Product = context.Products.First(), Location = "LOC-002-01", Quantity = 100}
                });
            
            var cache = new DbStockCache(context);
            cache.Refresh(warehouse.Object);

            Assert.Equal(1, context.StocksCache.Count());
            Assert.Equal(100, context.StocksCache.Sum(s=>s.Quantity));
        }   
    }
}