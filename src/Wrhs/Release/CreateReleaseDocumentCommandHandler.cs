using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class CreateReleaseDocumentCommandHandler : ICommandHandler<CreateReleaseDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public CreateReleaseDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv)
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(CreateReleaseDocumentCommand command)
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

            docSrv.Save(document);

            var evt = new CreateReleaseDocumentEvent(document, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}