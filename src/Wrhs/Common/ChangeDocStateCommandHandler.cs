using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class ChangeDocStateCommandHandler
        : CommandHandler<ChangeDocStateCommand, ChangeDocStateEvent>
    {
        public ChangeDocStateCommandHandler(IValidator<ChangeDocStateCommand> validator, IEventBus eventBus) 
            : base(validator, eventBus)
        {
        }

        protected override void ProcessInvalidCommand(ChangeDocStateCommand command, IEnumerable<ValidationResult> results)
        {
            throw new NotImplementedException();
        }

        protected override ChangeDocStateEvent ProcessValidCommand(ChangeDocStateCommand command)
        {
            throw new NotImplementedException();
        }
    }
}