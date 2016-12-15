// using System;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Mvc;
// using Wrhs.Core;
// using Wrhs.Core.Search;
// using Wrhs.Core.Search.Interfaces;
// using Wrhs.Products;
// using Wrhs.Products.Commands;

// namespace Wrhs.WebApp.Controllers
// {
//     [Route("api/[controller]")]
//     public class ProductController : BaseController
//     {
//         IRepository<Product> productRepository;
        
//         public ProductController(IRepository<Product> productRepository)
//         {
//            this.productRepository = productRepository;
//         }

//         [HttpGet]
//         public IPaginateResult<Product> Get(string name="", string code="", string ean="", int page=1, int perPage=10)
//         {
//             var paginator = new Paginator<Product>();
//             var search = new ResourceSearch<Product>(productRepository, paginator, new ProductSearchCriteriaFactory());
//             var criteria = (ProductSearchCriteria)search.MakeCriteria();
//             criteria.Page = page;
//             criteria.PerPage = perPage;

//             if(!String.IsNullOrWhiteSpace(name))
//                 criteria.WhereName(Condition.Contains, name);

//             if(!String.IsNullOrWhiteSpace(code))
//                 criteria.WhereCode(Condition.Contains, code);

//             if(!String.IsNullOrWhiteSpace(ean))
//                 criteria.WhereEAN(Condition.Contains, ean);

//             return search.Exec(criteria);
//         }

//         [HttpGet("{id}")]
//         public IActionResult GetById(int id)
//         {
//             var product = productRepository.GetById(id);

//             if(product == null)
//                 return NotFound();

//             return Ok(product);
//         }

//         [HttpPost]
//         public IActionResult CreateProduct([FromBody]CreateProductCommand cmd, [FromServices]IValidator<CreateProductCommand> validator, 
//             [FromServices]ICommandHandler<CreateProductCommand> handler)
//         {            
//             return HandleCommand<CreateProductCommand>(handler, validator, cmd);
//         }

//         [HttpDelete("{id}")]
//         public IActionResult Delete(int id, [FromBody] DeleteProductCommand cmd, 
//             [FromServices]IValidator<DeleteProductCommand> validator, [FromServices]ICommandHandler<DeleteProductCommand> handler)
//         {
//             cmd.ProductId = id;
//             return HandleCommand<DeleteProductCommand>(handler, validator, cmd);
//         }

//         [HttpPut("{id}")]
//         public IActionResult Update(int id, [FromBody] UpdateProductCommand cmd, 
//             [FromServices]IValidator<UpdateProductCommand> validator, [FromServices]ICommandHandler<UpdateProductCommand> handler)
//         {
//             cmd.ProductId = id;
//             return HandleCommand<UpdateProductCommand>(handler, validator, cmd);
//         }

//         [HttpGet("{productId}/stocks")]
//         public IEnumerable<Stock> GetStocks(int productId, [FromServices]IWarehouse warehouse)
//         {
//             var product = productRepository.GetById(productId);

//             return product != null ? warehouse.ReadStocksByProductCode(product.Code)
//                 : new List<Stock>();
//         }

//         [HttpGet("{productId}/stocks/calculated")]
//         public IEnumerable<Stock> GetCalculatedStocks(int productId, [FromServices]IWarehouse warehouse)
//         {
//             var product = productRepository.GetById(productId);

//             return product != null ? warehouse.CalculateStocks(product.Code)
//                 : new List<Stock>();
//         }
//     }
// }