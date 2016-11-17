using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.WebApp.src.Controllers.Operations;
using Wrhs.WebApp.Utils;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryOperationControllerTests
    {
        readonly DeliveryOperationController controller;

        readonly Mock<ICache> cacheMock;

        public DeliveryOperationControllerTests()
        {
            cacheMock = new Mock<ICache>();
            controller = new DeliveryOperationController(cacheMock.Object); 
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnNewOperation()
        {

            var result = controller.NewOperation();

            Assert.IsType<OkObjectResult>(result);
            Assert.NotEmpty((result as OkObjectResult).Value as string);
        }

        [Fact]
        public void ShouldPutClearOperationStateIntoCacheOnNewOperation()
        {
            var result = controller.NewOperation();

            var guid = (result as OkObjectResult).Value as string;
            cacheMock.Verify(m => m.SetValue(guid, It.IsAny<object>()));
        }
    }
}