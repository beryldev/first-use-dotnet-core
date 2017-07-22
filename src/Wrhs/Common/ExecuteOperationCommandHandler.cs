using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public class ExecuteOperationCommandHandler : ICommandHandler<ExecuteOperationCommand>
    {
        private readonly IEventBus eventBus;
        protected readonly IOperationService operationSrv;
        protected readonly IDocumentService docService;
        protected readonly IStockService stockService;  

       public ExecuteOperationCommandHandler(IEventBus eventBus, HandlerParameters parameters)
        {
            this.eventBus = eventBus;
            operationSrv = parameters.OperationService;
            docService = parameters.DocumentService;
            stockService = parameters.StockService;           
        }   

        public void Handle(ExecuteOperationCommand command)
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

            var evt = new ExecuteOperationEvent(operation, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }


    public class HandlerParameters
    {
        public IOperationService OperationService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IStockService StockService { get; set; }
    }
}