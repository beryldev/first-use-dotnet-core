// using Microsoft.AspNetCore.Mvc;
// using Wrhs.Core;
// using Wrhs.Operations;

// namespace Wrhs.WebApp.Controllers
// {
//     public interface IOperationController<TOper, TDoc, TRequest>
//         where TOper : IOperation
//         where TDoc : class, IEntity 
//     {
//         IActionResult NewOperation(int documentId, IRepository<TDoc> documentRepository);

//         IActionResult GetOperation(string guid);

//         IActionResult AddStep(string guid, TRequest request);

//         IActionResult Perform(string guid, IWarehouse warehouse);
//     }
// }