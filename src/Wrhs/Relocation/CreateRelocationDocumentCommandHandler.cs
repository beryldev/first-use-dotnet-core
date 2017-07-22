using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class CreateRelocationDocumentCommandHandler : ICommandHandler<CreateRelocationDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public CreateRelocationDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv)
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(CreateRelocationDocumentCommand command)
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

            docSrv.Save(document);

            var evt = new CreateRelocationDocumentEvent(document, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}