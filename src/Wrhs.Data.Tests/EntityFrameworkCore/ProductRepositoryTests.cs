using System.Linq;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.EntityFrameworkCore
{
    public class ProductRepositoryTests : TestsBase
    {
        WrhsContext context;

        public ProductRepositoryTests()
        {
            context = CreateContext();
        }   

        [Fact]
        public void CanSaveProduct()
        {
            var repo = new ProductRepository(context);
            var product = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "1111",
                SKU = "111",
                Description = "some desc",
            };

            repo.Save(product);

            Assert.Equal(1, repo.Get().Count());
        }

        [Fact]
        public void CanRetriveProductById()
        {
            var repo = new ProductRepository(context);
            var product = new Product
                {
                    Code = "PROD1",
                    Name = "Product 1",
                    EAN = "1111",
                    SKU = "111",
                    Description = "some desc",
                };
            
            repo.Save(product);
            var prod = repo.GetById(product.Id);
            
            Assert.Equal("PROD1", prod.Code);
        }

        [Fact]
        public void CanDeleteProduct()
        {
            //Given
            var repo = new ProductRepository(context);
            var product = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "1111",
                SKU = "111",
                Description = "some desc",
            };
        
            repo.Save(product);
            repo.Delete(product);
        
            Assert.Empty(repo.Get());
        }

        [Fact]
        public void CanUpdateProduct()
        {
            var repo = new ProductRepository(context);
            var product = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "1111",
                SKU = "111",
                Description = "some desc",
            };

            repo.Save(product);
            var id = product.Id;
            product.Name = "Other name";
            repo.Update(product);
            
            var prod = repo.GetById(id);
            Assert.Equal("Other name", prod.Name);     
        }

    }
}