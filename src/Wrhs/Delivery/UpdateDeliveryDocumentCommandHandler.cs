using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentCommandHandler : ICommandHandler<UpdateDeliveryDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public UpdateDeliveryDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv)
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(UpdateDeliveryDocumentCommand command)
        {
            var document = docSrv.GetDocumentById(command.DocumentId);

            document.Remarks = command.Remarks;

            document.Lines.Clear();

            document.Lines.AddRange(command.Lines.Select(l => new DocumentLine
            {
                ProductId = l.ProductId,
                DstLocation = l.DstLocation,
                Quantity = l.Quantity,
            }));

            docSrv.Update(document);

            var evt = new UpdateDocumentEvent(document.Id, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}