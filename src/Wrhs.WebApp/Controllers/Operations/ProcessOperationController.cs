using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation")]
    public class ProcessOperationController : BaseController
    {
        private readonly ICommandBus cmdBus;

        public ProcessOperationController(ICommandBus cmdBus)
        {
            this.cmdBus = cmdBus;
        }

        [HttpPost("delivery/{guid}/shift")]
        public IActionResult ProcessDelivery(string guid, [FromBody]ProcessDeliveryOperationCommand command)
        {
            command.OperationGuid = guid;
            return HandleCommand<ProcessDeliveryOperationCommand>(command);     
        }

        [HttpPost("relocation/{guid}/shift")]
        public IActionResult ProcessRelocation(string guid, [FromBody]ProcessRelocationOperationCommand command)
        {
            command.OperationGuid = guid;
            return HandleCommand<ProcessRelocationOperationCommand>(command);
        }

        [HttpPost("release/{guid}/shift")]
        public IActionResult ProcessRelease(string guid, [FromBody]ProcessReleaseOperationCommand command)
        {
            command.OperationGuid = guid;
            return HandleCommand<ProcessReleaseOperationCommand>(command);
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