using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public abstract class ProcessOperationCommandHandler<TCommand, TEvent> : CommandHandler<TCommand, TEvent>
        where TCommand : ProcessOperationCommand
        where TEvent : ProcessOperationEvent
    {
        protected readonly IStockService stockService;
        protected readonly IOperationService operationSrv;

        public ProcessOperationCommandHandler(IValidator<TCommand> validator, IEventBus eventBus,
            IStockService stockService, IOperationService operationSrv) : base(validator, eventBus)
        {
            this.stockService = stockService;
            this.operationSrv = operationSrv;
        }

        protected int GetOperationId(string operationGuid)
        {
            var operation = operationSrv.GetOperationByGuid(operationGuid);

            return operation.Id;
        }

        protected override void ProcessInvalidCommand(TCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command", command, results);
        }

        protected override TEvent ProcessValidCommand(TCommand command)
        {
            var shifts = RegisterShifts(command);
            return CreateEvent(shifts, DateTime.UtcNow);
        }

        protected abstract IEnumerable<Shift> RegisterShifts(TCommand command);

        protected abstract TEvent CreateEvent(IEnumerable<Shift> shifts, DateTime createdAt);
   
    }
}