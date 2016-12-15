using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public abstract class CreateDocumentCommandHandler<TCommand, TEvent>
        : CommandHandler<TCommand, TEvent>
        where TCommand : CreateDocumentCommand
        where TEvent : IEvent
    {
        
        protected readonly IDocumentPersist docPersist;

        public CreateDocumentCommandHandler(IValidator<TCommand> validator, IEventBus eventBus, 
            IDocumentPersist docPersist) : base(validator, eventBus)
        {
            this.docPersist = docPersist;
        }

        protected abstract Document SaveDocument(TCommand command);

        protected abstract TEvent CreateEvent(Document document, DateTime createdAt);

        protected override TEvent ProcessValidCommand(TCommand command)
        {
            var document = SaveDocument(command);
            return CreateEvent(document, DateTime.UtcNow);
        }

        protected override void ProcessInvalidCommand(TCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid create delivery document command.",
                    command, results);
        }
    }
}