using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wrhs.Products;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class CreateProductCommandHandlerTests
    {
        [Test]
        public void OnHandleCreateAndSaveNewProduct()
        {
            var items = new List<Product>();
            var repoMock = MakeProductRepoMock(items);
            var handler = new CreateProductCommandHandler(repoMock);
            
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012",
                Description = "Some desc"
            };
            handler.Handle(command);

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("PROD1", items[0].Code);
        }

        [Test]
        [TestCase("PROD1")]
        [TestCase("prod1")]
        [TestCase("pRoD1")]
        [TestCase("PrOd1")]
        public void OnHandleAlwaysUppercaseProductCode(string code)
        {
            var items = new List<Product>();
            var repoMock = MakeProductRepoMock(items);
            var handler = new CreateProductCommandHandler(repoMock);
            var command = new CreateProductCommand
            {
                Code = code,
                Name = "Product",
                EAN = "123456789012",
                Description = "Some desc"
            };
            
            handler.Handle(command);

            Assert.AreEqual("PROD1", items[0].Code);
        }

        IRepository<Product> MakeProductRepoMock(List<Product> items)
        {
            var repoMock = new Mock<IRepository<Product>>();
            repoMock.Setup(m=>m.Save(It.IsAny<Product>()))
                .Callback((Product prod)=>{ items.Add(prod); });

            return repoMock.Object;
        }
    }
}