using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.WebApp.Controllers.Documents;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class DocumentReadControllerTests : IDisposable
    {
        protected Mock<IDocumentService> documentSrvMock;

        protected DocumentReadController controller;

        public DocumentReadControllerTests()
        {
            documentSrvMock = CreateDocService();
            controller = new DocumentReadController(documentSrvMock.Object);
            SetupDocumentService();
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        protected virtual Mock<IDocumentService> CreateDocService()
        {
            return new Mock<IDocumentService>();
        }

        protected void SetupDocumentService()
        {
            var result = new ResultPage<Document>(
                    new List<Document>
                    {
                        new Document(),
                        new Document(),
                        new Document()
                    }, 1, 20 );

            documentSrvMock.Setup(m=>m.GetDocuments(It.IsNotNull<DocumentType>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(It.IsNotNull<Dictionary<string, object>>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(It.IsNotNull<Dictionary<string, object>>(),
                It.IsNotNull<int>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(It.IsNotNull<Dictionary<string, object>>(),
                It.IsNotNull<int>(), It.IsNotNull<int>()))
                .Returns(result);
        }

        [Theory]
        [InlineData(null, null, null, null)]
        [InlineData(DocumentType.Delivery, null, null, null)]
        [InlineData(DocumentType.Release, null, DocumentState.Canceled, "")]
        [InlineData(DocumentType.Relocation, null, null, "some-number")]
        public void ShouldReturnOkWithResultOnGetDocuments(DocumentType? type, DateTime? issueDate, DocumentState? state, string fullNumber)
        {
            var filter = new DocumentFilter
            {
                Type = type,
                State = state,
                IssueDate = issueDate,
                FullNumber = fullNumber
            };


            var result = controller.GetDocuments(filter);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeOfType<ResultPage<Document>>();
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, null, null)]
        [InlineData(null, DocumentState.Canceled, "")]
        [InlineData(null, null, "some-number")]
        public void ShouldReturnOkWithResultOnGetDeliveryDocuments(DateTime? issueDate, DocumentState? state, string fullNumber)
        {
            var filter = new DocumentFilter
            {
                State = state,
                IssueDate = issueDate,
                FullNumber = fullNumber
            };

            var result = controller.GetDeliveryDocuments(filter);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeOfType<ResultPage<Document>>();
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, null, null)]
        [InlineData(null, DocumentState.Canceled, "")]
        [InlineData(null, null, "some-number")]
        public void ShouldReturnOkWithResultOnGetRelocationDocuments(DateTime? issueDate, DocumentState? state, string fullNumber)
        {
            var filter = new DocumentFilter
            {
                State = state,
                IssueDate = issueDate,
                FullNumber = fullNumber
            };

            var result = controller.GetRelocationDocuments(filter);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeOfType<ResultPage<Document>>();
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, null, null)]
        [InlineData(null, DocumentState.Canceled, "")]
        [InlineData(null, null, "some-number")]
        public void ShouldReturnOkWithResultOnGetReleaseDocuments(DateTime? issueDate, DocumentState? state, string fullNumber)
        {
            var filter = new DocumentFilter
            {
                State = state,
                IssueDate = issueDate,
                FullNumber = fullNumber
            };

            var result = controller.GetReleaseDocuments(filter);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().BeOfType<ResultPage<Document>>();
        }

        [Fact]
        public void ShouldReturnOkWithDocumentOnGetDocumentWhenFound()
        {
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(new Document());

            var result = controller.GetDocument(1);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().NotBeNull();
            (result as OkObjectResult).Value.Should().BeOfType<Document>();
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetDocumentWhenNotFound()
        {
            documentSrvMock.Setup(m=>m.GetDocumentById(It.IsNotNull<int>()))
                .Returns(null as Document);

            var result = controller.GetDocument(1);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}