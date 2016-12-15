// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Wrhs.Core;
// using Wrhs.Documents;
// using Wrhs.Products;
// using Wrhs.WebApp.Controllers.DocBuilders;
// using Wrhs.WebApp.Utils;
// using Xunit;

// namespace Wrhs.WebApp.Tests
// {
//     public abstract class DocBuilderControllerTests<TDoc, TLine, TCmd> 
//         where TDoc : class, IEntity, INumerableDocument, IDocument<TLine>
//         where TLine : IDocumentLine
//         where TCmd : IDocAddLineCmd
//     {
//         protected string guid;

//         protected Mock<ICache> cache;

//         protected Mock<IRepository<Product>> prodRepository;

//         protected Mock<IValidator<TCmd>> validator;

//         protected Mock<IDocumentRegistrator<TDoc>> registrator;

//         protected DocBuilderController<TDoc, TLine, TCmd> controller;

//         public DocBuilderControllerTests()
//         {
//             guid = "some-guid";
//             cache = new Mock<ICache>();
//             prodRepository = new Mock<IRepository<Product>>();
//             validator = new Mock<IValidator<TCmd>>();
//             registrator = new Mock<IDocumentRegistrator<TDoc>>();
//             controller = CreateController(new CreateParameterObject
//             {
//                 Cache = cache.Object,
//                 ProdRepository = prodRepository.Object,
//                 Validator = validator.Object
//             });
//         }

//         [Fact]
//         public void ShouldReturnTempDocUidOnNewDocument()
//         {
//             var result = controller.NewDocument();

//             Assert.IsType<string>(result);
//             Assert.NotEqual(String.Empty, result);
//         }

//         [Fact]
//         public void ShouldCacheBuildedDocumentUnderReturnedUidOnNewDocument()
//         {
//             var uid = controller.NewDocument();

//             cache.Verify(m=>m.SetValue(uid, It.IsAny<TDoc>()), Times.Once());
//         }

//         [Fact]
//         public void ShouldOperateCachedBuildedDocumentOnAddLine()
//         {
//             var cmd = CreateAddLineCmd();
//             var document = CreateDocument();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);
//             cache.Setup(m=>m.SetValue(It.IsAny<string>(), It.IsAny<TDoc>()));

//             controller.AddLine(guid, cmd);

//             cache.Verify(m=>m.GetValue(guid), Times.Once());
//             cache.Verify(m=>m.SetValue(guid, It.IsAny<TDoc>()));
//         }

//         [Fact]
//         public void ShouldReturnOkWithLinesOnAddLineWhenSuccess()
//         {
//             var cmd = CreateAddLineCmd();
//             var document = CreateDocument();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);
//             validator.Setup(m=>m.Validate(It.IsAny<TCmd>())).Returns(new List<ValidationResult>());

//             var result = controller.AddLine(guid, cmd);

//             Assert.IsType<OkObjectResult>(result);
//             Assert.Equal(1, ((IEnumerable<TLine>)(result as OkObjectResult).Value).Count());
//         }

//         [Fact]
//         public void ShouldReturnBadRequestWithErrorsOnAddLineWhenFail()
//         {
//             var cmd = CreateAddLineCmd();
//             var document = CreateDocument();
//             validator.Setup(m=>m.Validate(It.IsAny<TCmd>())).Returns(new List<ValidationResult>(){new ValidationResult()});   
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.AddLine(guid, cmd);

//             Assert.IsType<BadRequestObjectResult>(result);
//             Assert.NotEmpty(((BadRequestObjectResult)result).Value as IEnumerable<ValidationResult>);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnAddLineWhenBuildedDocumentNotExists()
//         {
//             var cmd = CreateAddLineCmd();
//             validator.Setup(m=>m.Validate(It.IsAny<TCmd>())).Returns(new List<ValidationResult>(){new ValidationResult()});
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(null);

//             var result = controller.AddLine(guid, cmd);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public void ShouldSaveBuildedDocumentToCacheOnAddLineWhenSuccess()
//         {
//             var cmd = CreateAddLineCmd();
//             var document = CreateDocument();
//             validator.Setup(m=>m.Validate(It.IsAny<TCmd>())).Returns(new List<ValidationResult>());           
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.AddLine(guid, cmd);

//             Assert.IsType<OkObjectResult>(result);
//             cache.Verify(m=>m.SetValue(guid, It.IsAny<TDoc>()), Times.Once());
//         }

//         [Fact]
//         public void ShouldReturnOkWithDocumentOnGetDocumentWhenSuccess()
//         {
//             var document = CreateDocument();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.GetDocument(guid);

//             Assert.IsType<OkObjectResult>(result);
//             Assert.IsType<TDoc>(((OkObjectResult)result).Value);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnGetDocumentWhenBuilderNotExists()
//         {
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(null);

//             var result = controller.GetDocument(guid);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public void ShouldReturnOkWithDocumentLinesOnGetDocumentLinesWhenSuccess()
//         {
//             var document = CreateDocument(); 
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.GetDocumentLines(guid);

