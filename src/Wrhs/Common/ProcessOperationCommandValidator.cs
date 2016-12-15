using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Common
{
    public abstract class ProcessOperationCommandValidator<TCommand>
        : CommandValidator<TCommand>
        where TCommand : ProcessOperationCommand
    {
        protected readonly IOperationService operationSrv;
        protected readonly ProductInfoValidator productValidator;
        protected readonly OperationInfoValidator operationValidator;

        public ProcessOperationCommandValidator(IOperationService operationSrv, IProductService productSrv)
        {
            this.operationSrv = operationSrv;
            productValidator = new ProductInfoValidator(productSrv);
            operationValidator = new OperationInfoValidator(operationSrv);
        }

        protected abstract OperationType GetOperationType();

        public override IEnumerable<ValidationResult> Validate(TCommand command)
        {
            results.AddRange(operationValidator.Validate(command));

            results.AddRange(productValidator.Validate(command));
            
            var operation = operationSrv.GetOperationByGuid(command.OperationGuid);
            if(operation != null && operation.Type != GetOperationType())
                AddValidationResult("OperationGuid", $"Invalid operation type.");

            return results;
        }
    }
}