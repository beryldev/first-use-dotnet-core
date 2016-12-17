using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Services;
using Wrhs.WebApp.Controllers;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class ProductControllerTests
    {
        private readonly ProductReadController controller;
        private readonly Mock<IProductService> productSrvMock;

        public ProductControllerTests()
        {
            var product = new Product(){Code = "PROD1", Name = "Product 1"};
            productSrvMock = new Mock<IProductService>();
            productSrvMock.Setup(m=>m.FilterProducts(It.IsAny<Dictionary<string, object>>(), 
                It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ResultPage<Product>(new List<Product>{new Product()}, 1, 20));
            productSrvMock.Setup(m=>m.GetProductById(It.IsNotNull<int>()))
                .Returns(product);
            controller = new ProductReadController(productSrvMock.Object);
        }

        [Fact]
        public void ShouldReturnOkWithProductsAsPaginateResultWhenNoRequestParams()
        {            
            var result = controller.Get();

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkWithPageWhenRequestedPage()
        {

            var result = controller.Get(page: 3);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkWithPageWhenRequestedPageSize()
        {
            var result = controller.Get(pageSize: 25);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassNameRequestParam()
        {
            var result = controller.Get(name:"Product 1");

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassCodeRequestParam()
        {
            var result = controller.Get(code:"PROD1");

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassEANRequestParam()
        {
            var result = controller.Get(ean:"1111");

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkWithProductWhenGetExistedProduct()
        {
            var result = controller.GetProductById(1);

            Assert.IsType<OkObjectResult>(result);
            var resultProd = ((OkObjectResult)result).Value as Product;
            Assert.Equal("PROD1", resultProd.Code);
            Assert.Equal("Product 1", resultProd.Name);
        }

        // [Fact]
        // public void ShouldReturnStocksWhenGetStocks()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //     repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var warehouse = new Mock<IWarehouse>();
        //     warehouse.Setup(m=>m.ReadStocksByProductCode(It.IsAny<string>()))
        //         .Returns(new List<Stock>());
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.GetStocks(1, warehouse.Object);

        //     Assert.IsType<List<Stock>>(result);
        // }

        // [Fact]
        // public void ShouldReturnEmptyProductWhenGetNonExistedProduct()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //     repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns((Product)null);
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.GetById(1);

        //     Assert.IsType<NotFoundResult>(result);
        // }

        // [Fact]
        // public void ShouldReturnOkRequestOnSuccessWhenCreateProduct()
        // {
        //     var repository = RepositoryFactory<Product>.Make();
        //     var cmd = new CreateProductCommand();
        //     var handler = new Mock<ICommandHandler<CreateProductCommand>>();
        //     var validator = new Mock<IValidator<CreateProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd)).Returns(new List<ValidationResult>());
        //     var controller = new ProductController(repository);

        //     var result = controller.CreateProduct(cmd, validator.Object, handler.Object);

        //     Assert.IsType<OkResult>(result);
        // }

        // [Fact]
        // public void ShouldBadRequestWithErrorsRequestOnFailWhenCreateProduct()
        // {
        //     var repository = RepositoryFactory<Product>.Make();
        //     var cmd = new CreateProductCommand();
        //     var handler = new Mock<ICommandHandler<CreateProductCommand>>();
        //     var validator = new Mock<IValidator<CreateProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>(){new ValidationResult{Field="test", Message="test"}});
        //     var controller = new ProductController(repository);

        //     var result = controller.CreateProduct(cmd, validator.Object, handler.Object);

        //     Assert.IsType<BadRequestObjectResult>(result);
        //     Assert.Equal(1, ((IEnumerable<ValidationResult>)((BadRequestObjectResult)result).Value).Count());
        // }

        // [Fact]
        // public void ShouldReturnOkOnDeleteWhenSuccess()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new DeleteProductCommand();
        //     var handler = new Mock<ICommandHandler<DeleteProductCommand>>();
        //     var validator = new Mock<IValidator<DeleteProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>());
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Delete(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<OkResult>(result);
        // }

        // [Fact]
        // public void ShouldReturnBadRequestOnDeleteWhenFail()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new DeleteProductCommand();
        //     var handler = new Mock<ICommandHandler<DeleteProductCommand>>();
        //     var validator = new Mock<IValidator<DeleteProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>(){new ValidationResult()});
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Delete(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<BadRequestObjectResult>(result);
        // }

        // [Fact]
        // public void ShouldReturnOkOnUpdateWhenSuccess()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new UpdateProductCommand();
        //     var handler = new Mock<ICommandHandler<UpdateProductCommand>>();
        //     var validator = new Mock<IValidator<UpdateProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>());
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Update(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<OkResult>(result);
        // }

        // [Fact]
        // public void ShouldReturnBadRequestOnUpdateWhenFail()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new UpdateProductCommand();
        //     var handler = new Mock<ICommandHandler<UpdateProductCommand>>();
        //     var validator = new Mock<IValidator<UpdateProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>(){new ValidationResult()});
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Update(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<BadRequestObjectResult>(result);
        // }

        

        // [Fact]
        // public void ShouldReturnStocksWhenGetCalculatedStocks()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //     repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var warehouse = new Mock<IWarehouse>();
        //     warehouse.Setup(m=>m.CalculateStocks(It.IsAny<string>()))
        //         .Returns(new List<Stock>());
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.GetCalculatedStocks(1, warehouse.Object);

        //     Assert.IsType<List<Stock>>(result);
        // }
    }
    
}