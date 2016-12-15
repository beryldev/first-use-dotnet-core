using System.Collections.Generic;
using System.Linq;

namespace Wrhs.Core
{
    public abstract class CommandHandler<TCommand, TEvent> : ICommandHandler<TCommand>
        where TCommand : ICommand
        where TEvent : IEvent
    {
        protected IValidator<TCommand> validator;

        protected IEventBus eventBus;

        public CommandHandler(IValidator<TCommand> validator, IEventBus eventBus)
        {
            this.validator = validator;
            this.eventBus = eventBus;
        }

        public void Handle(TCommand command)
        {
            var result = validator.Validate(command);

            if(!result.Any())
            {
                var @event = ProcessValidCommand(command);
                eventBus.Publish(@event);
            }
            else
            {
                ProcessInvalidCommand(command, result);
            }
        }

        protected abstract TEvent ProcessValidCommand(TCommand command);
    
        protected abstract void ProcessInvalidCommand(TCommand command, IEnumerable<ValidationResult> results);
    }
}