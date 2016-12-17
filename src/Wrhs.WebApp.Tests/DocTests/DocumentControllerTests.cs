using System;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.WebApp.Controllers.Documents;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public abstract class DocumentControllerTests : IDisposable
    {
        protected Mock<IDocumentService> documentSrvMock;

        protected DocumentController controller;

        public DocumentControllerTests()
        {
            documentSrvMock = CreateDocService();
            controller = CreateDocController();
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        protected virtual Mock<IDocumentService> CreateDocService()
        {
            return new Mock<IDocumentService>();
        }

        protected abstract DocumentController CreateDocController();

        [Fact]
        public void ShouldReturnDocumentsOnGetWithoutParameters()
        {
            var result = controller.Get();

            Assert.IsType<ResultPage<Document>>(result);
            result.Items.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        {
            var result = controller.Get(fullNumber: "some-number");

            Assert.IsType<ResultPage<Document>>(result);
            result.Items.Should().NotBeEmpty();
        }

        // [Fact]
        // public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        // {
        //     repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

        //     var result = controller.Get(issueDate: DateTime.Today);

        //     Assert.IsType<PaginateResult<TDoc>>(result);
        // }

        [Fact]
        public void ShouldReturnRequestedPageOnGet()
        {
            var result = controller.Get(page: 2);

            Assert.IsType<ResultPage<Document>>(result);
            result.Items.Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldReturnRequestedPageSize()
        {
            var result = controller.Get(pageSize: 30);

            Assert.IsType<ResultPage<Document>>(result);
            result.Items.Should().NotBeEmpty();
        }
    }
}