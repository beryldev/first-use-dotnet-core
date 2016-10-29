using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Wrhs.WebApp.Controllers;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void ShouldReturnAllProductsAsPaginateResultWhenNoRequestParams()
        {
            var repository = RepositoryFactory<Product>.Make();
            repository.Save(new Product{Code="PROD1", Name="Product 1", EAN = "1111"});
            var controller = new ProductController(repository);

            var result = controller.Get();

            Assert.Equal(1, result.Total);
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassNameRequestParam()
        {
            var repository = RepositoryFactory<Product>.Make();
            repository.Save(new Product{Code="PROD1", Name="Product 1", EAN = "1111"});
            var controller = new ProductController(repository);

            var result = controller.Get(name:"Product 1");

            Assert.Equal(1, result.Total);
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassCodeRequestParam()
        {
            var repository = RepositoryFactory<Product>.Make();
            repository.Save(new Product{Code="PROD1", Name="Product 1", EAN = "1111"});
            var controller = new ProductController(repository);

            var result = controller.Get(code:"PROD1");

            Assert.Equal(1, result.Total);
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassEANRequestParam()
        {
            var repository = RepositoryFactory<Product>.Make();
            repository.Save(new Product{Code="PROD1", Name="Product 1", EAN = "1111"});
            var controller = new ProductController(repository);

            var result = controller.Get(ean:"1111");

            Assert.Equal(1, result.Total);
        }

        [Fact]
        public void ShouldReturnProductWhenGetExistedProduct()
        {
            var product = new Product(){Code = "PROD1", Name = "Product 1"};
            var repository = new Mock<IRepository<Product>>();
            repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(product);
            var controller = new ProductController(repository.Object);

            var result = controller.GetById(1);

            Assert.Equal("PROD1", result.Code);
            Assert.Equal("Product 1", result.Name);
        }

        [Fact]
        public void ShouldReturnEmptyProductWhenGetNonExistedProduct()
        {
            var repository = new Mock<IRepository<Product>>();
            repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns((Product)null);
            var controller = new ProductController(repository.Object);

            var result = controller.GetById(1);

            Assert.Equal(0, result.Id);
            Assert.Null(result.Code);
            Assert.Null(result.Name);
        }

        [Fact]
        public void ShoudReturnOkRequestOnSuccessWhenCreateProduct()
        {
            var repository = RepositoryFactory<Product>.Make();
            var cmd = new CreateProductCommand();
            var handler = new Mock<ICommandHandler<CreateProductCommand>>();
            var validator = new Mock<IValidator<CreateProductCommand>>();
            validator.Setup(m=>m.Validate(cmd)).Returns(new List<ValidationResult>());
            var controller = new ProductController(repository);

            var result = controller.CreateProduct(cmd, validator.Object, handler.Object);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ShoudBadRequestWithErrorsRequestOnFailWhenCreateProduct()
        {
            var repository = RepositoryFactory<Product>.Make();
            var cmd = new CreateProductCommand();
            var handler = new Mock<ICommandHandler<CreateProductCommand>>();
            var validator = new Mock<IValidator<CreateProductCommand>>();
            validator.Setup(m=>m.Validate(cmd))
                .Returns(new List<ValidationResult>(){new ValidationResult{Field="test", Message="test"}});
            var controller = new ProductController(repository);

            var result = controller.CreateProduct(cmd, validator.Object, handler.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(1, ((IEnumerable<ValidationResult>)((BadRequestObjectResult)result).Value).Count());
        }

        [Fact]
        public void ShouldReturnOkOnDeleteWhenSuccess()
        {
            var repository = new Mock<IRepository<Product>>();
              repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(new Product());
            var cmd = new DeleteProductCommand();
            var handler = new Mock<ICommandHandler<DeleteProductCommand>>();
            var validator = new Mock<IValidator<DeleteProductCommand>>();
            validator.Setup(m=>m.Validate(cmd))
                .Returns(new List<ValidationResult>());
            var controller = new ProductController(repository.Object);

            var result = controller.Delete(1, cmd, validator.Object, handler.Object);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ShouldReturnBadRequestOnDeleteWhenFail()
        {
            var repository = new Mock<IRepository<Product>>();
              repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(new Product());
            var cmd = new DeleteProductCommand();
            var handler = new Mock<ICommandHandler<DeleteProductCommand>>();
            var validator = new Mock<IValidator<DeleteProductCommand>>();
            validator.Setup(m=>m.Validate(cmd))
                .Returns(new List<ValidationResult>(){new ValidationResult()});
            var controller = new ProductController(repository.Object);

            var result = controller.Delete(1, cmd, validator.Object, handler.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ShouldReturnOkOnUpdateWhenSuccess()
        {
            var repository = new Mock<IRepository<Product>>();
              repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(new Product());
            var cmd = new UpdateProductCommand();
            var handler = new Mock<ICommandHandler<UpdateProductCommand>>();
            var validator = new Mock<IValidator<UpdateProductCommand>>();
            validator.Setup(m=>m.Validate(cmd))
                .Returns(new List<ValidationResult>());
            var controller = new ProductController(repository.Object);

            var result = controller.Update(1, cmd, validator.Object, handler.Object);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ShouldReturnBadRequestOnUpdateWhenFail()
        {
            var repository = new Mock<IRepository<Product>>();
              repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(new Product());
            var cmd = new UpdateProductCommand();
            var handler = new Mock<ICommandHandler<UpdateProductCommand>>();
            var validator = new Mock<IValidator<UpdateProductCommand>>();
            validator.Setup(m=>m.Validate(cmd))
                .Returns(new List<ValidationResult>(){new ValidationResult()});
            var controller = new ProductController(repository.Object);

            var result = controller.Update(1, cmd, validator.Object, handler.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ShouldReturnStocksWhenGetStocks()
        {
            var repository = new Mock<IRepository<Product>>();
            repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(new Product());
            var warehouse = new Mock<IWarehouse>();
            warehouse.Setup(m=>m.ReadStocksByProductCode(It.IsAny<string>()))
                .Returns(new List<Stock>());
            var controller = new ProductController(repository.Object);

            var result = controller.GetStocks(1, warehouse.Object);

            Assert.IsType<List<Stock>>(result);
        }

        [Fact]
        public void ShouldReturnStocksWhenGetCalculatedStocks()
        {
            var repository = new Mock<IRepository<Product>>();
            repository.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns(new Product());
            var warehouse = new Mock<IWarehouse>();
            warehouse.Setup(m=>m.CalculateStocks(It.IsAny<string>()))
                .Returns(new List<Stock>());
            var controller = new ProductController(repository.Object);

            var result = controller.GetCalculatedStocks(1, warehouse.Object);

            Assert.IsType<List<Stock>>(result);
        }
    }
    
}