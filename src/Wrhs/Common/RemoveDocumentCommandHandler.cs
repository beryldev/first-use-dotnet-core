using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class RemoveDocumentCommandHandler : ICommandHandler<RemoveDocumentCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public RemoveDocumentCommandHandler(IEventBus eventBus, IDocumentService docSrv) 
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(RemoveDocumentCommand command)
        {
            var document = docSrv.GetDocumentById(command.DocumentId);
            docSrv.Delete(document);

            var evt = new RemoveDocumentEvent(document, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}