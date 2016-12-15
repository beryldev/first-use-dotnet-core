// using System.Collections.Generic;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Wrhs.WebApp.Controllers;
// using Xunit;

// namespace Wrhs.WebApp.Tests
// {
//     public class StockControllerTests
//     {
//         [Fact]
//         public void ShouldReturnOkStocksOnGetStocks()
//         {
//             var warehouseMock = new Mock<IWarehouse>();
//             warehouseMock.Setup(m => m.ReadStocks())
//                 .Returns(new List<Stock>{new Stock()});
//             var controller = new StockController(warehouseMock.Object);

//             var result = controller.GetStocks();
//             var stocks = (result as OkObjectResult).Value as IEnumerable<Stock>;
//             stocks.Should().NotBeEmpty();
//         }
//     }
// }