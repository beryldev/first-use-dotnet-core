using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Services;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/product")]
    public class ProductReadController : BaseController
    {
        private readonly IProductService productSrv;
        
        public ProductReadController(IProductService productSrv)
        {
           this.productSrv = productSrv;
        }

        [HttpGet]
        public IActionResult Get(string name="", string code="", string ean="",
            int page=1, int pageSize=20)
        {
            page = page < 1 ? 1 : page;
            pageSize = (pageSize < 1 || pageSize > 100) ? 20 : pageSize;

            var filter = new Dictionary<string, object>
            {
                {"name", name ?? string.Empty},
                {"code", code ?? string.Empty},
                {"ean", ean ?? string.Empty}
            };

            var result = productSrv.FilterProducts(filter, page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = productSrv.GetProductById(id);

            if(product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("{productId}/stock")]
        public IActionResult GetStocks(int productId, [FromServices]IStockService stockSrv)
        {
            var result = stockSrv.GetProductStock(productId);
            return Ok(result);
        }

        // [HttpPost]
        // public IActionResult CreateProduct([FromBody]CreateProductCommand cmd, [FromServices]IValidator<CreateProductCommand> validator, 
        //     [FromServices]ICommandHandler<CreateProductCommand> handler)
        // {            
        //     return HandleCommand<CreateProductCommand>(handler, validator, cmd);
        // }

        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id, [FromBody] DeleteProductCommand cmd, 
        //     [FromServices]IValidator<DeleteProductCommand> validator, [FromServices]ICommandHandler<DeleteProductCommand> handler)
        // {
        //     cmd.ProductId = id;
        //     return HandleCommand<DeleteProductCommand>(handler, validator, cmd);
        // }

        // [HttpPut("{id}")]
        // public IActionResult Update(int id, [FromBody] UpdateProductCommand cmd, 
        //     [FromServices]IValidator<UpdateProductCommand> validator, [FromServices]ICommandHandler<UpdateProductCommand> handler)
        // {
        //     cmd.ProductId = id;
        //     return HandleCommand<UpdateProductCommand>(handler, validator, cmd);
        // }

        

        // [HttpGet("{productId}/stocks/calculated")]
        // public IEnumerable<Stock> GetCalculatedStocks(int productId, [FromServices]IWarehouse warehouse)
        // {
        //     var product = productRepository.GetById(productId);

        //     return product != null ? warehouse.CalculateStocks(product.Code)
        //         : new List<Stock>();
        // }
    }
}