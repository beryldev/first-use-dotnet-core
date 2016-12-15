using System;
using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Relocation
{
    public class ProcessRelocationOperationCommandValidator
        : ProcessOperationCommandValidator<ProcessRelocationOperationCommand>
    {
        public ProcessRelocationOperationCommandValidator(IOperationService operationSrv, IProductService productSrv) : base(operationSrv, productSrv)
        {
        }

        public override IEnumerable<ValidationResult> Validate(ProcessRelocationOperationCommand command)
        {
            base.Validate(command);

            if(String.IsNullOrWhiteSpace(command.DstLocation))
                AddValidationResult("DstLocation", "Invalid destination location.");

            if(String.IsNullOrWhiteSpace(command.SrcLocation))
                AddValidationResult("SrcLocation", "Invalid source location.");

            if(command.SrcLocation != null && command.DstLocation != null 
                && command.SrcLocation.Equals(command.DstLocation, StringComparison.CurrentCultureIgnoreCase))
                AddValidationResult("SrcLocation | DstLocation", "Destination location can't be source location.");

            return results;
        }

        protected override OperationType GetOperationType()
        {
            return OperationType.Relocation;
        }
    }
}