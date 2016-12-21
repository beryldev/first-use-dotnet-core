using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation")]
    public class BeginOperationController : BaseController
    {
        private readonly ICommandBus cmdBus;

        public BeginOperationController(ICommandBus cmdBus)
        {
            this.cmdBus = cmdBus;
        }

        [HttpPost("delivery")]
        public IActionResult BeginDelivery([FromBody]BeginDeliveryOperationCommand command)
        {
            return HandleCommand(command);
        }

        [HttpPost("relocation")]
        public IActionResult BeginRelocation([FromBody]BeginRelocationOperationCommand command)
        {
            return HandleCommand(command);
        }

        [HttpPost("release")]
        public IActionResult BeginRelease([FromBody]BeginReleaseOperationCommand command)
        {
           return HandleCommand(command);
        }

        protected IActionResult HandleCommand<T>(T command) where T : BeginOperationCommand
        {
            IActionResult result;

            try
            {
                command.OperationGuid = GenerateGuid();
                cmdBus.Send(command);
                result = Ok(command.OperationGuid);
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }

            return result;
        }

        protected string GenerateGuid()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}