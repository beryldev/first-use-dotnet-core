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
        public IActionResult Get(ProductFilter filter,
            int page=1, int pageSize=20)
        {
            page = page < 1 ? 1 : page;
            pageSize = (pageSize < 1 || pageSize > 100) ? 20 : pageSize;

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
    }
}