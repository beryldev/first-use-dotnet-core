using Wrhs.WebApp.Controllers.Operations;

namespace Wrhs.WebApp.Tests
{
    public class ExecuteOperationControllerTests : OperationControllerTestsBase
    {
        private readonly ExecuteOperationController controller;

        public ExecuteOperationControllerTests() : base()
        {
            controller = new ExecuteOperationController(cmdBusMock.Object);
        }
    }
}