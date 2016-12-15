using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;

namespace Wrhs.Tests.Delivery
{
    public class BeginDeliveryOperationCmdValidatorTests
        : BeginOperationCmdValidatorTestsBase<BeginDeliveryOperationCommand>
    {
        protected override BeginDeliveryOperationCommand CreateCommand()
        {
            return new BeginDeliveryOperationCommand
            {
                DocumentId = 1,
                OperationGuid = "some-guid"
            };
        }

        protected override IValidator<BeginDeliveryOperationCommand> CreateValidator(IDocumentService documentSrv, IOperationService operationSrv)
        {
            return new BeginDeliveryOperationCommandValidator(documentSrv, operationSrv);
        }

        protected override DocumentType GetInvalidDocumentType()
        {
            return DocumentType.Relocation;
        }

        protected override DocumentType GetValidDocumentType()
        {
            return DocumentType.Delivery;
        }
    }
}