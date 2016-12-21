using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;

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

        [HttpPost("delivery/{guid}")]
        public IActionResult ExecuteDelivery(string guid, [FromBody]ExecuteDeliveryOperationCommand command)
        {
            command.OperationGuid = guid;
            return HandleCommand(command);
        }

        [HttpPost("relocation/{guid}")]
        public IActionResult ExecuteRelocation(string guid, [FromBody]ExecuteRelocationOperationCommand command)
        {
            command.OperationGuid = guid;
            return HandleCommand(command);
        }

        [HttpPost("release/{guid}")]
        public IActionResult ExecuteRelease(string guid, [FromBody]ExecuteReleaseOperationCommand command)
        {
            command.OperationGuid = guid;
            return HandleCommand(command);
        }

        protected IActionResult HandleCommand<T>(T command) where T : ICommand
        {
            IActionResult result;

            try
            {          
                cmdBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result =  BadRequest(e.ValidationResults);
            }

            return result;
        }
    }
}