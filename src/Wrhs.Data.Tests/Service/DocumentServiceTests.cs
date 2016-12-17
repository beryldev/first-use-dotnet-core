using System;
using System.Collections.Generic;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public class DocumentServiceTests : ServiceTestsBase<Document>
    {
        private readonly DocumentService service;

        public DocumentServiceTests() : base()
        {       
            context.Products.Add(new Product());
            context.SaveChanges();

            service = new DocumentService(context);
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

            var result = service.FilterDocuments(DocumentType.Delivery, filter, page, pageSize);

            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.Items.Should().HaveCount(expected);
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