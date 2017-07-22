using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class BeginOperationCommandHandler : ICommandHandler<BeginOperationCommand>
    {
        private readonly IOperationService operationService;
        private readonly IEventBus eventBus;

        public BeginOperationCommandHandler(IEventBus eventBus, IOperationService operationService)
        {
            this.eventBus = eventBus;
            this.operationService = operationService;
        }

        public void Handle(BeginOperationCommand command)
        {
            var operation = new Operation
            {
                Type = command.OperationType,
                DocumentId = command.DocumentId,
                OperationGuid = command.OperationGuid
            };

            operationService.Save(operation);

            var evt = new BeginOperationEvent(operation, DateTime.UtcNow);
            eventBus.Publish<BeginOperationEvent>(evt);
        }
    }
}