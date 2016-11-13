using System.Linq;
using Moq;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Xunit;

namespace Wrhs.Tests
{
    public class DeliveryDocumentBuilderTests : DocumentBuilderTestsBase
    {
        [Fact]
        public void OnBuildReturnsDeliveryDocument()
        {
            var builder = MakeBuilder();

            var document = builder.Build();

            Assert.IsType<DeliveryDocument>(document);
        }

        [Fact]
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
            Assert.Equal(1, lines.Count());
            var line = builder.Lines.First();
            Assert.Equal(1, line.Product.Id);
            Assert.Equal(10, line.Quantity);
            Assert.Equal("some line remarks", line.Remarks);
        }

        [Fact]
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

            Assert.Empty(builder.Build().Lines);
            Assert.Empty(builder.Lines);
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