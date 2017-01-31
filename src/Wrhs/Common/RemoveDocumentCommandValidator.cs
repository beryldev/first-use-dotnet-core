using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class RemoveDocumentCommandValidator : CommandValidator<RemoveDocumentCommand>
    {
        private IDocumentService documentSrv;

        public RemoveDocumentCommandValidator(IDocumentService documentSrv)
        {
            this.documentSrv = documentSrv;
        }

        public override IEnumerable<ValidationResult> Validate(RemoveDocumentCommand command)
        {
            if(command.DocumentId <= 0)
                AddValidationResult("DocumentId", "Invalid document id.");

            var document = documentSrv.GetDocumentById(command.DocumentId);
            if(document == null)
                return results;

            if(document.State != DocumentState.Open)
                AddValidationResult("DocumentId", "Can't remove document. Invalid document state.");

            return results;
        }
    }
}