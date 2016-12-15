using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;

namespace Wrhs.Tests.Delivery
{
    public class CreateDeliveryDocCmdHndTests
        : CreateDocumentCmdHndTestsBase<CreateDeliveryDocumentCommand, CreateDeliveryDocumentEvent>
    {
        protected override CreateDeliveryDocumentCommand CreateCommand()
        {
            return new CreateDeliveryDocumentCommand
            {
                Lines = new List<CreateDocumentCommand.DocumentLine>()
            };
        }

        protected override ICommandHandler<CreateDeliveryDocumentCommand> CreateCommandHandler(IValidator<CreateDeliveryDocumentCommand> validator, IEventBus eventBus, IDocumentPersist docPersist)
        {
            return new CreateDeliveryDocumentCommandHandler(validator, eventBus, docPersist);
        }

        protected override DocumentType GetDocumentType()
        {
            return DocumentType.Delivery;
        }
    }
}