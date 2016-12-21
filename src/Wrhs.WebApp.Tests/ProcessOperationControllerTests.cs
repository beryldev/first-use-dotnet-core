using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;
using Wrhs.WebApp.Controllers.Operations;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class ProcessOperationControllerTests : OperationControllerTestsBase
    {
        private readonly ProcessOperationController controller;
        
        public ProcessOperationControllerTests() : base()
        {
            controller = new ProcessOperationController(cmdBusMock.Object);
        }

        [Fact]
        public void ShouldReturnOkOnProcessDeliveryWhenSuccess()
        {
            var command = new ProcessDeliveryOperationCommand();

            var result = controller.ProcessDelivery("some-guid", command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnProcessDeliveryWhenValidationFails()
        {
            var command = new ProcessDeliveryOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.ProcessDelivery("some-guid", command);

            AssertBadRequest(result);
        }

        [Fact]
        public void ShouldReturnOkOnProcessRelocationWhenSuccess()
        {
            var command = new ProcessRelocationOperationCommand();

            var result = controller.ProcessRelocation("some-guid", command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnProcessRelocationWhenValidationFails()
        {
            var command = new ProcessRelocationOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.ProcessRelocation("some-guid", command);

            AssertBadRequest(result);
        }

        [Fact]
        public void ShouldReturnOkOnProcessReleaseWhenSuccess()
        {
            var command = new ProcessReleaseOperationCommand();

            var result = controller.ProcessRelease("some-guid", command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnProcessReleaseWhenValidationFails()
        {
            var command = new ProcessReleaseOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.ProcessRelease("some-guid", command);

            AssertBadRequest(result);
        }     
    }
}