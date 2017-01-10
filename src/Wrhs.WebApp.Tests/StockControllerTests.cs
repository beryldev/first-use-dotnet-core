using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.WebApp.Controllers;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class StockControllerTests
    {
        [Fact]
        public void ShouldReturnOkWithPageOfStocksOnGetStocks()
        {
            var stockServiceMock = new Mock<IStockService>();
            stockServiceMock.Setup(m=>m.GetStocks(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int page, int pageSize) => 
                { 
                    return new ResultPage<Stock>(new List<Stock>(), page, pageSize);
                });

            var controller = new StockController(stockServiceMock.Object);

            var result = controller.GetStocks(1, 5);
            var resultPage = (result as OkObjectResult).Value as ResultPage<Stock>;
            resultPage.Items.Should().NotBeNull();
        }
    }
}