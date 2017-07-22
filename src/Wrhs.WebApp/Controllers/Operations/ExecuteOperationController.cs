using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation")]
    public class ExecuteOperationController : BaseController
    {
        private readonly ICommandBus cmdBus;  

        public ExecuteOperationController(ICommandBus cmdBus)
        {
            this.cmdBus = cmdBus;
        } 

        [HttpPost("{guid}")]
        public void Execute(string guid)
        {
            var command = new ExecuteOperationCommand { OperationGuid = guid };
            cmdBus.Send(command);
        }
    }
}