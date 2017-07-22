using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
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
        public void ProcessDelivery(string guid, [FromBody]ProcessDeliveryOperationCommand command)
        {
            command.OperationGuid = guid;
            cmdBus.Send(command);
        }

        [HttpPost("relocation/{guid}/shift")]
        public void ProcessRelocation(string guid, [FromBody]ProcessRelocationOperationCommand command)
        {
            command.OperationGuid = guid;
            cmdBus.Send(command);
        }

        [HttpPost("release/{guid}/shift")]
        public void ProcessRelease(string guid, [FromBody]ProcessReleaseOperationCommand command)
        {
            command.OperationGuid = guid;
            cmdBus.Send(command);
        }
    }
}