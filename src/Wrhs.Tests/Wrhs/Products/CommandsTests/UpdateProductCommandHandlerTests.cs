using System.Collections.Generic;
using System.Linq;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class UpdateProductCommandHandlerTests : ProductCommandTestsBase
    {
        [Fact]
        public void OnHandleUpdateExistingProduct()
        {
            var repo = MakeProductRepository(MakeSingleItemList());
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "NPROD1",
                Name = "New name product 1",
                EAN = "222333444555",
                Description = "New desc"
            };
            var handler = new UpdateProductCommandHandler(repo);

            handler.Handle(command);

            var prod = repo.Get().First();
            Assert.Equal("NPROD1", prod.Code);
            Assert.Equal("New name product 1", prod.Name);
            Assert.Equal("New desc", prod.Description);
            Assert.Equal("222333444555", prod.EAN);
        }

        [Theory]
        [InlineData("NPROD1")]
        [InlineData("nprod1")]
        [InlineData("nPrOd1")]
        [InlineData("NpRoD1")]
        public void OnHandleAlwaysUppercaseProductCode(string newCode)
        {
            var repo = MakeProductRepository(MakeSingleItemList());
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = newCode,
                Name = "New name product 1",
                EAN = "222333444555",
                Description = "New desc"
            };
            var handler = new UpdateProductCommandHandler(repo);

            handler.Handle(command);

            var prod = repo.Get().First();
            Assert.Equal("NPROD1", prod.Code);
        }

        protected List<Product> MakeSingleItemList()
        {
            return new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Code = "PROD1",
                    Name = "Product 1",
                    Description = "Some desc"
                }
            };
        }
    }
}