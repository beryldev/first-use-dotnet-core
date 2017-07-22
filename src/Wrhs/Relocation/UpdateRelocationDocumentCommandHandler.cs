using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class UpdateRelocationDocumentCommandHandler : ICommandHandler<UpdateRelocationDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public UpdateRelocationDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv) 
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(UpdateRelocationDocumentCommand command)
        {
            var document = docSrv.GetDocumentById(command.DocumentId);
            document.Remarks = command.Remarks;
            document.Lines.Clear();
            document.Lines.AddRange(
                command.Lines.Select(l => new DocumentLine
                {
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    SrcLocation = l.SrcLocation,
                    DstLocation = l.DstLocation
                }));

            docSrv.Update(document);

            var evt = new UpdateDocumentEvent(document.Id, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}