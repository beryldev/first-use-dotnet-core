using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class RemoveDocumentCommandHandler 
        : CommandHandler<RemoveDocumentCommand, RemoveDocumentEvent>
    {
        public RemoveDocumentCommandHandler(IValidator<RemoveDocumentCommand> validator, IEventBus eventBus) 
            : base(validator, eventBus)
        {
        }

        protected override void ProcessInvalidCommand(RemoveDocumentCommand command, IEnumerable<ValidationResult> results)
        {
            throw new NotImplementedException();
        }

        protected override RemoveDocumentEvent ProcessValidCommand(RemoveDocumentCommand command)
        {
            throw new NotImplementedException();
        }
    }
}