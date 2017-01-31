using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public class RemoveDocumentCommandHandler 
        : CommandHandler<RemoveDocumentCommand, RemoveDocumentEvent>
    {
        private readonly IDocumentPersist docPersist;
        private readonly IDocumentService docService;

        public RemoveDocumentCommandHandler(IValidator<RemoveDocumentCommand> validator, IEventBus eventBus, IDocumentPersist docPersist, IDocumentService docService) 
            : base(validator, eventBus)
        {
            this.docPersist = docPersist;
            this.docService = docService;
        }

        protected override void ProcessInvalidCommand(RemoveDocumentCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command", command, results);
        }

        protected override RemoveDocumentEvent ProcessValidCommand(RemoveDocumentCommand command)
        {
            var document = docService.GetDocumentById(command.DocumentId);
            docPersist.Delete(document);

            return new RemoveDocumentEvent(document, DateTime.UtcNow);
        }
    }
}