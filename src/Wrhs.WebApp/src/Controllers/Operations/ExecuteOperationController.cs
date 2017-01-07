using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

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

        [HttpPost("release/{guid}")]
        [HttpPost("relocation/{guid}")]
        [HttpPost("delivery/{guid}")]
        public IActionResult Execute(string guid, [FromBody]ExecuteOperationCommand command)
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