using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public class BeginOperationCommandHandler 
        : CommandHandler<BeginOperationCommand, BeginOperationEvent>
    {
        protected readonly IOperationPersist operationPersist;

        public BeginOperationCommandHandler(IValidator<BeginOperationCommand> validator, IEventBus eventBus,
            IOperationPersist operationPersist) : base(validator, eventBus)
        {
            this.operationPersist = operationPersist;
        }

        protected override void ProcessInvalidCommand(BeginOperationCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command.", command, results);
        }

        protected override BeginOperationEvent ProcessValidCommand(BeginOperationCommand command)
        {
            var operation = RegisterOperation(command);
            return CreateEvent(operation, DateTime.UtcNow);
        }

         protected BeginOperationEvent CreateEvent(Operation operation, DateTime createdAt)
        {
            return new BeginOperationEvent(operation, createdAt);
        }

        protected Operation RegisterOperation(BeginOperationCommand command)
        {
            var operation = new Operation
            {
                Type = command.OperationType,
                DocumentId = command.DocumentId,
                OperationGuid = command.OperationGuid
            };

            operationPersist.Save(operation);

            return operation;
        }
    }
}