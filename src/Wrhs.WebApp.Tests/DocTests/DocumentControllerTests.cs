using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;
using Wrhs.WebApp.src.Controllers.Documents;
using Xunit;

namespace Wrhs.WebApp.Tests.DocTests
{
    public class DocumentControllerTests
    {
        private readonly Mock<ICommandBus> commandBusMock;
        private readonly DocumentController controller;

        public DocumentControllerTests()
        {
            commandBusMock = new Mock<ICommandBus>();
            controller = new DocumentController(commandBusMock.Object);
        }

        [Fact]
        public void ShouldReturnOkOnCreateDeliveryDocumentWhenSucces()
        {
            var command = new CreateDeliveryDocumentCommand();

            var result = controller.CreateDeliveryDocument(command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnOkOnCreateDeliveryDocumentWhenValidationFail()
        {
            var command = new CreateDeliveryDocumentCommand();
            commandBusMock.Setup(m=>m.Send(command))
                .Throws(new CommandValidationException("Validation fail", 
                    command, new List<ValidationResult>{new ValidationResult("Field", "Error")}));
            var controller = new DocumentController(commandBusMock.Object);
            
            var result = controller.CreateDeliveryDocument(command);

            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkOnCreateRelocationDocumentWhenSucces()
        {
            var command = new CreateRelocationDocumentCommand();

            var result = controller.CreateRelocationDocument(command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnOkOnCreateRelocationDocumentWhenValidationFail()
        {
            var command = new CreateRelocationDocumentCommand();
            commandBusMock.Setup(m=>m.Send(command))
                .Throws(new CommandValidationException("Validation fail", 
                    command, new List<ValidationResult>{new ValidationResult("Field", "Error")}));
            var controller = new DocumentController(commandBusMock.Object);
            
            var result = controller.CreateRelocationDocument(command);

            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkOnCreateReleaseDocumentWhenSucces()
        {
            var command = new CreateReleaseDocumentCommand();

            var result = controller.CreateReleaseDocument(command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnOkOnCreateReleaseDocumentWhenValidationFail()
        {
            var command = new CreateReleaseDocumentCommand();
            commandBusMock.Setup(m=>m.Send(command))
                .Throws(new CommandValidationException("Validation fail", 
                    command, new List<ValidationResult>{new ValidationResult("Field", "Error")}));
            var controller = new DocumentController(commandBusMock.Object);
            
            var result = controller.CreateReleaseDocument(command);

            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }
    }
}