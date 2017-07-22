using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;

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
        public IActionResult BeginDelivery([FromBody]BeginOperationCommand command)
        {
            command.OperationType = OperationType.Delivery;
            return HandleCommand(command);
        }

        [HttpPost("relocation")]
        public IActionResult BeginRelocation([FromBody]BeginOperationCommand command)
        {
            command.OperationType = OperationType.Relocation;
            return HandleCommand(command);
        }

        [HttpPost("release")]
        public IActionResult BeginRelease([FromBody]BeginOperationCommand command)
        {
            command.OperationType = OperationType.Release;
           return HandleCommand(command);
        }

        protected IActionResult HandleCommand<T>(T command) where T : BeginOperationCommand
        {
            command.OperationGuid = GenerateGuid();
            cmdBus.Send(command);
            var result = Ok(command.OperationGuid);

            return result;
        }

        protected string GenerateGuid()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}