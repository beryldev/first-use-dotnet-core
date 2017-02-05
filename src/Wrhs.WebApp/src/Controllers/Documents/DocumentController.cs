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
           return HandleRequest<CreateDeliveryDocumentCommand>(command);
        }

        [HttpPost("relocation")]
        public IActionResult CreateRelocationDocument([FromBody]CreateRelocationDocumentCommand command)
        {
           return HandleRequest<CreateRelocationDocumentCommand>(command);
        }

        [HttpPost("release")]
        public IActionResult CreateReleaseDocument([FromBody]CreateReleaseDocumentCommand command)
        {
            return HandleRequest<CreateReleaseDocumentCommand>(command);
        }

        [HttpDelete("delivery/{documentId}")]
        [HttpDelete("relocation/{documentId}")]
        [HttpDelete("release/{documentId}")]
        public IActionResult RemoveDocument(int documentId)
        {
            var command = new RemoveDocumentCommand { DocumentId = documentId};
            
            return HandleRequest<RemoveDocumentCommand>(command);
        }

        [HttpPut("delivery/{documentId}/state")]
        [HttpPut("relocation/{documentId}/state")]
        [HttpPut("release/{documentId}/state")]
        public IActionResult ChangeDocState(int documentId, DocumentState state)
        {
            var command = new ChangeDocStateCommand 
            { 
                DocumentId = documentId,
                NewState = state
            };

            return HandleRequest<ChangeDocStateCommand>(command);
        }


        [HttpPut("delivery/{documentId}")]
        public IActionResult UpdateDeliveryDocument(int documentId, [FromBody]UpdateDeliveryDocumentCommand command)
        {
            command.DocumentId = documentId;
            return HandleRequest<UpdateDeliveryDocumentCommand>(command);
        }

        protected IActionResult HandleRequest<T>(T command) where T : ICommand
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