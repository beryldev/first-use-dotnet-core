using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class BeginRelocationOperationCommandHandler
        : BeginOperationCommandHandler<BeginRelocationOperationCommand, BeginRelocationOperationEvent>
    {
        public BeginRelocationOperationCommandHandler(IValidator<BeginRelocationOperationCommand> validator, 
            IEventBus eventBus, IOperationPersist operationPersist) : base(validator, eventBus, operationPersist)
        {
        }

        protected override BeginRelocationOperationEvent CreateEvent(Operation operation, DateTime createdAt)
        {
            return new BeginRelocationOperationEvent(operation, createdAt);
        }

        protected override Operation RegisterOperation(BeginRelocationOperationCommand command)
        {
             var operation = new Operation
            {
                Type = OperationType.Relocation,
                DocumentId = command.DocumentId,
                OperationGuid = command.OperationGuid
            };

            operationPersist.Save(operation);

            return operation;
        }
    }
}