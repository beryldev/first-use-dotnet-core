using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Relocation
{
    public class UpdateRelocationDocumentCommandHandler
        : CommandHandler<UpdateRelocationDocumentCommand, UpdateDocumentEvent>
    {
        private readonly IDocumentService docService;

        public UpdateRelocationDocumentCommandHandler(IValidator<UpdateRelocationDocumentCommand> validator,
            IEventBus eventBus, IDocumentService docService) 
            : base(validator, eventBus)
        {
            this.docService = docService;
        }

        protected override void ProcessInvalidCommand(UpdateRelocationDocumentCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command", command, results);
        }

        protected override UpdateDocumentEvent ProcessValidCommand(UpdateRelocationDocumentCommand command)
        {
            var document = docService.GetDocumentById(command.DocumentId);
            document.Remarks = command.Remarks;
            document.Lines.Clear();
            document.Lines.AddRange(
                command.Lines.Select(l => new DocumentLine{
                    ProductId = l.ProductId,
                    Quantity = l.Quantity,
                    SrcLocation = l.SrcLocation,
                    DstLocation = l.DstLocation
                }));

            docService.Update(document);

            return new UpdateDocumentEvent(document.Id, DateTime.UtcNow);
        }
    }
}