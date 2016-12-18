using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;
using Wrhs.WebApp.Controllers;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class ProductControllerTests
    {
        private readonly ProductController controller;
        private readonly Mock<ICommandBus> cmdBusMock;

        public ProductControllerTests()
        {
            cmdBusMock = new Mock<ICommandBus>();
            cmdBusMock.Setup(m=>m.Send(It.IsNotNull<ICommand>())).Verifiable();
            controller = new ProductController(cmdBusMock.Object);
        }

        [Fact]
        public void ShouldReturnOkOnCreateProductWhenSuccess()
        {
            var command = new CreateProductCommand();

            var result = controller.CreateProduct(command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldSendCmdToCommandBusOnCreateProduct()
        {
            var command = new CreateProductCommand();

            controller.CreateProduct(command);

            cmdBusMock.Verify(m=>m.Send(It.IsNotNull<ICommand>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsWhenCmdValidationFail()
        {
            var command = new CreateProductCommand();
            cmdBusMock.Setup(m=>m.Send(It.IsNotNull<ICommand>()))
                .Throws(new CommandValidationException("Message", command,
                    new List<ValidationResult>{new ValidationResult("Field", "Message")}));

            var result = controller.CreateProduct(command);

            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnOkOnUpdateWhenSuccess()
        {
            var command = new UpdateProductCommand();

            var result = controller.UpdateProduct(1, command);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldSendCmdToCommandBusOnUpdateProduct()
        {
            var command = new UpdateProductCommand();
            
            controller.UpdateProduct(1, command);

            cmdBusMock.Verify(m=>m.Send(It.IsNotNull<ICommand>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnUpdateProductWhenValidationFail()
        {
            var command = new UpdateProductCommand();
            cmdBusMock.Setup(m=>m.Send(It.IsNotNull<ICommand>()))
                .Throws(new CommandValidationException("Message", command,
                    new List<ValidationResult>{new ValidationResult("Field", "Message")}));

            var result = controller.UpdateProduct(1, command);
            
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            errors.Should().NotBeNullOrEmpty();
        }

        // [Fact]
        // public void ShouldReturnBadRequestOnUpdateWhenFail()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new UpdateProductCommand();
        //     var handler = new Mock<ICommandHandler<UpdateProductCommand>>();
        //     var validator = new Mock<IValidator<UpdateProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>(){new ValidationResult()});
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Update(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<BadRequestObjectResult>(result);
        // }

        // [Fact]
        // public void ShouldReturnOkOnDeleteWhenSuccess()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new DeleteProductCommand();
        //     var handler = new Mock<ICommandHandler<DeleteProductCommand>>();
        //     var validator = new Mock<IValidator<DeleteProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>());
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Delete(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<OkResult>(result);
        // }

        // [Fact]
        // public void ShouldReturnBadRequestOnDeleteWhenFail()
        // {
        //     var repository = new Mock<IRepository<Product>>();
        //       repository.Setup(m=>m.GetById(It.IsAny<int>()))
        //         .Returns(new Product());
        //     var cmd = new DeleteProductCommand();
        //     var handler = new Mock<ICommandHandler<DeleteProductCommand>>();
        //     var validator = new Mock<IValidator<DeleteProductCommand>>();
        //     validator.Setup(m=>m.Validate(cmd))
        //         .Returns(new List<ValidationResult>(){new ValidationResult()});
        //     var controller = new ProductController(repository.Object);

        //     var result = controller.Delete(1, cmd, validator.Object, handler.Object);

        //     Assert.IsType<BadRequestObjectResult>(result);
        // }

        
    }
}