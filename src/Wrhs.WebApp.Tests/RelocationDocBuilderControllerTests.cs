using System;
using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Wrhs.WebApp.Controllers;
using Wrhs.WebApp.Utils;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class RelocationDocBuilderControllerTests
    {
        string guid;

        Mock<IRepository<RelocationDocument>> repository;

        Mock<ICache> cache;

        Mock<IRepository<Product>> prodRepository;

        Mock<IValidator<RelocDocAddLineCmd>> validator;

        Mock<IDocumentRegistrator<RelocationDocument>> registrator;

        RelocationDocBuilderController controller;

        public RelocationDocBuilderControllerTests()
        {
            guid = "someguid";
            repository = new Mock<IRepository<RelocationDocument>>();
            cache = new Mock<ICache>();
            prodRepository = new Mock<IRepository<Product>>();
            validator = new Mock<IValidator<RelocDocAddLineCmd>>();
            registrator = new Mock<IDocumentRegistrator<RelocationDocument>>();
            controller = new RelocationDocBuilderController(cache.Object, prodRepository.Object, validator.Object);
        }

        [Fact]
        public void ShouldReturnTempDocUidOnNewDocument()
        {
            repository.Setup(m=>m.Get()).Returns(new List<RelocationDocument>());

            var result = controller.NewDocument();

            Assert.IsType<string>(result);
            Assert.NotEqual(String.Empty, result);
        }
    }
}