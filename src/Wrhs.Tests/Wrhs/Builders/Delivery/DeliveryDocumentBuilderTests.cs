using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DeliveryDocumentBuilderTests : DocumentBuilderTestsBase
    {
        [Test]
        public void OnBuildReturnsDeliveryDocument()
        {
            var builder = MakeBuilder();

            var document = builder.Build();

            Assert.IsInstanceOf<DeliveryDocument>(document);
        }

        public DeliveryDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<IDocAddLineCmd>>();

            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object);
            return builder;
        }
    }
}