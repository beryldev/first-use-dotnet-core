using System;
using Warehouse.Validation;

namespace Warehouse.Operations
{
    public abstract class BaseOperation : IWarehouseOperation
    {
        public event EventHandler<ValidationResult> OnValidationFailed;

        public IOperationResult Perform()
        {
            var operationResult = new OperationResult();
            var result = Validate();
            if(result.IsValid)
            {
                operationResult.OperationDocument = PerformValidatedOperation();
                operationResult.Status = ResultStatus.OK;
            }

            OnValidationFailed.Invoke(this, result);
            
            operationResult.Status = ResultStatus.Error;

            return operationResult;
        }

        protected abstract IOperationDocument PerformValidatedOperation();

        protected abstract ValidationResult Validate();
    }
}