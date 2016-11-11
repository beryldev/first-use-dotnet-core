using System;
using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Operations.Delivery;
using Wrhs.WebApp.Controllers.Documents;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryDocumentControllerTests : IDisposable
    {
        Mock<IRepository<DeliveryDocument>> repository;

        DeliveryDocumentController controller;

        public DeliveryDocumentControllerTests()
        {
            repository = new Mock<IRepository<DeliveryDocument>>();
            controller = new DeliveryDocumentController(repository.Object);
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithoutParameters()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());

            var result = controller.Get();

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());

            var result = controller.Get(fullNumber: "D/001/2016");

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());

            var result = controller.Get(issueDate: DateTime.Today);

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnRequestedPageOnGet()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());

            var result = controller.Get(page: 2);

            Assert.Equal(2, result.Page);
        }

        [Fact]
        public void ShouldReturnRequestedPageSize()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());

            var result = controller.Get(perPage: 30);

            Assert.Equal(30, result.PerPage);
        }
    }
}