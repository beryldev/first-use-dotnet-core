using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Products;
using Wrhs.Products.Commands;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        IRepository<Product> productRepository;
        
        public ProductsController(IRepository<Product> productRepository)
        {
           this.productRepository = productRepository;
        }

        [HttpGet]
        public IPaginateResult<Product> Get()
        {
            var paginator = new Paginator<Product>();
            var search = new ResourceSearch<Product>(productRepository, paginator, new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            return search.Exec(criteria);
        }

        [HttpPost]
        public IActionResult Post([FromBody]CreateProductCommand cmd, [FromServices]IValidator<CreateProductCommand> validator, 
            [FromServices]ICommandHandler<CreateProductCommand> handler)
        {            
            return HandleCommand<CreateProductCommand>(handler, validator, cmd);
        }

        [HttpGet("{productId}/stocks")]
        public IEnumerable<Stock> Stocks(int productId, [FromServices]IWarehouse warehouse)
        {
            var product = productRepository.GetById(productId);

            return product != null ? warehouse.ReadStocksByProductCode(product.Code)
                : new List<Stock>();
        }

        [HttpGet("{productId}/stocks/calculated")]
        public IEnumerable<Stock> CalculatedStocks(int productId, [FromServices]IWarehouse warehouse)
        {
            var product = productRepository.GetById(productId);

            return product != null ? warehouse.CalculateStocks(product.Code)
                : new List<Stock>();
        }
    }
}