//             Assert.IsType<OkObjectResult>(result);
//             Assert.IsType<TLine[]>(((OkObjectResult)result).Value);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnGetDocumentLinesWhenBuildedDocumentNotExists()
//         {
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(null);

//             var result = controller.GetDocumentLines(guid);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public void ShouldReturnOkWithLinesOnUpdateLineWhenSuccess()
//         {
//             var line = CreateDocumentLine();
//             var document = CreateDocument(); 
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.UpdateLine(guid, line);

//             Assert.IsType<OkObjectResult>(result);
//             var obj = result as OkObjectResult;
//             Assert.IsType<TLine[]>(obj.Value);
//         }

//         [Fact]
//         public void ShouldSaveBuildedDocumentToCacheOnUpdateLineWhenSuccess()
//         {
//             var line = CreateDocumentLine();
//             var document = CreateDocument();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>())).Returns(document);
//             cache.Setup(m=>m.SetValue(It.IsAny<string>(), It.IsAny<TDoc>())).Verifiable();

//             var result = controller.UpdateLine(guid, line);

//             cache.Verify(m=>m.SetValue(guid, It.IsAny<TDoc>()), Times.Once());
//         }

//         [Fact]
//         public void ShouldReturnBadRequestWithErrorsOnUpdateLineWhenFail()
//         {
//             var line = CreateDocumentLine();
//             var document = CreateDocument();
//             validator.Setup(m=>m.Validate(It.IsAny<TCmd>()))
//                 .Returns(new List<ValidationResult>(){new ValidationResult()});
            
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.UpdateLine(guid, line);

//             Assert.IsType<BadRequestObjectResult>(result);
//             Assert.NotEmpty(((BadRequestObjectResult)result).Value as IEnumerable<ValidationResult>);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnUpdateLineWhenBuildedDocumentNotExists()
//         {
//             var line = CreateDocumentLine();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(null);

//             var result = controller.UpdateLine(guid, line);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public void ShouldReturnOkWithLinesOnDeleteLineWhenSuccess()
//         {
//             var line = CreateDocumentLine();
//             var document = CreateDocument();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(document);

//             var result = controller.DeleteLine(guid, line);

//             Assert.IsType<OkObjectResult>(result);
//             var obj = ((OkObjectResult)result).Value as IEnumerable<TLine>;
//             Assert.Empty(obj);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnDeleteLineWhenBuildedDocumentNotExists()
//         {
//             var line = CreateDocumentLine();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>()))
//                 .Returns(null);

//             var result = controller.DeleteLine(guid, line);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public void ShouldUpdateBuildedDocInCacheAfterDelete()
//         {
//             var document = CreateDocument();
//             var line = CreateDocumentLine();
//             cache.Setup(m=>m.SetValue(guid, It.IsAny<TDoc>())).Verifiable();
//             cache.Setup(m=>m.GetValue(guid)).Returns(document);

//             var result = controller.DeleteLine(guid, line);

//             Assert.IsType<OkObjectResult>(result);
//             cache.Verify(m=>m.SetValue(guid, It.IsAny<TDoc>()), Times.Once());
//         }

//         [Fact]
//         public void ShouldReturnOkWithDeliveryDocumentOnRegisterWhenSuccess()
//         {  
//             var document = CreateDocument();
//             cache.Setup(m=>m.GetValue(It.IsAny<string>())).Returns(document);

//             var result = controller.Register(guid, registrator.Object);

//             Assert.IsType<OkObjectResult>(result);
//             var obj = ((OkObjectResult)result).Value;
//             Assert.IsType<TDoc>(obj);
//         }

//         [Fact]
//         public void ShouldReturnNotFoundOnRegisterWhenDocumentNotExists()
//         {
//             cache.Setup(m=>m.GetValue(It.IsAny<string>())).Returns(null);

//             var result = controller.Register(guid, registrator.Object);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Theory]
//         [InlineData("some remarks")]
//         [InlineData("")]
//         [InlineData(" ")]
//         [InlineData(null)]
//         public void ShouldSetPassedDocumentRemarksOnRegister(string remarks)
//         {
//             string savedRemarks = null;
//             var document = CreateDocument();
//             var documentParameter = CreateDocument();
//             documentParameter.Remarks = remarks;
//             cache.Setup(m => m.GetValue(It.IsAny<string>())).Returns(document);
//             registrator.Setup(m => m.Register(It.IsAny<TDoc>()))
//                 .Callback((TDoc doc) => { savedRemarks = doc.Remarks; });
            
//             controller.Register(guid, registrator.Object, documentParameter);
            
//             Assert.Equal(remarks, savedRemarks);
//         }

//         protected abstract DocBuilderController<TDoc, TLine, TCmd> CreateController(CreateParameterObject parameter);

//         protected abstract TDoc CreateDocument();

//         protected abstract TLine CreateDocumentLine();

//         protected abstract TCmd CreateAddLineCmd();

//         protected class CreateParameterObject
//         {
//             public IRepository<Product> ProdRepository { get; set; }

//             public ICache Cache { get; set; }

//             public IValidator<TCmd> Validator { get; set; }
//         }
//     }
// }