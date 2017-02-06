using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public class ExecuteOperationCommandHandler
        : CommandHandler<ExecuteOperationCommand, ExecuteOperationEvent>
    {
        protected readonly IOperationService operationSrv;
        protected readonly IDocumentService docService;
        protected readonly IStockService stockService;  

       public ExecuteOperationCommandHandler(IValidator<ExecuteOperationCommand> validator, IEventBus eventBus,
            HandlerParameters parameters) : base(validator, eventBus)
        {
            this.operationSrv = parameters.OperationService;
            this.docService = parameters.DocumentService;
            this.stockService = parameters.StockService;           
        }   

        protected override void ProcessInvalidCommand(ExecuteOperationCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command.", command, results);
        } 

        protected override ExecuteOperationEvent ProcessValidCommand(ExecuteOperationCommand command)
        {
            var operation = operationSrv.GetOperationByGuid(command.OperationGuid);

            foreach (var shift in operation.Shifts)
            {
                shift.Confirmed = true;
                stockService.Update(shift);
            }

            operation.Document.State = DocumentState.Executed;
            docService.Update(operation.Document);

            operation.Status = OperationStatus.Done;
            operationSrv.Update(operation);

            return CreateEvent(operation, DateTime.UtcNow);
        }


        protected ExecuteOperationEvent CreateEvent(Operation operation, DateTime executedAt)
        {
            return new ExecuteOperationEvent(operation, executedAt);
        }

    }


    public class HandlerParameters
    {
        public IOperationService OperationService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IStockService StockService { get; set; }
    }
}