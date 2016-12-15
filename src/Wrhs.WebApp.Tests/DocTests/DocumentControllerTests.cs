using System;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.WebApp.Controllers.Documents;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public abstract class DocumentControllerTests : IDisposable
    {
        protected Mock<IDocumentService> documentSrv;

        protected DocumentController controller;

        public DocumentControllerTests()
        {
            documentSrv = CreateDocService();
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
        }

        // [Fact]
        // public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        // {
        //     repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

        //     var result = controller.Get(fullNumber: "D/001/2016");

        //     Assert.IsType<PaginateResult<TDoc>>(result);
        // }

        // [Fact]
        // public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        // {
        //     repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

        //     var result = controller.Get(issueDate: DateTime.Today);

        //     Assert.IsType<PaginateResult<TDoc>>(result);
        // }

        // [Fact]
        // public void ShouldReturnRequestedPageOnGet()
        // {
        //     repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

        //     var result = controller.Get(page: 2);

        //     Assert.Equal(2, result.Page);
        // }

        // [Fact]
        // public void ShouldReturnRequestedPageSize()
        // {
        //     repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

        //     var result = controller.Get(perPage: 30);

        //     Assert.Equal(30, result.PerPage);
        // }
    }
}