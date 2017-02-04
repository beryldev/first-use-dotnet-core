using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public class ChangeDocStateCommandHandler
        : CommandHandler<ChangeDocStateCommand, ChangeDocStateEvent>
    {
        private readonly IDocumentPersist docPersist;
        private readonly IDocumentService docService;

        public ChangeDocStateCommandHandler(IValidator<ChangeDocStateCommand> validator, IEventBus eventBus, 
            IDocumentPersist docPersist, IDocumentService docService) 
            : base(validator, eventBus)
        {
            this.docPersist = docPersist;
            this.docService = docService;
        }

        protected override void ProcessInvalidCommand(ChangeDocStateCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command.", command, results);
        }

        protected override ChangeDocStateEvent ProcessValidCommand(ChangeDocStateCommand command)
        {
            var document = docService.GetDocumentById(command.DocumentId);
            var oldState = document.State;
            document.State = command.NewState;
            docPersist.Update(document);

            return new ChangeDocStateEvent
            {
                DocumentId = command.DocumentId,
                OldState = oldState,
                NewState = command.NewState
            };
        }
    }
}