using System;
using Warehouse.Validation;

namespace Warehouse.Operations.Delivery
{
    public class OperationDelivery : BaseOperation
    {
        protected override IOperationDocument PerformValidatedOperation()
        {
            return new DocumentDelivery();
        }

        protected override ValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}