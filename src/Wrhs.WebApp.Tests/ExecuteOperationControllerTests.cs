using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
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
        public void ShouldReturnOkOnExecuteWhenSuccess()
        {
            var command = new ExecuteOperationCommand();

            var result = controller.Execute("some-guid");

            result.Should().BeOfType<OkResult>();   
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnExecuteWhenValidationFail()
        {
            var command = new ExecuteOperationCommand();
            SetupCmdBusValidationFails(command);

            var result = controller.Execute("some-guid");

            AssertBadRequest(result);
        }
    }
}