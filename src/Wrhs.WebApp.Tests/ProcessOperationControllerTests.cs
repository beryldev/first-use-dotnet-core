using Wrhs.WebApp.Controllers.Operations;

namespace Wrhs.WebApp.Tests
{
    public class ProcessOperationControllerTests : OperationControllerTestsBase
    {
        private readonly ProcessOperationController controller;
        
        public ProcessOperationControllerTests() : base()
        {
            controller = new ProcessOperationController(cmdBusMock.Object);
        } 
    }
}