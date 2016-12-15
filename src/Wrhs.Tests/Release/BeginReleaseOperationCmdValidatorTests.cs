using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;

namespace Wrhs.Tests.Release
{
    public class BeginReleaseOperationCmdValidatorTests
        : BeginOperationCmdValidatorTestsBase<BeginReleaseOperationCommand>
    {
        protected override BeginReleaseOperationCommand CreateCommand()
        {
            return new BeginReleaseOperationCommand { OperationGuid = "some-guid" };
        }

        protected override IValidator<BeginReleaseOperationCommand> CreateValidator(IDocumentService documentSrv,
            IOperationService operationSrv)
        {
            return new BeginReleaseOperationCommandValidator(documentSrv, operationSrv);
        }

        protected override DocumentType GetInvalidDocumentType()
        {
            return DocumentType.Delivery;
        }

        protected override DocumentType GetValidDocumentType()
        {
            return DocumentType.Release;
        }
    }
}