using System;
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
        public IPaginateResult<Product> Get(string name="", string code="",  string ean="")
        {
            var paginator = new Paginator<Product>();
            var search = new ResourceSearch<Product>(productRepository, paginator, new ProductSearchCriteriaFactory());
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            
            if(!String.IsNullOrWhiteSpace(name))
                criteria.WhereName(Condition.Contains, name);

            if(!String.IsNullOrWhiteSpace(code))
                criteria.WhereCode(Condition.Contains, code);

            if(!String.IsNullOrWhiteSpace(ean))
                criteria.WhereEAN(Condition.Contains, ean);

            return search.Exec(criteria);
        }

        [HttpGet("{id}")]
        public Product GetById(int id)
        {
            var product = productRepository.GetById(id);

            return product == null ? new Product() : product;
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody]CreateProductCommand cmd, [FromServices]IValidator<CreateProductCommand> validator, 
            [FromServices]ICommandHandler<CreateProductCommand> handler)
        {            
            return HandleCommand<CreateProductCommand>(handler, validator, cmd);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody] DeleteProductCommand cmd, 
            [FromServices]IValidator<DeleteProductCommand> validator, [FromServices]ICommandHandler<DeleteProductCommand> handler)
        {
            cmd.ProductId = id;
            return HandleCommand<DeleteProductCommand>(handler, validator, cmd);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateProductCommand cmd, 
            [FromServices]IValidator<UpdateProductCommand> validator, [FromServices]ICommandHandler<UpdateProductCommand> handler)
        {
            cmd.ProductId = id;
            return HandleCommand<UpdateProductCommand>(handler, validator, cmd);
        }

        [HttpGet("{productId}/stocks")]
        public IEnumerable<Stock> GetStocks(int productId, [FromServices]IWarehouse warehouse)
        {
            var product = productRepository.GetById(productId);

            return product != null ? warehouse.ReadStocksByProductCode(product.Code)
                : new List<Stock>();
        }

        [HttpGet("{productId}/stocks/calculated")]
        public IEnumerable<Stock> GetCalculatedStocks(int productId, [FromServices]IWarehouse warehouse)
        {
            var product = productRepository.GetById(productId);

            return product != null ? warehouse.CalculateStocks(product.Code)
                : new List<Stock>();
        }
    }
}