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
        protected readonly IOperationPersist operationPersist;
        protected readonly IDocumentPersist docPersist;
        protected readonly IShiftPersist shiftPersist;  

       public ExecuteOperationCommandHandler(IValidator<ExecuteOperationCommand> validator, IEventBus eventBus,
            HandlerParameters parameters) : base(validator, eventBus)
        {
            this.operationSrv = parameters.OperationService;
            this.operationPersist = parameters.OperationPersist;
            this.docPersist = parameters.DocumentPersist;
            this.shiftPersist = parameters.ShiftPersist;           
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
                shiftPersist.Update(shift);
            }

            operation.Document.State = DocumentState.Executed;
            docPersist.Update(operation.Document);

            operation.Status = OperationStatus.Done;
            operationPersist.Update(operation);

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
        public IOperationPersist OperationPersist { get; set; }
        public IDocumentPersist DocumentPersist { get; set; }
        public IShiftPersist ShiftPersist { get; set; }
    }
}