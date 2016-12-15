using Wrhs.WebApp.Controllers.Documents;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryDocumentControllerTests : DocumentControllerTests
    {
        protected override DocumentController CreateDocController()
        {
            return new DeliveryDocController(documentSrv.Object);
        }
    }
}