using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentCommandHandler
        : CommandHandler<UpdateDeliveryDocumentCommand, UpdateDeliveryDocumentEvent>
    {
        private readonly IDocumentService docService;
        private readonly IDocumentPersist docPersist;

        public UpdateDeliveryDocumentCommandHandler(IValidator<UpdateDeliveryDocumentCommand> validator, IEventBus eventBus, 
            IDocumentPersist docPersist, IDocumentService docService) 
            : base(validator, eventBus)
        {
            this.docService = docService;
            this.docPersist = docPersist;
        }

        protected override void ProcessInvalidCommand(UpdateDeliveryDocumentCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command", command, results);
        }

        protected override UpdateDeliveryDocumentEvent ProcessValidCommand(UpdateDeliveryDocumentCommand command)
        {
            var document = docService.GetDocumentById(command.DocumentId);

            document.Remarks = command.Remarks;

            document.Lines.Clear();

            document.Lines.AddRange(command.Lines.Select(l => new DocumentLine
            {
                ProductId = l.ProductId,
                DstLocation = l.DstLocation,
                Quantity = l.Quantity,
            }));

            docService.Update(document);

            return new UpdateDeliveryDocumentEvent(document.Id, DateTime.UtcNow);
        }
    }
}