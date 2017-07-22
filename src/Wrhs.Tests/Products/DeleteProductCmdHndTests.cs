using FluentAssertions;
using Moq;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class DeleteProductCmdHndTests : BaseProductCmdHndTests
    {
        private readonly DeleteProductCommandHandler handler;
        private readonly DeleteProductCommand command;

        public DeleteProductCmdHndTests() : base()
        {
            command = new DeleteProductCommand();
            handler = new DeleteProductCommandHandler(eventBusMock.Object, productSrvMock.Object);
        }
        
        [Fact]
        public void ShouldRemoveProductOnHandle()
        {
            handler.Handle(command);

            productSrvMock.Verify(m=>m.Delete(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public void ShouldPublishEventAfterHandleCommandWhenValid()
        {
            var deleted = new Product();
            productSrvMock.Setup(m=>m.GetProductById(It.IsAny<int>()))
                .Returns(new Product { Name="Product 1", Code = "PROD1" });
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<DeleteProductEvent>()))
                .Callback((DeleteProductEvent @event)=>{
                    deleted = @event.Product;
                }); 

            handler.Handle(command);

            deleted.Name.Should().Be("Product 1");
            deleted.Code.Should().Be("PROD1");
        }
    }
}