using System;
using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Delivery
{
    public class ProcessDeliveryOperationCommandValidator
        : ProcessOperationCommandValidator<ProcessDeliveryOperationCommand>
    {
        public ProcessDeliveryOperationCommandValidator(IOperationService operationSrv, IProductService productSrv)
            : base(operationSrv, productSrv)
        {
        }

        public override IEnumerable<ValidationResult> Validate(ProcessDeliveryOperationCommand command)
        {
            base.Validate(command);

            if(String.IsNullOrWhiteSpace(command.DstLocation))
                AddValidationResult("DstLocation", "Invalid destination location.");

            return results;
        }

        protected override OperationType GetOperationType()
        {
            return OperationType.Delivery;
        }
    }
}