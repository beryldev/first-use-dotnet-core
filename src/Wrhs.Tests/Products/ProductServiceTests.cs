
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Products.Commands;

namespace Wrhs.Products.Tests.Products
{
    [TestFixture]
    public class ProductServiceTests
    {
        [Test]
        public void CanCreateNewProduct()
        {
            var items = new List<Product>();
            var repoMock = MakeProductRepo(items);
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repoMock.Object),
                new CreateProductCommandValidator(repoMock.Object)
            );
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012"
            };

            service.Handle(command);

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("PROD1", items[0].Code);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void CantCreateProductWithEmptyCode(string code)
        {
            var items = new List<Product>();
            var repoMock = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = code,
                Name = "Product 1",
                EAN = "123456789012"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repoMock.Object),
                new CreateProductCommandValidator(repoMock.Object)
            );

            service.Handle(command);
        
            CollectionAssert.IsEmpty(items);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void CantCreateProductWithEmptyName(string name)
        {
            var items = new List<Product>();
            var repoMock = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = name,
                EAN = "123456789012"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repoMock.Object),
                new CreateProductCommandValidator(repoMock.Object)
            );

            service.Handle(command);
        
            CollectionAssert.IsEmpty(items);
        }

        [Test]
        public void CantCreateProductWithExistingCode()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789011" } };
            var repoMock = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repoMock.Object),
                new CreateProductCommandValidator(repoMock.Object)
            );

            service.Handle(command);
        
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        public void CantCreateProductWithExistingEAN()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789011" } };
            var repoMock = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = "PROD2",
                Name = "Product 2",
                EAN = "123456789011"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repoMock.Object),
                new CreateProductCommandValidator(repoMock.Object)
            );

            service.Handle(command);
        
            Assert.AreEqual(1, items.Count);
        }

        protected Mock<IRepository<Product>> MakeProductRepo(List<Product> items)
        {
            var mock = new Mock<IRepository<Product>>();
            mock.Setup(m=>m.Save(It.IsAny<Product>()))
                .Callback((Product prod)=>{ items.Add(prod); });

            mock.Setup(m=>m.Get())
                .Returns(items);

            return mock;
        }
    }
}