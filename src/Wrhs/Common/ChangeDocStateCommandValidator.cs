using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class ChangeDocStateCommandValidator 
        : CommandValidator<ChangeDocStateCommand>
    {
        private readonly IDocumentService documentSrv;

        public ChangeDocStateCommandValidator(IDocumentService documentSrv)
        {
            this.documentSrv = documentSrv;
        }

        public override IEnumerable<ValidationResult> Validate(ChangeDocStateCommand command)
        {
            if(command.DocumentId <= 0)
            {
                AddValidationResult("DocumentId", "Invalid document id");
                return results;
            }

            var document = documentSrv.GetDocumentById(command.DocumentId);
            if(document == null)
            {
                AddValidationResult("DocumentId", $"Document (id: {command.DocumentId}) not found");
                return results;
            }

            if(!IsAllowedCombination(document.State, command.NewState))
                AddValidationResult("NewState", 
                    $"Can't change state from {document.State} to {command.NewState}");

            return results;
        }

        protected bool IsAllowedCombination(DocumentState from, DocumentState to)
        {
            var allowed = new string[]
            {
                $"{DocumentState.Open}|{DocumentState.Confirmed}",
                $"{from}|{from}"
            };

            var combination = $"{from}|{to}";
        
            return allowed.Contains(combination);
        }
    }
}