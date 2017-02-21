using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public class DocumentServiceTests : ServiceTestsBase<Document>
    {
        private readonly DocumentService service;
        private readonly Mock<IDocumentNumerator> docNumeratorMock;

        public DocumentServiceTests() : base()
        {       
            docNumeratorMock = new Mock<IDocumentNumerator>();
            docNumeratorMock.Setup(m=>m.AssignNumber(It.IsNotNull<Document>()))
                .Returns((Document doc)=>{ 
                    doc.FullNumber = "some-number";
                    return doc;
                 });

            context.Products.Add(new Product());
            context.SaveChanges();

            service = new DocumentService(context, docNumeratorMock.Object);
        }

        protected override BaseService<Document> GetService()
        {
            return service as BaseService<Document>;
        }

        [Fact]
        public void ShouldReturnTrueOnCheckByIdWhenExists()
        {
            context.Documents.Add(new Document());
            context.SaveChanges();

            var result = service.CheckDocumentExistsById(1);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseOnCheckByIdWhenDontExists()
        {
            context.Documents.Add(new Document());
            context.SaveChanges();

            var result = service.CheckDocumentExistsById(900);

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnDocumentWithLinesOnGetById()
        {
            var result = service.GetDocumentById(1);
            result.Should().NotBeNull();
            result.Type.Should().Be(DocumentType.Delivery);
            result.Lines.Should().HaveCount(2);
        }

        [Fact]
        public void ShouldReturnDocumentsByType()
        {
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery});
            context.SaveChanges();

            var result = service.GetDocuments(DocumentType.Release);

            result.Items.Should().HaveCount(3);
        }

        [Fact]
        public void ShouldReturnDocumentsByTypeAndPage()
        {
            var page = 1;
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.SaveChanges();

            var result = service.GetDocuments(DocumentType.Release, page);

            result.Items.Should().HaveCount(3);
            result.Page.Should().Be(1);
        }

        [Theory]
        [InlineData(1, 5, 3)]
        [InlineData(2, 2, 1)]
        public void ShouldReturnDocumentsByTypePageAndPageSize(int page, int pageSize, int expected)
        {
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery});
            context.SaveChanges();

            var result = service.GetDocuments(DocumentType.Release, page, pageSize);

            result.Items.Should().HaveCount(expected);
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(1, 2, 2)]
        public void ShouldReturnRequestedPageAndPageSizeOnFilterDocuments(int page, int pageSize, int expected)
        {
            context.Documents.Add(new Document{ Type = DocumentType.Release, FullNumber="RLS-1", IssueDate=new DateTime(2016, 1, 2)});
            context.Documents.Add(new Document{ Type = DocumentType.Release, FullNumber="RLS-2", IssueDate=new DateTime(2016, 1, 2)});
            context.Documents.Add(new Document{ Type = DocumentType.Release, FullNumber="RLS-3", IssueDate=new DateTime(2016, 12, 1)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-1", IssueDate=new DateTime(2016, 12, 1)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-12",IssueDate=new DateTime(2016, 12, 1)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-22", IssueDate=new DateTime(2016, 1, 2)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-15", IssueDate=new DateTime(2016, 1, 2)});
            context.SaveChanges();

            var filter = new Dictionary<string, object>();
            filter.Add("FullNumber", "DLV-1");
            filter.Add("IssueDate", new DateTime(2016, 12, 1));
            filter.Add("Type", DocumentType.Delivery);

            var result = service.FilterDocuments(filter, page, pageSize);

            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().HaveCount(expected);
        }

        [Theory]
        [InlineData(DocumentType.Release, "State", DocumentState.Confirmed, 1)]
        [InlineData(DocumentType.Release, "State", DocumentState.Canceled, 1)]
        [InlineData(DocumentType.Release, "FullNumber", null, 3)]
        public void ShouldReturnFilteredResultsOnFilterDocuments(DocumentType type, string field, object value, int expectedCount)
        {
            context.Documents.Add(new Document{ Type = DocumentType.Release, State = DocumentState.Canceled, FullNumber="RLS-1", IssueDate=new DateTime(2016, 1, 2)});
            context.Documents.Add(new Document{ Type = DocumentType.Release, State = DocumentState.Open, FullNumber="RLS-2", IssueDate=new DateTime(2016, 1, 2)});
            context.Documents.Add(new Document{ Type = DocumentType.Release, State = DocumentState.Confirmed, FullNumber="RLS-3", IssueDate=new DateTime(2016, 12, 1)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-1", IssueDate=new DateTime(2016, 12, 1)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-12",IssueDate=new DateTime(2016, 12, 1)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-22", IssueDate=new DateTime(2016, 1, 2)});
            context.Documents.Add(new Document{ Type = DocumentType.Delivery, FullNumber="DLV-15", IssueDate=new DateTime(2016, 1, 2)});
            context.SaveChanges();

            var filter = new Dictionary<string, object> 
            { 
                { field, value },
                { "type", type}
            };
            var result = service.FilterDocuments(filter);

            result.Items.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void ShouldStoreDocumentWithLinesInContextOnSave()
        {
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

            service.Save(document);

            context.Documents.Should().HaveCount(5);
            context.DocumentLines.Should().HaveCount(10);
            context.Documents.First().Type.Should()
                .Be(DocumentType.Delivery);
            context.Documents.Last().State.Should()
                .Be(DocumentState.Confirmed);
        }

        [Fact]
        public void ShouldAssignNumberToDocumentOnSave()
        {
            var document = new Document { Type = DocumentType.Delivery};

            service.Save(document);

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
            document.Lines.Last().Quantity = 99;
            service.Update(document);

            var upDoc = context.Documents.Last();
            var upLine = context.DocumentLines.Last();
            upDoc.Type.Should().Be(DocumentType.Delivery);
            upDoc.State.Should().Be(DocumentState.Executed);
            upLine.ProductId.Should().Be(1);
            upLine.Quantity.Should().Be(99);
        }

        [Fact]
        public void ShouldRemoveEntityFromContextOnDelete()
        {
            var doc = context.Documents.First();
            
            service.Delete(doc);

            context.Documents.Should().HaveCount(3);
            context.DocumentLines.Should().HaveCount(6);
        }

        protected override Document CreateEntity(int i)
        {
            context.Products.Add(new Product());
            context.SaveChanges();
            
            return new Document
            {
                Type = DocumentType.Delivery,
                FullNumber = "some-number",
                Lines = new List<DocumentLine>
                {
                    new DocumentLine{ProductId=1},
                    new DocumentLine{ProductId=1}
                }
            };
        }
    }
}