using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
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

        [HttpDelete("delivery/{documentId}")]
        [HttpDelete("relocation/{documentId}")]
        [HttpDelete("release/{documentId}")]
        public IActionResult RemoveDocument(int documentId)
        {
            IActionResult result;

            try
            {
                var command = new RemoveDocumentCommand { DocumentId = documentId};
                commandBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }

            return result;
        }

        [HttpPut("delivery/{documentId}/state")]
        [HttpPut("relocation/{documentId}/state")]
        [HttpPut("release/{documentId}/state")]
        public IActionResult ChangeDocState(int documentId, DocumentState state)
        {
            System.Console.WriteLine($"!!!!!DOCUMENT ID: {documentId}");
            System.Console.WriteLine($"!!!!!NEW STATE: {state}");
            IActionResult result;

            try
            {
                var command = new ChangeDocStateCommand 
                { 
                    DocumentId = documentId,
                    NewState = state
                };
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