// using Wrhs.Operations.Relocation;
// using Wrhs.Products;
// using Wrhs.WebApp.Controllers.DocBuilders;

// namespace Wrhs.WebApp.Tests
// {
//     public class RelocationDocBuilderControllerTests
//         : DocBuilderControllerTests<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd>
//     {
//         protected override RelocDocAddLineCmd CreateAddLineCmd()
//         {
//             return new RelocDocAddLineCmd();
//         }

//         protected override DocBuilderController<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd> CreateController(CreateParameterObject parameter)
//         {
//             return new RelocationDocBuilderController(cache.Object, prodRepository.Object,
//                 validator.Object);
//         }

//         protected override RelocationDocument CreateDocument()
//         {
//             return new RelocationDocument();
//         }

//         protected override RelocationDocumentLine CreateDocumentLine()
//         {
//             return new RelocationDocumentLine(){Product = new Product(), Quantity = 1, From = "LOC-001-01", To = "LOC-001-02"};
//         }
//     }
// }