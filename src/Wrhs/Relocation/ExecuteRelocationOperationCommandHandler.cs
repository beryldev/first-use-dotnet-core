using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Relocation
{
    public class ExecuteRelocationOperationCommandHandler
        : ExecuteOperationCommandHandler<ExecuteRelocationOperationCommand, ExecuteRelocationOperationEvent>
    {
        public ExecuteRelocationOperationCommandHandler(IValidator<ExecuteRelocationOperationCommand> validator, IEventBus eventBus, HandlerParameters parameters) : base(validator, eventBus, parameters)
        {
        }

        protected override ExecuteRelocationOperationEvent CreateEvent(Operation operation, DateTime executedAt)
        {
            return new ExecuteRelocationOperationEvent(operation, executedAt);
        }
    }
}