
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Products.Commands;
using Wrhs.Tests;
using Xunit;

namespace Wrhs.Products.Tests.Products
{
    public class ProductServiceTests
    {
        [Fact]
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

            Assert.Equal(1, repo.Get().Count());
            Assert.Equal("PROD1", repo.Get().First().Code);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
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
        
            Assert.Empty(items);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
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
        
            Assert.Empty(items);
        }

        [Fact]
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
        
            Assert.Equal(1, repo.Get().Count());
        }

        [Fact]
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
        
            Assert.Equal(1, repo.Get().Count());
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