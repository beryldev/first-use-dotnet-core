using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class ProcessOperationCommandHandler<TCommand, TEvent> :ICommandHandler<TCommand>
        where TCommand : ProcessOperationCommand
        where TEvent : ProcessOperationEvent
    {
        protected readonly IStockService stockService;
        protected readonly IOperationService operationSrv;
        protected readonly IEventBus eventBus;

        public ProcessOperationCommandHandler(IEventBus eventBus, IStockService stockService, IOperationService operationSrv)
        {
            this.eventBus = eventBus;
            this.stockService = stockService;
            this.operationSrv = operationSrv;
        }

        protected int GetOperationId(string operationGuid)
        {
            var operation = operationSrv.GetOperationByGuid(operationGuid);

            return operation.Id;
        }
    
        public void Handle(TCommand command)
        {
            var shifts = RegisterShifts(command);
            var evt = CreateEvent(shifts, DateTime.UtcNow);
            eventBus.Publish(evt);
        }

        protected abstract IEnumerable<Shift> RegisterShifts(TCommand command);

        protected abstract TEvent CreateEvent(IEnumerable<Shift> shifts, DateTime createdAt);

    }
}