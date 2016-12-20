using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class DeleteProductCmdHndTests : BaseProductCmdHndTests
    {
        private readonly DeleteProductCommandHandler handler;
        private readonly Mock<IValidator<DeleteProductCommand>> validatorMock;
        private readonly DeleteProductCommand command;

        public DeleteProductCmdHndTests() : base()
        {
            command = new DeleteProductCommand();
            validatorMock = new Mock<IValidator<DeleteProductCommand>>();
            handler = new DeleteProductCommandHandler(validatorMock.Object, eventBusMock.Object,
                productSrvMock.Object, productPersistMock.Object);
        }
        
        [Fact]
        public void ShouldRemoveProductOnHandleCommandWhenValid()
        {
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<DeleteProductCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            productPersistMock.Verify(m=>m.Delete(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public void ShouldPublishEventAfterHandleCommandWhenValid()
        {
            var deleted = new Product();
            productSrvMock.Setup(m=>m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Name="Product 1", Code = "PROD1" });
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<DeleteProductCommand>()))
                .Returns(new List<ValidationResult>());
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<DeleteProductEvent>()))
                .Callback((DeleteProductEvent @event)=>{
                    deleted = @event.Product;
                }); 

            handler.Handle(command);

            deleted.Name.Should().Be("Product 1");
            deleted.Code.Should().Be("PROD1");
        }

        [Fact]
        public void ShouldOnlyThrowExceptionOnHandleWhenValidationFail()
        {
            validatorMock.Setup(m=>m.Validate(It.IsNotNull<DeleteProductCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("Field", "Message")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            productPersistMock.Verify(m=>m.Delete(It.IsAny<Product>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsNotNull<DeleteProductEvent>()), Times.Never());
        }
    }
}