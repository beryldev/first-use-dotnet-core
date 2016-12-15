using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;

namespace Wrhs.Tests.Relocation
{
    public class BeginRelocationCmdValidatorTests
        : BeginOperationCmdValidatorTestsBase<BeginRelocationOperationCommand>
    {
        protected override BeginRelocationOperationCommand CreateCommand()
        {
            return new BeginRelocationOperationCommand
            {
                DocumentId = 1,
                OperationGuid = "some-guid"
            };
        }

        protected override IValidator<BeginRelocationOperationCommand> CreateValidator(IDocumentService documentSrv, IOperationService operationSrv)
        {
            return new BeginRelocationOperationCommandValidator(documentSrv, operationSrv);
        }

        protected override DocumentType GetInvalidDocumentType()
        {
            return DocumentType.Delivery;
        }

        protected override DocumentType GetValidDocumentType()
        {
            return DocumentType.Relocation;
        }
    }
}