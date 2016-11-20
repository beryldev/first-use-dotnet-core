using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wrhs.Core;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Wrhs.WebApp.Controllers.Operations;
using Wrhs.WebApp.Utils;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryOperationControllerTests
    {
        const string DELIVERY_DOC_NUMBER = @"DEL\001\2016";

        const string OPERATION_GUID_OK = "ok-guid";

        const string OPERATION_GUID_BAD = "bad-guid";

        readonly DeliveryOperationController controller;

        readonly Mock<ICache> cacheMock;

        readonly Mock<IRepository<DeliveryDocument>> docRepoMock;

        readonly Mock<IWarehouse> warehouseMock;

        public DeliveryOperationControllerTests()
        {
            var state = new DeliveryOperation.State();
            state.BaseDocument = new DeliveryDocument(){ FullNumber = DELIVERY_DOC_NUMBER };
            state.BaseDocument.Lines.Add(new DeliveryDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = "PROD1"},
                Quantity = 5
            });
            cacheMock = new Mock<ICache>();
            cacheMock.Setup(m => m.GetValue(It.IsAny<string>()))
                .Returns((string guid) => { return guid == OPERATION_GUID_OK ? state : null; });

            controller = new DeliveryOperationController(cacheMock.Object); 
            docRepoMock = new Mock<IRepository<DeliveryDocument>>();
            docRepoMock.Setup(m => m.GetById(It.IsAny<int>()))
                .Returns(new DeliveryDocument(){ FullNumber = DELIVERY_DOC_NUMBER });

            warehouseMock = new Mock<IWarehouse>();
        }

        [Fact]
        public void ShouldReturnOkWithGuidOnNewOperation()
        {
            var result = controller.NewOperation(1, docRepoMock.Object);

            Assert.IsType<OkObjectResult>(result);
            Assert.NotEmpty((result as OkObjectResult).Value as string);
        }

        [Fact]
        public void ShouldPutClearOperationStateWithBaseDocumentIntoCacheOnNewOperation()
        {
            DeliveryOperation.State state = null;
            cacheMock.Setup(m => m.SetValue(It.IsNotNull<string>(), It.IsAny<object>()))
                .Callback((string key, object value) => { state = (DeliveryOperation.State)value; });

            var result = controller.NewOperation(1, docRepoMock.Object);

            var guid = (result as OkObjectResult).Value as string;
            cacheMock.Verify(m => m.SetValue(guid, It.IsNotNull<DeliveryOperation.State>()));
            state.BaseDocument.FullNumber.Should().Be(DELIVERY_DOC_NUMBER);
        }

        [Fact]
        public void ShouldReturnNotFoundOnNewOperationWhenDocumentNotExists()
        {
            docRepoMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((DeliveryDocument)null);

            var result = controller.NewOperation(1, docRepoMock.Object);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ShouldReturnOkWithOperationStateOnGetOperation()
        {
            var state = new DeliveryOperation.State();
            state.BaseDocument = new DeliveryDocument(){ FullNumber = DELIVERY_DOC_NUMBER };

            var result = controller.GetOperation(OPERATION_GUID_OK);

            result.Should().BeOfType<OkObjectResult>();
            var resultState = (result as OkObjectResult).Value as DeliveryOperation.State;
            resultState.BaseDocument.FullNumber.Should().Be(DELIVERY_DOC_NUMBER);
        }

        [Fact]
        public void ShouldReturnNotFoundOnGetOperationWhenOperationNotExists()
        {
            var result = controller.GetOperation(OPERATION_GUID_BAD);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ShouldReturnOkWithOperationStateOnAllocateItem()
        {
            var line = new DeliveryDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = "PROD1"},
                Quantity = 5
            };

            var request = new DeliveryOperationController.AllocationRequest
            {
                Line = line,
                Quantity = 1,
                Location = "LOC-001-01"
            };
            var result = controller.AllocateItem(OPERATION_GUID_OK, request);

            result.Should().BeOfType<OkObjectResult>();
            var state = (result as OkObjectResult).Value as DeliveryOperation.State;
            state.BaseDocument.FullNumber.Should().Be(DELIVERY_DOC_NUMBER);
            state.PendingAllocations.Should().NotBeEmpty();
            
            var allocation = state.PendingAllocations.First();
            allocation.Product.Code.Should().Be("PROD1");
            allocation.Quantity.Should().Be(request.Quantity);
            allocation.Location.Should().Be(request.Location);
        }

        [Fact]
        public void ShouldReturnNotFoundOnAllocateItemWhenOperationNotExists()
        {
            var line = new DeliveryDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = "PROD1"},
                Quantity = 5
            };

            var request = new DeliveryOperationController.AllocationRequest
            {
                Line = line,
                Quantity = 1,
                Location = "LOC-001-01"
            };
            var result = controller.AllocateItem(OPERATION_GUID_BAD, request);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData(10, "PROD1")]
        [InlineData(1, "PROD4")]
        [InlineData(10, "PROD1")]
        public void ShouldReturnBadRequestWithValidationResultsOnAllocateItemWhenAllocationError(decimal quantity, string productCode)
        {
            var line = new DeliveryDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = productCode},
                Quantity = 5
            };

            var request = new DeliveryOperationController.AllocationRequest
            {
                Line = line,
                Quantity = quantity,
                Location = "LOC-001-01"
            };
            var result = controller.AllocateItem(OPERATION_GUID_OK, request);

            result.Should().BeOfType<BadRequestObjectResult>();
            var validationResult = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            validationResult.Should().NotBeEmpty();
            validationResult.First().Message.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void ShouldStoreBackToCacheStateOnAllocateItemWhenSuccess()
        {
            var line = new DeliveryDocumentLine
            {
                Product = new Product { Id = 1, Name = "Product 1", Code = "PROD1"},
                Quantity = 5
            };

            var request = new DeliveryOperationController.AllocationRequest
            {
                Line = line,
                Quantity = 1,
                Location = "LOC-001-01"
            };
            var result = controller.AllocateItem(OPERATION_GUID_OK, request);

            cacheMock.Verify(m => m.SetValue(OPERATION_GUID_OK, It.IsNotNull<DeliveryOperation.State>()), Times.Once());
        }

        [Fact]
        public void ShouldReturnOkOnPerformWhenSuccess()
        {
            var result = controller.Perform(OPERATION_GUID_OK, warehouseMock.Object);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void ShouldReturnNotFoundOnPerformWhenOperationNotExists()
        {
            var result = controller.Perform(OPERATION_GUID_BAD, warehouseMock.Object);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ShouldReturnBadRequestWithValidationResultOnPerformWhenError()
        {
            warehouseMock.Setup(m => m.ProcessOperation(It.IsAny<IOperation>()))
                .Throws(new InvalidOperationException("Some exception"));

            var result = controller.Perform(OPERATION_GUID_OK, warehouseMock.Object);

            result.Should().BeOfType<BadRequestObjectResult>();
            var validationResult = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
            validationResult.Should().NotBeEmpty();
            validationResult.First().Message.Should().NotBeNullOrWhiteSpace();
        }
    }
}