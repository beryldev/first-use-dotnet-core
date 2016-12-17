using System;
using System.Collections.Generic;
using System.Reflection;
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

            documentSrvMock.Setup(m=>m.FilterDocuments(It.IsNotNull<DocumentType>(), It.IsNotNull<Dictionary<string, object>>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(It.IsNotNull<DocumentType>(), It.IsNotNull<Dictionary<string, object>>(),
                It.IsNotNull<int>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(It.IsNotNull<DocumentType>(), It.IsNotNull<Dictionary<string, object>>(),
                It.IsNotNull<int>(), It.IsNotNull<int>()))
                .Returns(result);
        }

        [Theory]
        [InlineData("GetDeliveryDocuments")]
        [InlineData("GetRelocationDocuments")]
        [InlineData("GetReleaseDocuments")]
        public void ShouldReturnOkWithDocumentsOnGetWithoutParameters(string variant)
        {
            var controllerType = controller.GetType();
            var methodUnderTest  = controllerType.GetMethod(variant);

            var parameters = new object[]{null, null, null, null};
            var result = methodUnderTest.Invoke(controller, parameters) as OkObjectResult;

            Assert.IsType<ResultPage<Document>>(result.Value);
            (result.Value as ResultPage<Document>).Items.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("GetDeliveryDocuments")]
        [InlineData("GetRelocationDocuments")]
        [InlineData("GetReleaseDocuments")]
        public void ShouldReturnOkWithDocumentsOnGetWithFullNumberParameter(string variant)
        {
            var controllerType = controller.GetType();
            var methodUnderTest  = controllerType.GetMethod(variant);

            var parameters = new object[]{null, "some-number", null, null};
            var result = methodUnderTest.Invoke(controller, parameters) as OkObjectResult;

            Assert.IsType<ResultPage<Document>>(result.Value);
            (result.Value as ResultPage<Document>).Items.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("GetDeliveryDocuments")]
        [InlineData("GetRelocationDocuments")]
        [InlineData("GetReleaseDocuments")]
        public void ShouldReturnOkWithDocumentsOnGetWithIssueDateParameter(string variant)
        {
            var controllerType = controller.GetType();
            var methodUnderTest  = controllerType.GetMethod(variant);

            var parameters = new object[]{new DateTime(2016,1,1), null, null, null};
            var result = methodUnderTest.Invoke(controller, parameters) as OkObjectResult;

            Assert.IsType<ResultPage<Document>>(result.Value);
            (result.Value as ResultPage<Document>).Items.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("GetDeliveryDocuments")]
        [InlineData("GetRelocationDocuments")]
        [InlineData("GetReleaseDocuments")]
        public void ShouldReturnOkWithRequestedPageOnGet(string variant)
        {
            var controllerType = controller.GetType();
            var methodUnderTest  = controllerType.GetMethod(variant);

            var parameters = new object[]{null, null, 2, null};
            var result = methodUnderTest.Invoke(controller, parameters) as OkObjectResult;

            Assert.IsType<ResultPage<Document>>(result.Value);
            (result.Value as ResultPage<Document>).Items.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("GetDeliveryDocuments")]
        [InlineData("GetRelocationDocuments")]
        [InlineData("GetReleaseDocuments")]
        public void ShouldReturnOkWithRequestedPageSize(string variant)
        {
            var controllerType = controller.GetType();
            var methodUnderTest  = controllerType.GetMethod(variant);

            var parameters = new object[]{null, null, null, 30};
            var result = methodUnderTest.Invoke(controller, parameters) as OkObjectResult;

            Assert.IsType<ResultPage<Document>>(result.Value);
            (result.Value as ResultPage<Document>).Items.Should().NotBeEmpty();
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