using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class ExecuteOperationCommandValidator
        : CommandValidator<ExecuteOperationCommand>
    {
        protected readonly IOperationService operationSrv;
        protected readonly OperationInfoValidator operationValidator;

        public ExecuteOperationCommandValidator(IOperationService operationSrv)
        {
            this.operationSrv = operationSrv;
            operationValidator = new OperationInfoValidator(operationSrv);
        }

        public override IEnumerable<ValidationResult> Validate(ExecuteOperationCommand command)
        {
            results.AddRange(operationValidator.Validate(command));

            var operation = operationSrv.GetOperationByGuid(command.OperationGuid);

            return results;
        }
    }
}