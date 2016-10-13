using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wrhs.Products;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class UpdateProductCommandHandlerTests : ProductCommandTestsBase
    {
        [Test]
        public void OnHandleUpdateExistingProduct()
        {
            var items = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Code = "PROD1",
                    Name = "Product 1",
                    Description = "Some desc"
                }
            };
            var repo = MakeProductRepository(items);
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

            var prod = items.First();
            Assert.AreEqual("NPROD1", prod.Code);
            Assert.AreEqual("New name product 1", prod.Name);
            Assert.AreEqual("New desc", prod.Description);
            Assert.AreEqual("222333444555", prod.EAN);
        }

        [Test]
        [TestCase("NPROD1")]
        [TestCase("nprod1")]
        [TestCase("nPrOd1")]
        [TestCase("NpRoD1")]
        public void OnHandleAlwaysUppercaseProductCode(string newCode)
        {
            var items = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Code = "PROD1",
                    Name = "Product 1",
                    Description = "Some desc"
                }
            };
            var repo = MakeProductRepository(items);
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

            var prod = items.First();
            Assert.AreEqual("NPROD1", prod.Code);
        }
    }
}