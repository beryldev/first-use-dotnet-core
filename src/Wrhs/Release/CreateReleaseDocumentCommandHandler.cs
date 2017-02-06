using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class CreateReleaseDocumentCommandHandler
        : CreateDocumentCommandHandler<CreateReleaseDocumentCommand, CreateReleaseDocumentEvent>
    {
        public CreateReleaseDocumentCommandHandler(IValidator<CreateReleaseDocumentCommand> validator, IEventBus eventBus,
            IDocumentService docService) : base(validator, eventBus, docService)
        {
        }

        protected override CreateReleaseDocumentEvent CreateEvent(Document document, DateTime createdAt)
        {
            return new CreateReleaseDocumentEvent(document, createdAt);
        }

        protected override Document SaveDocument(CreateReleaseDocumentCommand command)
        {
            var document = new Document
            {
                Type = DocumentType.Release,
                Remarks = command.Remarks,
                Lines = command.Lines.Select(l => new DocumentLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    SrcLocation = l.SrcLocation
                }).ToList()
            };

            docService.Save(document);

            return document;
        }
    }
}