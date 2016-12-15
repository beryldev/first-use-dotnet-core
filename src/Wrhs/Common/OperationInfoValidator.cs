using System;
using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class OperationInfoValidator : IValidator<IValidableOperationInfo>
    {
        private readonly IOperationService operationSrv;
        private readonly bool beginOperation;
        private readonly List<ValidationResult> results; 

        public OperationInfoValidator(IOperationService operationSrv, bool beginOperation=false)
        {
            this.operationSrv = operationSrv;
            this.beginOperation = beginOperation;
            results = new List<ValidationResult>();
        }

        public IEnumerable<ValidationResult> Validate(IValidableOperationInfo command)
        {
            if(String.IsNullOrWhiteSpace(command.OperationGuid))
                AddValidationResult("OperationGuid", "Invalid operation guid.");

            if(beginOperation && operationSrv.CheckOperationGuidExists(command.OperationGuid))
                AddValidationResult("OperationGuid", $"Operation with guid: {command.OperationGuid} exists.");
       
            if(!beginOperation && !operationSrv.CheckOperationGuidExists(command.OperationGuid))
                AddValidationResult("OperationGuid", $"Operation with guid: {command.OperationGuid} not exists.");

            return results;
        }

        private void AddValidationResult(string field, string message)
        {
            results.Add(new ValidationResult(field, message));
        }
    }
}