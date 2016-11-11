using System;
using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Documents;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Wrhs.WebApp.Controllers.Documents;
using Wrhs.WebApp.Utils;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class RelocationDocumentControllerTests
    {
        readonly Mock<IRepository<RelocationDocument>> repository;

        Mock<ICache> cache;

        Mock<IRepository<Product>> prodRepository;

        Mock<IValidator<DocAddLineCmd>> validator;

        Mock<IDocumentRegistrator<RelocationDocument>> registrator;

        readonly RelocationDocumentController controller;

        public RelocationDocumentControllerTests()
        {
            repository = new Mock<IRepository<RelocationDocument>>();
            cache = new Mock<ICache>();
            prodRepository = new Mock<IRepository<Product>>();
            validator = new Mock<IValidator<DocAddLineCmd>>();
            registrator = new Mock<IDocumentRegistrator<RelocationDocument>>();
            controller = new RelocationDocumentController(repository.Object);
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithoutParameters()
        {
            repository.Setup(m=>m.Get()).Returns(new List<RelocationDocument>());

            var result = controller.Get();

            Assert.IsType<PaginateResult<RelocationDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<RelocationDocument>());

            var result = controller.Get(fullNumber: "RLC/001/2016");

            Assert.IsType<PaginateResult<RelocationDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<RelocationDocument>());

            var result = controller.Get(issueDate: DateTime.Today);

            Assert.IsType<PaginateResult<RelocationDocument>>(result);
        }
    }
}