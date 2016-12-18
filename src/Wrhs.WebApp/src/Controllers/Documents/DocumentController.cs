using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Delivery;
using Wrhs.Release;
using Wrhs.Relocation;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document")]
    public class DocumentController : BaseController
    {
        private readonly ICommandBus commandBus;

        public DocumentController(ICommandBus commandBus)
        {
             this.commandBus = commandBus;
        }

        [HttpPost("delivery")]
        public IActionResult CreateDeliveryDocument([FromBody]CreateDeliveryDocumentCommand command)
        {
            IActionResult result;

            try
            {
                commandBus.Send(command ?? new CreateDeliveryDocumentCommand());
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }
            
            return result;
        }

        [HttpPost("relocation")]
        public IActionResult CreateRelocationDocument([FromBody]CreateRelocationDocumentCommand command)
        {
            IActionResult result;

            try
            {
                commandBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }
            
            return result;
        }

        [HttpPost("release")]
        public IActionResult CreateReleaseDocument([FromBody]CreateReleaseDocumentCommand command)
        {
            IActionResult result;

            try
            {
                commandBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }
            
            return result;
        }


    }
}