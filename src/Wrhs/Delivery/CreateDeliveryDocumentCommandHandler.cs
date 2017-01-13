using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class CreateDeliveryDocumentCommandHandler 
        : CreateDocumentCommandHandler<CreateDeliveryDocumentCommand, CreateDeliveryDocumentEvent>
    {
        public CreateDeliveryDocumentCommandHandler(IValidator<CreateDeliveryDocumentCommand> validator, IEventBus eventBus, 
            IDocumentPersist docPersist) : base(validator, eventBus, docPersist)
        {
        }

        protected override CreateDeliveryDocumentEvent CreateEvent(Document document, DateTime createdAt)
        {
            return new CreateDeliveryDocumentEvent(document, createdAt);
        }

        protected override Document SaveDocument(CreateDeliveryDocumentCommand command)
        {
            var document = new Document
            {
                Type = DocumentType.Delivery,
                Remarks = command.Remarks,
                Lines = command.Lines.Select(l => new DocumentLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity
                }).ToList()
            };

            docPersist.Save(document);

            return document;
        }
    }
}