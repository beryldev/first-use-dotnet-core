using Wrhs.Operations.Delivery;
using Wrhs.WebApp.Controllers.Documents;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryDocumentControllerTests
        : DocumentControllerTests<DeliveryDocument, DeliveryDocumentLine>
    {
        protected override DocumentController<DeliveryDocument, DeliveryDocumentLine> CreateDocController()
        {
            var controller = new DeliveryDocController(repository.Object);
            return controller as DocumentController<DeliveryDocument, DeliveryDocumentLine>;
        }
    }
}