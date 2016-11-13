using System.Collections.Generic;
using System.Linq;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class CreateProductCommandHandlerTests : ProductCommandTestsBase
    {
        [Fact]
        public void OnHandleCreateAndSaveNewProduct()
        {
            var repo = MakeProductRepository(new List<Product>());
            var handler = new CreateProductCommandHandler(repo);
            
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012",
                Description = "Some desc"
            };
            handler.Handle(command);

            Assert.Equal(1, repo.Get().Count());
            Assert.Equal("PROD1", repo.Get().First().Code);
        }

        [Theory]
        [InlineData("PROD1")]
        [InlineData("prod1")]
        [InlineData("pRoD1")]
        [InlineData("PrOd1")]
        public void OnHandleAlwaysUppercaseProductCode(string code)
        {
            var repo = MakeProductRepository(new List<Product>());
            var handler = new CreateProductCommandHandler(repo);
            var command = new CreateProductCommand
            {
                Code = code,
                Name = "Product",
                EAN = "123456789012",
                Description = "Some desc"
            };
            
            handler.Handle(command);

            Assert.Equal("PROD1", repo.Get().First().Code);
        }
    }
}