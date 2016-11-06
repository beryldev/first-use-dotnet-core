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
    public class DeliveryDocumentControllerTests
    {
        string guid;

        Mock<IRepository<DeliveryDocument>> repository;

        Mock<ICache> cache;

        Mock<IRepository<Product>> prodRepository;

        Mock<IValidator<IDocAddLineCmd>> validator;

        public DeliveryDocumentControllerTests()
        {
            guid = "someguid";
            repository = new Mock<IRepository<DeliveryDocument>>();
            cache = new Mock<ICache>();
            prodRepository = new Mock<IRepository<Product>>();
            validator = new Mock<IValidator<IDocAddLineCmd>>();
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithoutParameters()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.Get();

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.Get(fullNumber: "D/001/2016");

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.Get(issueDate: DateTime.Today);

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnTempDocUidOnNewDocument()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.NewDocument(cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<string>(result);
            Assert.NotEqual(String.Empty, result);
        }

        [Fact]
        public void ShouldCacheBuildedDocumentUnderReturnedUidOnNewDocument()
        {
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var uid = controller.NewDocument(cache.Object, prodRepository.Object, validator.Object);

            cache.Verify(m=>m.SetValue(uid, It.IsAny<DeliveryDocument>()), Times.Once());
        }

        [Fact]
        public void ShouldOperateCachedBuildedDocumentOnAddLine()
        {
            var document = new DeliveryDocument();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            cache.Setup(m=>m.SetValue(It.IsAny<string>(), It.IsAny<DeliveryDocument>()));
            var controller = new DeliveryDocumentController(repository.Object);

            controller.AddLine(guid, new DocAddLineCmd(), cache.Object, prodRepository.Object, validator.Object);

            cache.Verify(m=>m.GetValue(guid), Times.Once());
            cache.Verify(m=>m.SetValue(guid, It.IsAny<DeliveryDocument>()));
        }

        [Fact]
        public void ShouldReturnOkWithLinesOnAddLineWhenSuccess()
        {
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(guid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, ((IEnumerable<DeliveryDocumentLine>)(result as OkObjectResult).Value).Count());
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnAddLineWhenFail()
        {
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>(){new ValidationResult()});
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);

            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(guid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotEmpty(((BadRequestObjectResult)result).Value as IEnumerable<ValidationResult>);
        }

        [Fact]
        public void ShouldReturnNotFoundOnAddLineWhenBuildedDocumentNotExists()
        {
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>(){new ValidationResult()});
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);

            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(guid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldSaveBuildedDocumentToCacheOnAddLineWhenSuccess()
        {
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>());
            var document = new DeliveryDocument();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(guid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            cache.Verify(m=>m.SetValue(guid, It.IsAny<DeliveryDocument>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnOkWithDocumentOnGetDocumentWhenSuccess()
        {
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocument(guid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DeliveryDocument>(((OkObjectResult)result).Value);
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetDocumentWhenBuilderNotExists()
        {
            var builder = new DeliveryDocumentBuilder(prodRepository.Object, validator.Object); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocument(guid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldReturnOkWithDocumentLinesOnGetDocumentLinesWhenSuccess()
        {
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocumentLines(guid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DeliveryDocumentLine[]>(((OkObjectResult)result).Value);
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetDocumentLinesWhenBuildedDocumentNotExists()
        {
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocumentLines(guid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldReturnOkWithLinesOnUpdateLineWhenSuccess()
        {
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            var obj = result as OkObjectResult;
            Assert.IsType<DeliveryDocumentLine[]>(obj.Value);
        }

        [Fact]
        public void ShouldSaveBuildedDocumentToCacheOnUpdateLineWhenSuccess()
        {
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var document = new DeliveryDocument();
            cache.Setup(m=>m.GetValue(It.IsAny<string>())).Returns(document);
            cache.Setup(m=>m.SetValue(It.IsAny<string>(), It.IsAny<DeliveryDocument>())).Verifiable();
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            cache.Verify(m=>m.SetValue(guid, It.IsAny<DeliveryDocument>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnUpdateLineWhenFail()
        {
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            validator.Setup(m=>m.Validate(It.IsAny<IDocAddLineCmd>()))
                .Returns(new List<ValidationResult>(){new ValidationResult()});
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotEmpty(((BadRequestObjectResult)result).Value as IEnumerable<ValidationResult>);
        }

        [Fact]
        public void ShouldReturnNotFoundOnUpdateLineWhenBuildedDocumentNotExists()
        {
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldReturnOkWithLinesOnDeleteLineWhenSuccess()
        {
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var document = new DeliveryDocument(); 
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.DeleteLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            var obj = ((OkObjectResult)result).Value as IEnumerable<DeliveryDocumentLine>;
            Assert.Empty(obj);
        }

        [Fact]
        public void ShouldReturnNotFoundOnDeleteLineWhenBuildedDocumentNotExists()
        {
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.DeleteLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldUpdateBuildedDocInCacheAfterDelete()
        {
            var document = new DeliveryDocument();
            var line = new DeliveryDocumentLine();
            cache.Setup(m=>m.SetValue(guid, It.IsAny<DeliveryDocument>())).Verifiable();
            cache.Setup(m=>m.GetValue(guid)).Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.DeleteLine(guid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            cache.Verify(m=>m.SetValue(guid, It.IsAny<DeliveryDocument>()), Times.Once());
        }
    }
}