using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Services;
using Wrhs.WebApp.Controllers;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class ProductReadControllerTests
    {
        private readonly ProductReadController controller;
        private readonly Mock<IProductService> productSrvMock;

        public ProductReadControllerTests()
        {
            var product = new Product(){Code = "PROD1", Name = "Product 1"};
            
            productSrvMock = new Mock<IProductService>();
            
            productSrvMock.Setup(m=>m.FilterProducts(It.IsAny<ProductFilter>(), 
                It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ResultPage<Product>(new List<Product>{new Product()}, 1, 20));
            
            productSrvMock.Setup(m=>m.GetProductById(It.IsNotNull<int>()))
                .Returns(product);

            controller = new ProductReadController(productSrvMock.Object);
        }

        [Fact]
        public void ShouldReturnOkWithProductsAsPaginateResultWhenNoRequestParams()
        {      
            var filter = new ProductFilter();

            var result = controller.Get(filter);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkWithPageWhenRequestedPage()
        {
            var filter = new ProductFilter();

            var result = controller.Get(filter: filter, page: 3);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkWithPageWhenRequestedPageSize()
        {
            var filter = new ProductFilter();

            var result = controller.Get(filter, pageSize: 25);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassNameRequestParam()
        {
            var filter = new ProductFilter { Name = "Product 1" };

            var result = controller.Get(filter);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassCodeRequestParam()
        {
            var filter = new ProductFilter { Code = "PROD1" };

            var result = controller.Get(filter);

            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Product>;
            page.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnFilteredResultWhenPassEANRequestParam()
        {
            var filter = new ProductFilter { Ean = "1111" };

            var result = controller.Get(filter);

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

        [Fact]
        public void ShouldReturnOkWithStocksWhenGetStocks()
        {
            var stockSrvMock = new Mock<IStockService>();
            stockSrvMock.Setup(m=>m.GetProductStock(It.IsNotNull<int>()))
                .Returns(new List<Stock>{new Stock()});

            var result = controller.GetStocks(1, stockSrvMock.Object);

            result.Should().BeOfType<OkObjectResult>();
            var stocks = (result as OkObjectResult).Value as IEnumerable<Stock>;
            stocks.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnNotFoundWhenGetNonExistedProduct()
        {
            productSrvMock.Setup(m=>m.GetProductById(1)).Returns(null as Product);
            var result = controller.GetProductById(1);

            result.Should().BeOfType<NotFoundResult>();
        }

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