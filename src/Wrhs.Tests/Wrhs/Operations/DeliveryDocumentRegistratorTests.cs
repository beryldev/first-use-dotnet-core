using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    public class DeliveryDocumentRegistratorTests : DocumentRegistratorTests<DeliveryDocument>
    {
        protected override string GetDocumentPrefix()
        {
            return "D";
        }

        protected override DeliveryDocument CreateDocument()
        {
            return new DeliveryDocument();
        }
    }
}