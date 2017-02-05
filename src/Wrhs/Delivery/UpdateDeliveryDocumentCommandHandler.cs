using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentCommandHandler
        : CreateDocumentCommandHandler<UpdateDeliveryDocumentCommand, UpdateDeliveryDocumentEvent>
    {
        public UpdateDeliveryDocumentCommandHandler(IValidator<UpdateDeliveryDocumentCommand> validator, IEventBus eventBus, IDocumentPersist docPersist) 
            : base(validator, eventBus, docPersist)
        {
        }

        protected override UpdateDeliveryDocumentEvent CreateEvent(Document document, DateTime createdAt)
        {
            throw new NotImplementedException();
        }

        protected override Document SaveDocument(UpdateDeliveryDocumentCommand command)
        {
            throw new NotImplementedException();
        }
    }
}