using Wrhs.Common;

namespace Wrhs.Delivery
{
    public class BeginDeliveryOperationCommandValidator
        : BeginOperationCommandValidator<BeginDeliveryOperationCommand>
    {
        public BeginDeliveryOperationCommandValidator(IDocumentService documentSrv, 
            IOperationService operationSrv) : base(documentSrv, operationSrv)
        {
        }

        protected override DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Delivery;
        }
    }
}