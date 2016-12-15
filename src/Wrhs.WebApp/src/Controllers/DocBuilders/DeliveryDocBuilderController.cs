// using Microsoft.AspNetCore.Mvc;
// using Wrhs.Core;
// using Wrhs.Documents;
// using Wrhs.Operations.Delivery;
// using Wrhs.Products;
// using Wrhs.WebApp.Utils;

// namespace Wrhs.WebApp.Controllers.DocBuilders
// {
//     [Route("api/document/delivery")]
//     public class DeliveryDocBuilderController
//         : DocBuilderController<DeliveryDocument, DeliveryDocumentLine, DocAddLineCmd>
//     {
//         public DeliveryDocBuilderController(ICache cache, IRepository<Product> productRepo, 
//             IValidator<DocAddLineCmd> validator) 
//             : base(cache, productRepo, validator)
//         {
//         }

//         protected override DocumentBuilder<DeliveryDocument, DeliveryDocumentLine, DocAddLineCmd> CreateDocBuilder()
//         {
//             return new DeliveryDocumentBuilder(productRepo, validator);
//         }

//         protected override DocumentBuilder<DeliveryDocument, DeliveryDocumentLine, DocAddLineCmd> CreateDocBuilder(DeliveryDocument baseDoc)
//         {
//             return new DeliveryDocumentBuilder(productRepo, validator, baseDoc);
//         }
//     }
// }