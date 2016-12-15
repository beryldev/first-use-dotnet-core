using Moq;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Services;

namespace Wrhs.Tests.Products
{
    public abstract class BaseProductCmdHndTests
    {
        protected readonly Mock<IProductService> productSrvMock;

        protected readonly Mock<IProductPersist> productPersistMock;

        protected readonly Mock<IEventBus> eventBusMock;

        public BaseProductCmdHndTests()
        {
            productSrvMock = new Mock<IProductService>();
            productPersistMock = new Mock<IProductPersist>();
            eventBusMock = new Mock<IEventBus>();
        }
    }
}