using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.WebApp.Tests
{
    public abstract class OperationControllerTestsBase
    {
        protected readonly Mock<ICommandBus> cmdBusMock;

        public OperationControllerTestsBase()
        {
            cmdBusMock = new Mock<ICommandBus>();
        }

        protected void AssertBadRequest(IActionResult result)
        {
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }

        protected void SetupCmdBusValidationFails(ICommand command)
        {
            cmdBusMock.Setup(m=>m.Send(It.IsNotNull<ICommand>()))
                .Throws(new CommandValidationException("Fail", command,
                    new List<ValidationResult>{ new ValidationResult("F", "M")}));
        }
    }
}