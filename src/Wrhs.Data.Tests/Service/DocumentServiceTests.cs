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
        public DocumentServiceTests() : base()
        {
            context.Products.Add(new Product());
            context.SaveChanges();
        }

        [Fact]
        public void ShouldReturnTrueOnCheckByIdWhenExists()
        {
            context.Documents.Add(new Document());
            context.SaveChanges();

            var result = (service as DocumentService).CheckDocumentExistsById(1);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseOnCheckByIdWhenDontExists()
        {
            context.Documents.Add(new Document());
            context.SaveChanges();

            var result = (service as DocumentService).CheckDocumentExistsById(900);

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnDocumentWithLinesOnGetById()
        {
            var result = (service as DocumentService).GetDocumentById(1);
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
            context.SaveChanges();

            var result = (service as DocumentService).Get(DocumentType.Release);

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

            var result = (service as DocumentService).Get(DocumentType.Release, page);

            result.Items.Should().HaveCount(3);
        }

        [Theory]
        [InlineData(1, 5, 3)]
        [InlineData(2, 2, 1)]
        public void ShouldReturnDocumentsByTypePageAndPageSize(int page, int pageSize, int expected)
        {
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.Documents.Add(new Document{ Type = DocumentType.Release});
            context.SaveChanges();

            var result = (service as DocumentService).Get(DocumentType.Release, page, pageSize);

            result.Items.Should().HaveCount(expected);
        }

        protected override BaseService<Document> CreateService(WrhsContext context)
        {
            context.Products.Add(new Product());
            return new DocumentService(context);
        }

        protected override Document CreateEntity(int i)
        {
            return new Document
            {
                Type = DocumentType.Delivery,
                Lines = new List<DocumentLine>
                {
                    new DocumentLine{ProductId=1},
                    new DocumentLine{ProductId=1}
                }
            };
        }
    }
}