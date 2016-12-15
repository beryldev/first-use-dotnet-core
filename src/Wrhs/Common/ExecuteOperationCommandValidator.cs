using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class ExecuteOperationCommandValidator<TCommand>
        : CommandValidator<TCommand> where TCommand : ExecuteOperationCommand
    {
        protected readonly IOperationService operationSrv;
        protected readonly OperationInfoValidator operationValidator;

        protected ExecuteOperationCommandValidator(IOperationService operationSrv)
        {
            this.operationSrv = operationSrv;
            operationValidator = new OperationInfoValidator(operationSrv);
        }

        public override IEnumerable<ValidationResult> Validate(TCommand command)
        {
            results.AddRange(operationValidator.Validate(command));

            var operation = operationSrv.GetOperationByGuid(command.OperationGuid);
            if(operation != null && operation.Type != GetExpectedOperationType())
                AddValidationResult("OperationGuid", "Invalid operation type.");

            return results;
        }

        protected abstract OperationType GetExpectedOperationType();
    }
}