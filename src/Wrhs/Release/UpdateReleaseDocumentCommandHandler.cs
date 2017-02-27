using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Release
{
    public class UpdateReleaseDocumentCommandHandler
        : CommandHandler<UpdateReleaseDocumentCommand, UpdateDocumentEvent>
    {
        private readonly IDocumentService docService;

        public UpdateReleaseDocumentCommandHandler(IValidator<UpdateReleaseDocumentCommand> validator,
            IEventBus eventBus, IDocumentService docService) : base(validator, eventBus)
        {
            this.docService = docService;
        }

        protected override void ProcessInvalidCommand(UpdateReleaseDocumentCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command", command, results);
        }

        protected override UpdateDocumentEvent ProcessValidCommand(UpdateReleaseDocumentCommand command)
        {
            var document = docService.GetDocumentById(command.DocumentId);
            document.Remarks = command.Remarks;
            document.Lines.Clear();
            document.Lines.AddRange(command.Lines.Select(l => new DocumentLine{
                ProductId = l.ProductId,
                Quantity = l.Quantity,
                SrcLocation = l.SrcLocation
            }));

            docService.Update(document);

            return new UpdateDocumentEvent(document.Id, DateTime.UtcNow);
        }
    }
}