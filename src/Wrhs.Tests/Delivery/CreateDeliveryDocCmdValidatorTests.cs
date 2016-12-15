using Wrhs.Core;
using Wrhs.Delivery;
using Wrhs.Services;

namespace Wrhs.Tests.Delivery
{
    public class CreateDeliveryDocCmdValidatorTests
     : CreateDocumentCommandValidatorTestsBase<CreateDeliveryDocumentCommand>
    {
        protected override CreateDeliveryDocumentCommand CreateCommand()
        {
            return new CreateDeliveryDocumentCommand();
        }

        protected override IValidator<CreateDeliveryDocumentCommand> CreateValidator(IProductService productSrv)
        {
            return new CreateDeliveryDocumentCommandValidator(productSrv);
        }
    }
}