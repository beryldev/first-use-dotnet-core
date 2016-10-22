using Wrhs.Data.ContextFactory;
using Wrhs.Data.Repository;
using Wrhs.Products;

namespace Wrhs.Data.Tests
{
    public class TestsBase
    {
        protected WrhsContext CreateContext()
        {
            var context = InMemoryContextFactory.Create();
            //var context = SqliteContextFactory.Create("Filename=./wrhs.db");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        protected Product CreateProduct(WrhsContext context)
        {
            var productRepo = new ProductRepository(context);
            var product = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "1111",
                SKU = "111"
            };
            productRepo.Save(product);
            return product;
        }
    }
}