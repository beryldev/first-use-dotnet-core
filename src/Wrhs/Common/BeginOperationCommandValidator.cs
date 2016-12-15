using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class BeginOperationCommandValidator<TCommand>
        : CommandValidator<TCommand> where TCommand : BeginOperationCommand
    {
        protected readonly IDocumentService documentSrv;
        protected readonly IOperationService operationSrv;
        protected readonly OperationInfoValidator operationValidator;

        protected BeginOperationCommandValidator(IDocumentService documentSrv,
            IOperationService operationSrv)
        {
            this.documentSrv = documentSrv;
            this.operationSrv = operationSrv;
            operationValidator = new OperationInfoValidator(operationSrv, true);
        }

        public override IEnumerable<ValidationResult> Validate(TCommand command)
        {
            results.AddRange(operationValidator.Validate(command));

            if(!documentSrv.CheckDocumentExistsById(command.DocumentId))
                AddValidationResult("DocumentId", "Delivery document not exists.");

            var document = documentSrv.GetDocumentById(command.DocumentId);
            if(document.State != DocumentState.Confirmed)
                AddValidationResult("DocumentId", $"Invalid document state: {document.State}");

            if(document.Type != GetExpectedDocumentType())
                AddValidationResult("OperationGuid", 
                    $"Invalid document type. Expected: {GetExpectedDocumentType()} given: {document.Type}");

            return results;
        }

        protected abstract DocumentType GetExpectedDocumentType();
    }
}