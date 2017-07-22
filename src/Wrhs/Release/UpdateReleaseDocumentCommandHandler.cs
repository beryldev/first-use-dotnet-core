using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class UpdateReleaseDocumentCommandHandler : ICommandHandler<UpdateReleaseDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public UpdateReleaseDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv)
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(UpdateReleaseDocumentCommand command)
        {
            var document = docSrv.GetDocumentById(command.DocumentId);
            document.Remarks = command.Remarks;
            document.Lines.Clear();
            document.Lines.AddRange(command.Lines.Select(l => new DocumentLine
            {
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                SrcLocation = l.SrcLocation
            }));

            docSrv.Update(document);

            var evt = new UpdateDocumentEvent(document.Id, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}