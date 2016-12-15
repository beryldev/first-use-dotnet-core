using System;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Release
{
    public class ExecuteReleaseOperationCommandHandler
        : ExecuteOperationCommandHandler<ExecuteReleaseOperationCommand, ExecuteReleaseOperationEvent>
    {
        public ExecuteReleaseOperationCommandHandler(IValidator<ExecuteReleaseOperationCommand> validator, 
            IEventBus eventBus, HandlerParameters parameters) : base(validator, eventBus, parameters)
        {
        }

        protected override ExecuteReleaseOperationEvent CreateEvent(Operation operation, DateTime executedAt)
        {
            return new ExecuteReleaseOperationEvent(operation, executedAt);
        }
    }
}