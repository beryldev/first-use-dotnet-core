using System.Linq;
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

        [Test]
        public void BuilderCanBeBasedOnPassedDeliveryDocument()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<DocAddLineCmd>>();
            var document = new DeliveryDocument();
            document.Lines.Add(new DeliveryDocumentLine
            {
                Product = repo.GetById(1),
                Quantity = 10,
                Remarks = "some line remarks"
            });
            
            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object, document);

            var lines = builder.Lines;
            Assert.AreEqual(1, lines.Count());
            var line = builder.Lines.First();
            Assert.AreEqual(1, line.Product.Id);
            Assert.AreEqual(10, line.Quantity);
            Assert.AreEqual("some line remarks", line.Remarks);
        }

        [Test]
        public void ShouldRemovePassedLineOnDelete()
        {
            var repo = MakeProductRepository();
            var document = new DeliveryDocument();
            var addLineValidMock = new Mock<IValidator<DocAddLineCmd>>();
            document.Lines.Add(new DeliveryDocumentLine
            {
                Product = repo.GetById(1),
                Quantity = 10,
                Remarks = "some line remarks"
            });
            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object, document);

            var line = new DeliveryDocumentLine
            {
                Id = 1,
                Product = repo.GetById(1),
                Quantity = 10,
                Remarks = "some line remarks"
            };
            builder.RemoveLine(line);

            Assert.IsEmpty(builder.Build().Lines);
            Assert.IsEmpty(builder.Lines);
        }

        public DeliveryDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<DocAddLineCmd>>();

            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object);
            return builder;
        }
    }
}