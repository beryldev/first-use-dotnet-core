using System;
using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Documents;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Wrhs.WebApp.Controllers;
using Wrhs.WebApp.Utils;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class RelocationDocumentControllerTests
    {
        string guid;

        Mock<IRepository<RelocationDocument>> repository;

        Mock<ICache> cache;

        Mock<IRepository<Product>> prodRepository;

        Mock<IValidator<IDocAddLineCmd>> validator;

        Mock<IDocumentRegistrator<RelocationDocument>> registrator;

        RelocationDocumentController controller;

        public RelocationDocumentControllerTests()
        {
            guid = "someguid";
            repository = new Mock<IRepository<RelocationDocument>>();
            cache = new Mock<ICache>();
            prodRepository = new Mock<IRepository<Product>>();
            validator = new Mock<IValidator<IDocAddLineCmd>>();
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

        [Fact]
        public void ShouldReturnTempDocUidOnNewDocument()
        {
            repository.Setup(m=>m.Get()).Returns(new List<RelocationDocument>());
            
            var result = controller.NewDocument(cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<string>(result);
            Assert.NotEqual(String.Empty, result);
        }
    }
}