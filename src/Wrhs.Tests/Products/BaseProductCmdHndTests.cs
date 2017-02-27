using Moq;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Tests.Products
{
    public abstract class BaseProductCmdHndTests
    {
        protected readonly Mock<IProductService> productSrvMock;

        protected readonly Mock<IEventBus> eventBusMock;

        public BaseProductCmdHndTests()
        {
            productSrvMock = new Mock<IProductService>();
            eventBusMock = new Mock<IEventBus>();
        }
    }
}