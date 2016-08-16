using System;
using Warehouse.Validation;

namespace Warehouse.Operations
{
    public abstract class BaseOperation : IWarehouseOperation
    {
        public event EventHandler<ValidationResult> OnValidationFailed;

        public IOperationDocument Perform()
        {
            var result = Validate();
            if(result.IsValid)
                return PerfromValidatedOperation();

            OnValidationFailed.Invoke(this, result);
            throw new NotImplementedException(); //TODO
        }

        protected abstract IOperationDocument PerfromValidatedOperation();

        protected abstract ValidationResult Validate();
    }
}