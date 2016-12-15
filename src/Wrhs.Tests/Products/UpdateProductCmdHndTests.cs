using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class UpdateProductCmdHndTests : BaseProductCmdHndTests
    {
        private readonly UpdateProductCommand command;

        private readonly Mock<IValidator<UpdateProductCommand>> validatorMock;

        private readonly UpdateProductCommandHandler handler;

        public UpdateProductCmdHndTests() : base()
        {
            command = new UpdateProductCommand();
            validatorMock = new Mock<IValidator<UpdateProductCommand>>();
            handler = new UpdateProductCommandHandler(validatorMock.Object,
                eventBusMock.Object, productPersistMock.Object, productSrvMock.Object);
            productSrvMock.Setup(m=>m.GetProductById(It.IsAny<int>())).Returns(new Product());
        }

        [Fact]
        public void ShouldUpdateProductWhenCmdValid()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<UpdateProductCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            productPersistMock.Verify(m=>m.Update(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public void ShouldPublishEventAfterUpdate()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<UpdateProductCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<UpdateProductEvent>()), Times.Once());
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenValidationFails()
        {
             validatorMock.Setup(m=>m.Validate(It.IsAny<UpdateProductCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("Field", "Message")});

            Assert.Throws<CommandValidationException>(()=>
            {
                handler.Handle(command);
            });

            productPersistMock.Verify(m=>m.Update(It.IsAny<Product>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsAny<UpdateProductEvent>()), Times.Never());
        }
    }
}