using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Operations.Relocation;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationDocumentBuilderTests : DocumentBuilderTestsBase
    {
        [Test]
        public void OnBuildReturnRelocationDocument()
        {
            var repo = MakeProductRepository();
            var addLineValidator = new RelocationDocumentAddLineCommandValidator(repo);
            var builder = new RelocationDocumentBuilder(repo, addLineValidator);

            var document = builder.Build();

            Assert.IsInstanceOf<RelocationDocument>(document);
        }

        [Test]
        public void AfterAddLineBuildReturnDocumentWithAddedLine()
        {
            var builder = MakeBuilder();
            var command = new RelocationDocumentBuilderAddLineCommand { ProductId = 1, Quantity = 5 };
       
            builder.AddLine(command);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(5, document.Lines[0].Quantity);
        }

        RelocationDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<RelocationDocumentBuilderAddLineCommand>>();

            var builder = new RelocationDocumentBuilder(repo, addLineValidMock.Object);
            return builder;
        }
    }
}