using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class BeginReleaseOperationCommandHandler
        : BeginOperationCommandHandler<BeginReleaseOperationCommand, BeginReleaseOperationEvent>
    {
        public BeginReleaseOperationCommandHandler(IValidator<BeginReleaseOperationCommand> validator,
            IEventBus eventBus, IOperationPersist operationPersist) 
            : base(validator, eventBus, operationPersist)
        {
        }

        protected override BeginReleaseOperationEvent CreateEvent(Operation operation, DateTime createdAt)
        {
            return new BeginReleaseOperationEvent(operation, createdAt);
        }

        protected override Operation RegisterOperation(BeginReleaseOperationCommand command)
        {
            var operation = new Operation
            {
                Type = OperationType.Release,
                OperationGuid = command.OperationGuid,
                DocumentId = command.DocumentId,
                Status = OperationStatus.InProgress
            };

            operationPersist.Save(operation);

            return operation;
        }
    }
}