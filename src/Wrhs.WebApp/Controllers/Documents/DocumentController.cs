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
        public void CreateDeliveryDocument([FromBody]CreateDeliveryDocumentCommand command)
        {
            commandBus.Send(command);
        }

        [HttpPost("relocation")]
        public void CreateRelocationDocument([FromBody]CreateRelocationDocumentCommand command)
        {
            commandBus.Send(command);
        }

        [HttpPost("release")]
        public void CreateReleaseDocument([FromBody]CreateReleaseDocumentCommand command)
        {
            commandBus.Send(command);
        }

        [HttpDelete("delivery/{documentId}")]
        [HttpDelete("relocation/{documentId}")]
        [HttpDelete("release/{documentId}")]
        public void RemoveDocument(int documentId)
        {
            var command = new RemoveDocumentCommand { DocumentId = documentId};
            commandBus.Send(command);
        }

        [HttpPut("delivery/{documentId}/state")]
        [HttpPut("relocation/{documentId}/state")]
        [HttpPut("release/{documentId}/state")]
        public void ChangeDocState(int documentId, DocumentState state)
        {
            var command = new ChangeDocStateCommand 
            { 
                DocumentId = documentId,
                NewState = state
            };

            commandBus.Send(command);
        }


        [HttpPut("delivery/{documentId}")]
        public void UpdateDeliveryDocument(int documentId, [FromBody]UpdateDeliveryDocumentCommand command)
        {
            command.DocumentId = documentId;
            commandBus.Send(command);
        }

        [HttpPut("relocation/{documentId}")]
        public void UpdateRelocationDocument(int documentId, [FromBody]UpdateRelocationDocumentCommand command)
        {
            command.DocumentId = documentId;
            commandBus.Send(command);
        }

        [HttpPut("release/{documentId}")]
        public void UpdateReleaseDocument(int documentId, [FromBody]UpdateReleaseDocumentCommand command)
        {
            command.DocumentId = documentId;
            commandBus.Send(command);
        }
    }
}