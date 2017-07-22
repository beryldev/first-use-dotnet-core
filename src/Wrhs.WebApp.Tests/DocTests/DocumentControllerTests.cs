using Moq;
using Wrhs.Core;
using Wrhs.WebApp.Controllers.Documents;

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
    }
}