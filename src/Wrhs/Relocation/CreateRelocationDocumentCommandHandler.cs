using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class CreateRelocationDocumentCommandHandler
        : CreateDocumentCommandHandler<CreateRelocationDocumentCommand, CreateRelocationDocumentEvent>
    {
        public CreateRelocationDocumentCommandHandler(IValidator<CreateRelocationDocumentCommand> validator, IEventBus eventBus, 
            IDocumentService docService) : base(validator, eventBus, docService)
        {
        }

        protected override CreateRelocationDocumentEvent CreateEvent(Document document, DateTime createdAt)
        {
            return new CreateRelocationDocumentEvent(document, createdAt);
        }

        protected override Document SaveDocument(CreateRelocationDocumentCommand command)
        {
            var document = new Document
            {
                Type = DocumentType.Relocation,
                Remarks = command.Remarks,
                Lines = command.Lines.Select(l => new DocumentLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    SrcLocation = l.SrcLocation,
                    DstLocation = l.DstLocation
                }).ToList()
            };

            docService.Save(document);

            return document;
        }
    }
}