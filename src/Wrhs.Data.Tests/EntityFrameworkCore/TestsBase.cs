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
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        protected Product CreateProduct(WrhsContext context, string code="PROD1", string name="Product 1", string ean = "1111", string sku ="111")
        {
            var productRepo = new ProductRepository(context);
            var product = new Product
            {
                Code = code,
                Name = name,
                EAN = ean,
                SKU = sku
            };
            productRepo.Save(product);
            return product;
        }
    }
}