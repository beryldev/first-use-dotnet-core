using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public abstract class DocumentBuilderTests : DocumentBuilderTestsBase
     {
        [Test]
        public void AfterAddLineBuildReturnDocumentWithAddedLine()
        {
            var builder = MakeBuilder();
            var command = new DocAddLineCmd { ProductId = 1, Quantity = 5 };
       
            builder.AddLine(command);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(5, document.Lines[0].Quantity);
        }

        [Test]
        public void AfterManyAddLineBuildReturnDocumentWithAddedLines()
        {
            var builder = MakeBuilder();
            
            var command = new DocAddLineCmd { ProductId = 1, Quantity = 5 };
            builder.AddLine(command);
            command = new DocAddLineCmd { ProductId = 3, Quantity = 2 };
            builder.AddLine(command);
            command = new DocAddLineCmd{ ProductId = 8, Quantity = 9 };
            builder.AddLine(command);

            var document = builder.Build();

            Assert.AreEqual(3, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(5, document.Lines[0].Quantity);
            Assert.AreEqual(3, document.Lines[1].Product.Id);
            Assert.AreEqual(2, document.Lines[1].Quantity);
            Assert.AreEqual(8, document.Lines[2].Product.Id);
            Assert.AreEqual(9, document.Lines[2].Quantity);
        }

        [Test]
        public void AfterRemoveLineBuildReturnDocWithoutRemovedLine()
        {
            var builder = MakeBuilder();
            
            var command = new DocAddLineCmd { ProductId = 1, Quantity = 5 };
            builder.AddLine(command);
            
            var lines = builder.Lines;
            builder.RemoveLine(lines.First());

            var document = builder.Build();

            CollectionAssert.IsEmpty(document.Lines);
        }

        [Test]
        public void WhenExistsMoreThanOneLineAfterRemoveLineBuildReturnDocWithoutOnlyRemovedLine()
        {
            var builder = MakeBuilder();
            
            var command = new DocAddLineCmd { ProductId = 1, Quantity = 5};
            builder.AddLine(command);
            command = new DocAddLineCmd { ProductId = 3, Quantity = 2 };
            builder.AddLine(command);
            command = new DocAddLineCmd { ProductId = 8, Quantity = 9 };
            builder.AddLine(command);

            var lineToRemove = ((DocumentLine[])builder.Lines)[1];
            builder.RemoveLine(lineToRemove);

            var document = builder.Build();

            Assert.AreEqual(2, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(8, document.Lines[1].Product.Id);
        }

        [Test]
        public void AfterUpdateLineBuildReturnDocumentWithUpdatedLine()
        {
            var builder = MakeBuilder();

            var command = new DocAddLineCmd { ProductId = 1, Quantity = 5 };
            builder.AddLine(command);

            var line = builder.Lines.First();
            line.Quantity = 20;

            builder.UpdateLine(line);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(20, document.Lines.First().Quantity);
        }

        [Test]
        public void WhenOnAddLineValidationFailBuildReturnDocWithUnchangedLines()
        {
            var repo = RepositoryFactory<Product>.Make();

            var addLineValidMock = new Mock<IValidator<DocAddLineCmd>>();
            addLineValidMock.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>()))
                .Returns(new ValidationResult[]{ new ValidationResult() });

            var builder = new DocumentBuilderClassInTest(repo, addLineValidMock.Object);

            var command = new DocAddLineCmd { ProductId = -34, Quantity = 5 };
            builder.AddLine(command);

            var document = builder.Build();

            CollectionAssert.IsEmpty(document.Lines);
        }

        [Test]
        public void WhenOnAddLineValidationFailCallOnAddLineFail()
        {
            var onAddLineFailCalled = false;
            var repo = RepositoryFactory<Product>.Make();

            var addLineValidMock = new Mock<IValidator<DocAddLineCmd>>();
            addLineValidMock.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>()))
                .Returns(new ValidationResult[]{ new ValidationResult() });

            var builder = new DocumentBuilderClassInTest(repo, addLineValidMock.Object);
            builder.OnAddLineFail += (object sender, IEnumerable<ValidationResult> args) => onAddLineFailCalled=true;

            var command = new DocAddLineCmd { ProductId = -34, Quantity = 5 };
            builder.AddLine(command);

            Assert.IsTrue(onAddLineFailCalled);
        }

        [Test]
        public void WhenAddLineEachLineHasUniqeId()
        {
            var builder = MakeBuilder();

            var command1 = new DocAddLineCmd { ProductId = 1, Quantity = 1 };
            var command2 = new DocAddLineCmd { ProductId = 2, Quantity = 2 };
            builder.AddLine(command1);
            builder.AddLine(command2);
            var line = ((DocumentLine[])builder.Lines)[0];
            builder.RemoveLine(line);
            builder.AddLine(command1);

            Assert.AreEqual(2, builder.Lines.GroupBy(l=>l.Id).Select(l=>l).Count());
        }

        DocumentBuilderClassInTest MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<DocAddLineCmd>>();

            var builder = new DocumentBuilderClassInTest(repo, addLineValidMock.Object);
            return builder;
        }      
    }

    class DocumentBuilderClassInTest : DocumentBuilder<Document, DocumentLine, DocAddLineCmd>
    {
        IRepository<Product> productRepository;

        public DocumentBuilderClassInTest(IRepository<Product> productRepository, IValidator<DocAddLineCmd> addLineValidator) 
            : base(addLineValidator)
        {
            this.productRepository = productRepository;
        }

        public override Document Build()
        {
            var doc = new Document();
            doc.Lines.AddRange(lines);

            return (Document)doc;
        }

        protected override DocumentLine CommandToDocumentLine(DocAddLineCmd command)
        {
            return new DocumentLine
            {
                Product = productRepository.GetById(command.ProductId),
                Quantity = command.Quantity
            };
        }

        protected override DocAddLineCmd DocumentLineToAddLineCommand(DocumentLine line)
        {
            return new DocAddLineCmd
            {
                ProductId = line.Product.Id,
                Quantity = line.Quantity
            };
        }
    }
}