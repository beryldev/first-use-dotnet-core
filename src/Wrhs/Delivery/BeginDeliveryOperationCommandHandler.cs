using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class BeginDeliveryOperationCommandHandler 
        : BeginOperationCommandHandler<BeginDeliveryOperationCommand, BeginDeliveryOperationEvent>
    {
        public BeginDeliveryOperationCommandHandler(IValidator<BeginDeliveryOperationCommand> validator, 
            IEventBus eventBus, IOperationPersist operationPersist) : base(validator, eventBus, operationPersist)
        {
        }

        protected override BeginDeliveryOperationEvent CreateEvent(Operation operation, DateTime createdAt)
        {
            return new BeginDeliveryOperationEvent(operation, createdAt);
        }

        protected override Operation RegisterOperation(BeginDeliveryOperationCommand command)
        {
            var operation = new Operation
            {
                Type = OperationType.Delivery,
                DocumentId = command.DocumentId,
                OperationGuid = command.OperationGuid
            };

            operationPersist.Save(operation);

            return operation;
        }
    }
}