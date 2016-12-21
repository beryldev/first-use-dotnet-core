using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.WebApp.Controllers.Operations;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class OperationReadControllerTests
    {
        private readonly Mock<IOperationService> operationSrvMock;
        private readonly OperationReadController controller;

        public OperationReadControllerTests()
        {
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.Get())
                .Returns(new ResultPage<Operation>(new List<Operation>{new Operation()}, 1, 1));
            operationSrvMock.Setup(m=>m.Get(It.IsAny<int>()))
                .Returns(new ResultPage<Operation>(new List<Operation>{new Operation()}, 1, 1));
            operationSrvMock.Setup(m=>m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new ResultPage<Operation>(new List<Operation>{new Operation()}, 1, 1));
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsNotNull<string>()))
                .Returns(new Operation());

            controller = new OperationReadController(operationSrvMock.Object);
        }

        [Fact]
        public void ShouldReturnOkWithPageOfOperationsOnGetOperations()
        {
            var result = controller.GetOperations();

            AssertOkWithResultPage(result);
        }

        [Fact]
        public void ShouldReturnOkWithPageOnGetOperationWhenSpecPage()
        {
            var result = controller.GetOperations(page: 2);

            AssertOkWithResultPage(result);
        }

        [Fact]
        public void ShouldReturnOkWithPageOnGetOperationWhenSpecPageSize()
        {
            var result = controller.GetOperations(pageSize: 30);

            AssertOkWithResultPage(result);
        }

        [Fact]
        public void ShouldReturnOkWithOperationOnGetOperation()
        {
            var result = controller.GetOperation("some-guid");

            result.Should().BeOfType<OkObjectResult>();
            var operation = (result as OkObjectResult).Value;
            operation.Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetOperationWhenNotExists()
        {
            operationSrvMock.Setup(m=>m.GetOperationByGuid(It.IsNotNull<string>()))
                .Returns(null as Operation);

            var result = controller.GetOperation("some-guid");

            result.Should().BeOfType<NotFoundResult>();
        }

        protected void AssertOkWithResultPage(IActionResult result)
        { 
            result.Should().BeOfType<OkObjectResult>();
            var page = (result as OkObjectResult).Value as ResultPage<Operation>;
            page.Items.Should().NotBeNullOrEmpty();
        }
    }
}