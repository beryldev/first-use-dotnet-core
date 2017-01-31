using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Data.Persist;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Persist
{
    public class DocumentPersistTests : TestsBase, IDisposable
    {
        private readonly DocumentPersist documentPersist;

        private readonly Mock<IDocumentNumerator> docNumeratorMock;

        public DocumentPersistTests() : base()
        {
            docNumeratorMock = new Mock<IDocumentNumerator>();
            docNumeratorMock.Setup(m=>m.AssignNumber(It.IsNotNull<Document>()))
                .Returns((Document doc)=>{ 
                    doc.FullNumber = "some-number";
                    return doc;
                 });
            documentPersist = new DocumentPersist(context, docNumeratorMock.Object);
        }

        [Fact]
        public void ShouldStoreDocumentWithLinesInContextOnSave()
        {
            context.Products.Add(new Product());
            context.Products.Add(new Product());
            context.SaveChanges();
            var document = new Document
            {
                Type = DocumentType.Delivery,
                State = DocumentState.Confirmed,
                Lines = new List<DocumentLine>
                {
                    new DocumentLine { ProductId = 1, Quantity = 10},
                    new DocumentLine { ProductId = 1, Quantity = 20}
                }
            };

            documentPersist.Save(document);

            context.Documents.Should().HaveCount(1);
            context.DocumentLines.Should().HaveCount(2);
            context.Documents.First().Type.Should()
                .Be(DocumentType.Delivery);
            context.Documents.First().State.Should()
                .Be(DocumentState.Confirmed);
        }

        [Fact]
        public void ShouldAssignNumberToDocumentOnSave()
        {
            var document = new Document { Type = DocumentType.Delivery};

            documentPersist.Save(document);

            var saved = context.Documents.First();
            saved.FullNumber.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldUpdateDataInContextOnUpdate()
        {
            context.Products.Add(new Product());
            context.Products.Add(new Product());
            context.SaveChanges();
            var document = new Document
            {
                Type = DocumentType.Delivery,
                State = DocumentState.Confirmed,
                Lines = new List<DocumentLine>
                {
                    new DocumentLine { ProductId = 1, Quantity = 10},
                    new DocumentLine { ProductId = 1, Quantity = 20}
                }
            };
            context.Documents.Add(document);
            context.SaveChanges();

            document.State = DocumentState.Executed;
            document.Lines.First().Quantity = 99;
            documentPersist.Update(document);

            var upDoc = context.Documents.First();
            var upLine = context.DocumentLines.First();
            upDoc.Type.Should().Be(DocumentType.Delivery);
            upDoc.State.Should().Be(DocumentState.Executed);
            upLine.ProductId.Should().Be(1);
            upLine.Quantity.Should().Be(99);
        }

        [Fact]
        public void ShouldRemoveEntityFromContextOnDelete()
        {
            context.Products.Add(new Product());
            context.SaveChanges();
            var document1 = new Document
            {
                Type = DocumentType.Delivery,
                State = DocumentState.Confirmed,
                Lines = new List<DocumentLine>
                {
                    new DocumentLine { ProductId = 1, Quantity = 10}
                }
            };
            var document2 = new Document
            {
                Type = DocumentType.Delivery,
                State = DocumentState.Confirmed,
                Lines = new List<DocumentLine>
                {
                    new DocumentLine { ProductId = 1, Quantity = 10}
                }
            };
            context.Documents.Add(document1);
            context.Documents.Add(document2);
            context.SaveChanges();

            documentPersist.Delete(document1);

            context.Documents.Should().HaveCount(1);
            context.DocumentLines.Should().HaveCount(1);
        }

    }
}