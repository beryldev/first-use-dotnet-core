using Wrhs.Common;

namespace Wrhs.Relocation
{
    public class BeginRelocationOperationCommandValidator
        : BeginOperationCommandValidator<BeginRelocationOperationCommand>
    {
        public BeginRelocationOperationCommandValidator(IDocumentService documentSrv,
            IOperationService operationSrv) : base(documentSrv, operationSrv)
        {
        }

        protected override DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Relocation;
        }
    }
}