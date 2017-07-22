using Moq;
using Wrhs.Core;
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
        public void ShouldSendCmdToCommandBusOnCreateProduct()
        {
            var command = new CreateProductCommand();

            controller.CreateProduct(command);

            cmdBusMock.Verify(m=>m.Send(It.IsNotNull<ICommand>()), Times.Once());
        }

        [Fact]
        public void ShouldSendCmdToCommandBusOnUpdateProduct()
        {
            var command = new UpdateProductCommand();
            
            controller.UpdateProduct(1, command);

            cmdBusMock.Verify(m=>m.Send(It.IsNotNull<ICommand>()), Times.Once());
        } 
    }
}