// using System;
// using System.Collections.Generic;
// using System.Linq;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Moq;
// using Wrhs.Core;
// using Wrhs.WebApp.Controllers;
// using Wrhs.WebApp.Utils;
// using Xunit;

// namespace Wrhs.WebApp.Tests
// {
//     public abstract class BaseOperationControllerTests<TOper, TDoc, TLine, TRequest, TCtrl>
//         where TOper : IOperation
//         where TDoc : class, IEntity, IDocument<TLine>, ISearchableDocument
//         where TLine : class, IEntity, IDocumentLine
//         where TCtrl : IOperationController<TOper, TDoc, TRequest>
//         where TRequest : class
//     {
//         protected const string DOC_NUMBER = @"DOC\001\2016";

//         protected const string OPERATION_GUID_OK = "ok-guid";

//         protected const string OPERATION_GUID_BAD = "bad-guid";

//         protected readonly TCtrl controller;

//         protected readonly Mock<ICache> cacheMock;

//         protected readonly Mock<IRepository<TDoc>> docRepoMock;

//         protected readonly Mock<IWarehouse> warehouseMock;

//         protected readonly Mock<ILogger<TOper>> loggerMock;

//         protected abstract TDoc CreateDocument();

//         protected abstract TLine CreateDocumentLine(string code = "PROD1");

//         protected abstract TRequest CreateRequest(TLine line, decimal quantity=1);

//         protected abstract TCtrl CreateController(ICache cache, ILogger<TOper> logger);

//         public BaseOperationControllerTests()
//         {
//             var state = new OperationState<TDoc>();
//             state.BaseDocument = CreateDocument();
//             state.BaseDocument.Lines.Add(CreateDocumentLine());
//             loggerMock = new Mock<ILogger<TOper>>();
//             cacheMock = new Mock<ICache>();
//             cacheMock.Setup(m => m.GetValue(It.IsAny<string>()))
//                 .Returns((string guid) => { return guid == OPERATION_GUID_OK ? state : null; });

//             controller = CreateController(cacheMock.Object, loggerMock.Object); 
//             docRepoMock = new Mock<IRepository<TDoc>>();
//             docRepoMock.Setup(m => m.GetById(It.IsAny<int>()))
//                 .Returns(CreateDocument());

//             warehouseMock = new Mock<IWarehouse>();
            
//         }

//         [Fact]
//         public void ShouldReturnOkWithGuidOnNewOperation()
//         {
//             var result = controller.NewOperation(1, docRepoMock.Object);

//             Assert.IsType<OkObjectResult>(result);
//             Assert.NotEmpty((result as OkObjectResult).Value as string);
//         }

//         [Fact]
//         public void ShouldPutClearOperationStateWithBaseDocumentIntoCacheOnNewOperation()
//         {
//             OperationState<TDoc> state = null;
//             cacheMock.Setup(m => m.SetValue(It.IsNotNull<string>(), It.IsAny<object>()))
//                 .Callback((string key, object value) => { state = (OperationState<TDoc>)value; });

//             var result = controller.NewOperation(1, docRepoMock.Object);

//             var guid = (result as OkObjectResult).Value as string;
//             cacheMock.Verify(m => m.SetValue(guid, It.IsNotNull<OperationState<TDoc>>()));
//             state.BaseDocument.FullNumber.Should().Be(DOC_NUMBER);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnNewOperationWhenDocumentNotExists()
//         {
//             docRepoMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((TDoc)null);

//             var result = controller.NewOperation(1, docRepoMock.Object);

//             result.Should().BeOfType<NotFoundResult>();
//         }

//         [Fact]
//         public void ShouldReturnOkWithOperationStateOnGetOperation()
//         {
//             var state = new OperationState<TDoc>();
//             state.BaseDocument = CreateDocument();

//             var result = controller.GetOperation(OPERATION_GUID_OK);

//             result.Should().BeOfType<OkObjectResult>();
//             var resultState = (result as OkObjectResult).Value as OperationState<TDoc>;
//             resultState.BaseDocument.FullNumber.Should().Be(DOC_NUMBER);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnGetOperationWhenOperationNotExists()
//         {
//             var result = controller.GetOperation(OPERATION_GUID_BAD);

//             result.Should().BeOfType<NotFoundResult>();
//         }

