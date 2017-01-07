using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class BeginOperationCommandValidator
        : CommandValidator<BeginOperationCommand>
    {
        protected readonly IDocumentService documentSrv;
        protected readonly IOperationService operationSrv;
        protected readonly OperationInfoValidator operationValidator;

        public BeginOperationCommandValidator(IDocumentService documentSrv,
            IOperationService operationSrv)
        {
            this.documentSrv = documentSrv;
            this.operationSrv = operationSrv;
            operationValidator = new OperationInfoValidator(operationSrv, true);
        }

        public override IEnumerable<ValidationResult> Validate(BeginOperationCommand command)
        {
            results.AddRange(operationValidator.Validate(command));

            if(!documentSrv.CheckDocumentExistsById(command.DocumentId))
                AddValidationResult("DocumentId", "Delivery document not exists.");

            var document = documentSrv.GetDocumentById(command.DocumentId);
            if(document.State != DocumentState.Confirmed)
                AddValidationResult("DocumentId", $"Invalid document state: {document.State}");

            if(command.OperationType != (OperationType)document.Type)
                AddValidationResult("OperationType", "Operation type inconsistent with document type");
            
            return results;
        }
    }
}