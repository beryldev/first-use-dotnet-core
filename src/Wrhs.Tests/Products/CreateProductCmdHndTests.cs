using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class CreateProductCmdHndTests : BaseProductCmdHndTests
    {
        private readonly CreateProductCommand command;

        private readonly Mock<IValidator<CreateProductCommand>> validatorMock;

        private readonly CreateProductCommandHandler handler;

        public CreateProductCmdHndTests() : base()
        {
            command = new CreateProductCommand();
            validatorMock = new Mock<IValidator<CreateProductCommand>>();
            handler = new CreateProductCommandHandler(validatorMock.Object,
                eventBusMock.Object, productPersistMock.Object);  
        }

        [Fact]
        public void ShouldSaveNewProductWhenCmdValid()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<CreateProductCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            productPersistMock.Verify(m=>m.Save(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public void ShouldPublishEventAfterSaveProduct()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<CreateProductCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<CreateProductEvent>()), Times.Once());
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenInvalidCmd()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<CreateProductCommand>()))
                .Returns(new List<ValidationResult>(){new ValidationResult("Field", "Message")});
            
            Assert.Throws<CommandValidationException>(()=>
            {
                handler.Handle(command);
            });

            productPersistMock.Verify(m=>m.Save(It.IsAny<Product>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsAny<CreateProductEvent>()), Times.Never());
        }
    }
}