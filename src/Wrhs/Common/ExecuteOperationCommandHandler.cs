using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Common
{
    public abstract class ExecuteOperationCommandHandler<TCommand, TEvent>
        : CommandHandler<TCommand, TEvent>
        where TCommand : ExecuteOperationCommand
        where TEvent : ExecuteOperationEvent
    {
        protected readonly IOperationService operationSrv;
        protected readonly IOperationPersist operationPersist;
        protected readonly IDocumentPersist docPersist;
        protected readonly IShiftPersist shiftPersist;  

       public ExecuteOperationCommandHandler(IValidator<TCommand> validator, IEventBus eventBus,
            HandlerParameters parameters) : base(validator, eventBus)
        {
            this.operationSrv = parameters.OperationService;
            this.operationPersist = parameters.OperationPersist;
            this.docPersist = parameters.DocumentPersist;
            this.shiftPersist = parameters.ShiftPersist;           
        }   

        protected override void ProcessInvalidCommand(TCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid command.", command, results);
        } 

        protected override TEvent ProcessValidCommand(TCommand command)
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

        protected abstract TEvent CreateEvent(Operation operation, DateTime executedAt);

    }


    public class HandlerParameters
    {
        public IOperationService OperationService { get; set; }
        public IOperationPersist OperationPersist { get; set; }
        public IDocumentPersist DocumentPersist { get; set; }
        public IShiftPersist ShiftPersist { get; set; }
    }
}