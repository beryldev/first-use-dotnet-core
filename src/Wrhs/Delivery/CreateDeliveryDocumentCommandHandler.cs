using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class CreateDeliveryDocumentCommandHandler : ICommandHandler<CreateDeliveryDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public CreateDeliveryDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv)
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(CreateDeliveryDocumentCommand command)
        {
            var document = new Document
            {
                Type = DocumentType.Delivery,
                Remarks = command.Remarks,
                Lines = command.Lines.Select(l => new DocumentLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    DstLocation = l.DstLocation
                }).ToList()
            };

            docSrv.Save(document);
            
            var evt = new CreateDeliveryDocumentEvent(document, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}