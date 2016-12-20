using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;
using Wrhs.WebApp.Controllers.Operations;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class BeginOperationControllerTests
    {
        private readonly BeginOperationController controller;
        private readonly Mock<ICommandBus> cmdBusMock;

        public BeginOperationControllerTests()
        {
            cmdBusMock = new Mock<ICommandBus>();
            controller = new BeginOperationController(cmdBusMock.Object);
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnBeginDelivery()
        {
            var command = new BeginDeliveryOperationCommand();

            var result = controller.BeginDelivery(command);

            AssertOk(result);
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnBeginDeliveryWhenValidationFail()
        {
            var command = new BeginDeliveryOperationCommand();
            CmdBusShouldThrowExecpt(command);

            var result = controller.BeginDelivery(command);

            AssertBadRequest(result);
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnBeginRelocation()
        {
            var command = new BeginRelocationOperationCommand();

            var result = controller.BeginRelocation(command);

            AssertOk(result);
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnBeginRelocationWhenValidationFail()
        {
            var command = new BeginRelocationOperationCommand();
            CmdBusShouldThrowExecpt(command);

            var result = controller.BeginRelocation(command);

            AssertBadRequest(result);
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnBeginRelease()
        {
            var command = new BeginReleaseOperationCommand();

            var result = controller.BeginRelease(command);

            AssertOk(result);
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnBeginReleaseWhenValidationFail()
        {
            var command = new BeginReleaseOperationCommand();
            CmdBusShouldThrowExecpt(command);

            var result = controller.BeginRelease(command);

            AssertBadRequest(result);
        }

        protected void AssertOk(IActionResult result)
        {
            result.Should().BeOfType<OkObjectResult>();
            var guid = (result as OkObjectResult).Value as string;
            guid.Should().NotBeNullOrWhiteSpace();
        }

        protected void AssertBadRequest(IActionResult result)
        {
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }

        protected void CmdBusShouldThrowExecpt(ICommand command)
        {
            cmdBusMock.Setup(m=>m.Send(command))
                .Throws(new CommandValidationException("Fail", command,
                    new List<ValidationResult>{new ValidationResult("F", "M")}));
        }
    }
}