using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public abstract class BeginOperationCommandHandler<TCommand, TEvent>
        : CommandHandler<TCommand, TEvent>
        where TCommand : BeginOperationCommand
        where TEvent : BeginOperationEvent
    {
        protected readonly IOperationPersist operationPersist;

        protected BeginOperationCommandHandler(IValidator<TCommand> validator, IEventBus eventBus,
            IOperationPersist operationPersist) : base(validator, eventBus)
        {
            this.operationPersist = operationPersist;
        }

        protected override void ProcessInvalidCommand(TCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command.", command, results);
        }

        protected override TEvent ProcessValidCommand(TCommand command)
        {
            var operation = RegisterOperation(command);
            return CreateEvent(operation, DateTime.UtcNow);
        }

        protected abstract Operation RegisterOperation(TCommand command);

        protected abstract TEvent CreateEvent(Operation operation, DateTime createdAt);
    }
}