using Wrhs.Common;

namespace Wrhs.Release
{
    public class BeginReleaseOperationCommandValidator
        : BeginOperationCommandValidator<BeginReleaseOperationCommand>
    {
        public BeginReleaseOperationCommandValidator(IDocumentService documentSrv,
            IOperationService operationSrv) : base(documentSrv, operationSrv)
        {
        }

        protected override DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Release;
        }
    }
}