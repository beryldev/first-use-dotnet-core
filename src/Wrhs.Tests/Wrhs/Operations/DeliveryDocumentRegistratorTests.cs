using System.Linq;
using NUnit.Framework;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    [TestFixture]
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