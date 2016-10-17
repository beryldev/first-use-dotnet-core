
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Products.Commands;
using Wrhs.Tests;

namespace Wrhs.Products.Tests.Products
{
    [TestFixture]
    public class ProductServiceTests
    {
        [Test]
        public void CanCreateNewProduct()
        {
            var items = new List<Product>();
            var repo = MakeProductRepo(items);
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repo),
                new CreateProductCommandValidator(repo)
            );
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012"
            };

            service.Handle(command);

            Assert.AreEqual(1, repo.Get().Count());
            Assert.AreEqual("PROD1", repo.Get().First().Code);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void CantCreateProductWithEmptyCode(string code)
        {
            var items = new List<Product>();
            var repo = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = code,
                Name = "Product 1",
                EAN = "123456789012"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repo),
                new CreateProductCommandValidator(repo)
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
            var repo = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = name,
                EAN = "123456789012"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repo),
                new CreateProductCommandValidator(repo)
            );

            service.Handle(command);
        
            CollectionAssert.IsEmpty(items);
        }

        [Test]
        public void CantCreateProductWithExistingCode()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789011" } };
            var repo = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "123456789012"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repo),
                new CreateProductCommandValidator(repo)
            );

            service.Handle(command);
        
            Assert.AreEqual(1, repo.Get().Count());
        }

        [Test]
        public void CantCreateProductWithExistingEAN()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789011" } };
            var repo = MakeProductRepo(items);
            var command = new CreateProductCommand
            {
                Code = "PROD2",
                Name = "Product 2",
                EAN = "123456789011"
            };
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>(
                new CreateProductCommandHandler(repo),
                new CreateProductCommandValidator(repo)
            );

            service.Handle(command);
        
            Assert.AreEqual(1, repo.Get().Count());
        }

        protected IRepository<Product> MakeProductRepo(List<Product> items)
        {
            var repo = RepositoryFactory<Product>.Make();
            foreach(var item in items)
                repo.Save(item);

           return repo;
        }
    }
}