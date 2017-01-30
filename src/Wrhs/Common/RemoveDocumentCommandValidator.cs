using System;
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
            throw new NotImplementedException();
        }
    }
}