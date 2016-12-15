using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Delivery
{
    public class ExecuteDeliveryOperationCommandHandler
        : ExecuteOperationCommandHandler<ExecuteDeliveryOperationCommand, ExecuteDeliveryOperationEvent>
    {
        public ExecuteDeliveryOperationCommandHandler(IValidator<ExecuteDeliveryOperationCommand> validator, IEventBus eventBus, HandlerParameters parameters) : base(validator, eventBus, parameters)
        {
        }

        protected override ExecuteDeliveryOperationEvent CreateEvent(Operation operation, DateTime executedAt)
        {
            return new ExecuteDeliveryOperationEvent(operation, executedAt);
        }
    }
}