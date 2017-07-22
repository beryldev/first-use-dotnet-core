using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.WebApp.Controllers.Operations;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class BeginOperationControllerTests : OperationControllerTestsBase
    {
        private readonly BeginOperationController controller;

        public BeginOperationControllerTests() : base()
        {
            controller = new BeginOperationController(cmdBusMock.Object);
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnBeginDelivery()
        {
            var command = new BeginOperationCommand();

            var result = controller.BeginDelivery(command);

            AssertOk(result);
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnBeginRelocation()
        {
            var command = new BeginOperationCommand();

            var result = controller.BeginRelocation(command);

            AssertOk(result);
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnBeginRelease()
        {
            var command = new BeginOperationCommand();

            var result = controller.BeginRelease(command);

            AssertOk(result);
        }

        protected void AssertOk(IActionResult result)
        {
            result.Should().BeOfType<OkObjectResult>();
            var guid = (result as OkObjectResult).Value as string;
            guid.Should().NotBeNullOrWhiteSpace();
        }
    }
}