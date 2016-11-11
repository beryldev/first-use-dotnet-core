using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Wrhs.WebApp.Controllers;
using Wrhs.WebApp.Utils;
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
    }
}