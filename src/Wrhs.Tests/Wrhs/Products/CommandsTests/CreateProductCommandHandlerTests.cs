using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Products.Commands;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class CreateProductCommandHandlerTests : ProductCommandTestsBase
    {
        [Test]
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

            Assert.AreEqual(1, repo.Get().Count());
            Assert.AreEqual("PROD1", repo.Get().First().Code);
        }

        [Test]
        [TestCase("PROD1")]
        [TestCase("prod1")]
        [TestCase("pRoD1")]
        [TestCase("PrOd1")]
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

            Assert.AreEqual("PROD1", repo.Get().First().Code);
        }
    }
}