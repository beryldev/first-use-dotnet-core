using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;
using Wrhs.WebApp.Controllers.Operations;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class ExecuteOperationControllerTests : OperationControllerTestsBase
    {
        private readonly ExecuteOperationController controller;

        public ExecuteOperationControllerTests() : base()
        {
            controller = new ExecuteOperationController(cmdBusMock.Object);
        }

        [Fact]
        public void ShouldReturnOkOnExecuteDeliveryWhenSuccess()
        {
            var command = new ExecuteDeliveryOperationCommand();

            var result = controller.ExecuteDelivery("some-guid", command);

            result.Should().BeOfType<OkResult>();   
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnExecuteDeliveryWhenValidationFail()
        {
            var command = new ExecuteDeliveryOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.ExecuteDelivery("some-guid", command);

            AssertBadRequest(result);
        }

        [Fact]
        public void ShouldReturnOkOnExecuteRelocationWhenSuccess()
        {
            var command = new ExecuteRelocationOperationCommand();

            var result = controller.ExecuteRelocation("some-guid", command);

            result.Should().BeOfType<OkResult>();   
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnExecuteRelocationWhenValidationFail()
        {
            var command = new ExecuteRelocationOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.ExecuteRelocation("some-guid", command);

            AssertBadRequest(result);
        }
        
        [Fact]
        public void ShouldReturnOkOnExecuteReleaseWhenSuccess()
        {
            var command = new ExecuteReleaseOperationCommand();

            var result = controller.ExecuteRelease("some-guid", command);

            result.Should().BeOfType<OkResult>();   
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnExecuteReleaseWhenValidationFail()
        {
            var command = new ExecuteReleaseOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.ExecuteRelease("some-guid", command);

            AssertBadRequest(result);
        }
    }
}