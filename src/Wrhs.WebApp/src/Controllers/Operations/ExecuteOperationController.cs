using Wrhs.Core;

namespace Wrhs.WebApp.Controllers.Operations
{
    public class ExecuteOperationController : BaseController
    {
        private readonly ICommandBus cmdBus;  

        public ExecuteOperationController(ICommandBus cmdBus)
        {
            this.cmdBus = cmdBus;
        } 

        
    }
}