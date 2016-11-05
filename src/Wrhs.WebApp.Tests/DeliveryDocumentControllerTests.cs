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
        [Fact]
        public void ShouldReturnDocumentsOnGetWithoutParameters()
        {
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.Get();

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        {
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.Get(fullNumber: "D/001/2016");

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        {
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.Get(issueDate: DateTime.Today);

            Assert.IsType<PaginateResult<DeliveryDocument>>(result);
        }

        [Fact]
        public void ShouldReturnTempDocUidOnNewDocument()
        {
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var cache = new Mock<ICache>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.NewDocument(cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<string>(result);
            Assert.NotEqual(String.Empty, result);
        }

        [Fact]
        public void ShouldCacheBuildedDocumentUnderReturnedUidOnNewDocument()
        {
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var cache = new Mock<ICache>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var controller = new DeliveryDocumentController(repository.Object);

            var uid = controller.NewDocument(cache.Object, prodRepository.Object, validator.Object);

            cache.Verify(m=>m.SetValue(uid, It.IsAny<DeliveryDocument>()), Times.Once());
        }

        [Fact]
        public void ShouldOperateCachedBuildedDocumentOnAddLine()
        {
            var uid = "some-uid";
            var document = new DeliveryDocument();
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            cache.Setup(m=>m.SetValue(It.IsAny<string>(), It.IsAny<DeliveryDocument>()));
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var controller = new DeliveryDocumentController(repository.Object);

            controller.AddLine(uid, new DocAddLineCmd(), cache.Object, prodRepository.Object, validator.Object);

            cache.Verify(m=>m.GetValue(uid), Times.Once());
            cache.Verify(m=>m.SetValue(uid, It.IsAny<DeliveryDocument>()));
        }

        [Fact]
        public void ShouldReturnOkWithLinesOnAddLineWhenSuccess()
        {
            var uid = "111-22-3";
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>());
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(uid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, ((IEnumerable<DeliveryDocumentLine>)(result as OkObjectResult).Value).Count());
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnAddLineWhenFail()
        {
            var uid = "111-22-3";
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>(){new ValidationResult()});
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);

            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(uid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotEmpty(((BadRequestObjectResult)result).Value as IEnumerable<ValidationResult>);
        }

        [Fact]
        public void ShouldReturnNotFoundOnAddLineWhenBuildedDocumentNotExists()
        {
            var uid = "";
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>(){new ValidationResult()});
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);

            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(uid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldSaveBuildedDocumentToCacheOnAddLineWhenSuccess()
        {
            var uid = "someuid";
            var cmd = new DocAddLineCmd { ProductId=1, Quantity=10 };
            var repository = new Mock<IRepository<DeliveryDocument>>();
            repository.Setup(m=>m.Get()).Returns(new List<DeliveryDocument>());
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            validator.Setup(m=>m.Validate(It.IsAny<DocAddLineCmd>())).Returns(new List<ValidationResult>());
            var document = new DeliveryDocument();
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.AddLine(uid, cmd, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            cache.Verify(m=>m.SetValue(uid, It.IsAny<DeliveryDocument>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnOkWithDocumentOnGetDocumentWhenSuccess()
        {
            var uid = "someuid";
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocument(uid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DeliveryDocument>(((OkObjectResult)result).Value);
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetDocumentWhenBuilderNotExists()
        {
            var uid = "someuid";
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var builder = new DeliveryDocumentBuilder(prodRepository.Object, validator.Object); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocument(uid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldReturnOkWithDocumentLinesOnGetDocumentLinesWhenSuccess()
        {
            var uid = "someuid";
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocumentLines(uid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DeliveryDocumentLine[]>(((OkObjectResult)result).Value);
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetDocumentLinesWhenBuildedDocumentNotExists()
        {
            var uid = "someuid";
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.GetDocumentLines(uid, cache.Object, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldReturnOkOnUpdateLineWhenSuccess()
        {
            var uid = "someuid";
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(uid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ShouldReturnBadRequestWithErrorsOnUpdateLineWhenFail()
        {
            var uid = "someuid";
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            validator.Setup(m=>m.Validate(It.IsAny<IDocAddLineCmd>()))
                .Returns(new List<ValidationResult>(){new ValidationResult()});
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(uid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotEmpty(((BadRequestObjectResult)result).Value as IEnumerable<ValidationResult>);
        }

        [Fact]
        public void ShouldReturnNotFoundOnUpdateLineWhenBuildedDocumentNotExists()
        {
            var uid = "someuid";
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.UpdateLine(uid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ShouldReturnOkOnDeleteLineWhenSuccess()
        {
            var uid = "someuid";
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var document = new DeliveryDocument(); 
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(document);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.DeleteLine(uid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ShouldReturnNotFoundOnDeleteLineWhenBuildedDocumentNotExists()
        {
            var uid = "someuid";
            var line = new DeliveryDocumentLine(){Product = new Product{Id = 1, Code="PROD1", Name="Product 1"}, Quantity = 100};
            var repository = new Mock<IRepository<DeliveryDocument>>();
            var prodRepository = new Mock<IRepository<Product>>();
            var validator = new Mock<IValidator<IDocAddLineCmd>>();
            var cache = new Mock<ICache>();
            cache.Setup(m=>m.GetValue(It.IsAny<string>()))
                .Returns(null);
            var controller = new DeliveryDocumentController(repository.Object);

            var result = controller.DeleteLine(uid, cache.Object, line, prodRepository.Object, validator.Object);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}