//         [Fact]
//         public void ShouldReturnOkWithOperationStateOnAddStep()
//         {
//             var line = CreateDocumentLine();
//             var request = CreateRequest(line);

//             var result = controller.AddStep(OPERATION_GUID_OK, request);

//             result.Should().BeOfType<OkObjectResult>();
//             var state = (result as OkObjectResult).Value as OperationState<TDoc>;
//             state.BaseDocument.FullNumber.Should().Be(DOC_NUMBER);
//             state.PendingAllocations.Should().NotBeEmpty();
            
//             var allocation = state.PendingAllocations.First();
//             allocation.Product.Code.Should().Be("PROD1");
//             //TODO
//             //allocation.Quantity.Should().Be(request.Quantity);
//             //allocation.Location.Should().Be(request.Location);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnAddStepWhenOperationNotExists()
//         {
//             var line = CreateDocumentLine();
//             var request = CreateRequest(line);

//             var result = controller.AddStep(OPERATION_GUID_BAD, request);

//             result.Should().BeOfType<NotFoundResult>();
//         }

//         [Theory]
//         [InlineData(10, "PROD1")]
//         [InlineData(1, "PROD4")]
//         [InlineData(10, "PROD1")]
//         public void ShouldReturnBadRequestWithValidationResultsOnAddStepWhenAllocationError(decimal quantity, string productCode)
//         {
//             var line = CreateDocumentLine(productCode);
//             var request = CreateRequest(line, quantity);

//             var result = controller.AddStep(OPERATION_GUID_OK, request);

//             result.Should().BeOfType<BadRequestObjectResult>();
//             var validationResult = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
//             validationResult.Should().NotBeEmpty();
//             validationResult.First().Message.Should().NotBeNullOrWhiteSpace();
//         }

//         [Fact]
//         public void ShouldStoreBackToCacheStateOnAddStepWhenSuccess()
//         {
//             var line = CreateDocumentLine();
//             var request = CreateRequest(line);

//             var result = controller.AddStep(OPERATION_GUID_OK, request);

//             cacheMock.Verify(m => m.SetValue(OPERATION_GUID_OK, It.IsNotNull<OperationState<TDoc>>()), Times.Once());
//         }

//         [Fact]
//         public void ShouldReturnOkOnPerformWhenSuccess()
//         {
//             var result = controller.Perform(OPERATION_GUID_OK, warehouseMock.Object);

//             result.Should().BeOfType<OkResult>();
//         }

//          [Fact]
//         public void ShouldReturnNotFoundOnPerformWhenOperationNotExists()
//         {
//             var result = controller.Perform(OPERATION_GUID_BAD, warehouseMock.Object);

//             result.Should().BeOfType<NotFoundResult>();
//         }

//         [Fact]
//         public void ShouldReturnBadRequestWithValidationResultOnPerformWhenError()
//         {
//             warehouseMock.Setup(m => m.ProcessOperation(It.IsAny<IOperation>()))
//                 .Throws(new InvalidOperationException("Some exception"));

//             var result = controller.Perform(OPERATION_GUID_OK, warehouseMock.Object);

//             result.Should().BeOfType<BadRequestObjectResult>();
//             var validationResult = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
//             validationResult.Should().NotBeEmpty();
//             validationResult.First().Message.Should().NotBeNullOrWhiteSpace();
//         }

//         [Fact]
//         public void ShouldSaveToCacheOperationStateOnPerformWhenFail()
//         {
//             warehouseMock.Setup(m => m.ProcessOperation(It.IsAny<IOperation>()))
//                 .Throws(new InvalidOperationException("Some exception"));

//             controller.Perform(OPERATION_GUID_OK, warehouseMock.Object);

//             cacheMock.Verify(m => m.SetValue(OPERATION_GUID_OK, It.IsAny<OperationState<TDoc>>()), Times.Once());
//         }

//         [Fact]
//         public void ShouldReturnBadRequestOnAddStepWhenInvalidRequestIsNull()
//         {
//             var result = controller.AddStep(OPERATION_GUID_OK, null as TRequest);

//             result.Should().BeOfType<BadRequestObjectResult>();
//             var results = (result as BadRequestObjectResult).Value as IEnumerable<ValidationResult>;
//             results.Should().NotBeEmpty();
//         }
//     }
// }