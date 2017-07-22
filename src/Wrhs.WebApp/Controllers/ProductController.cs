using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/product")]
    public class ProductController : BaseController
    {
        private readonly ICommandBus cmdBus;

        public ProductController(ICommandBus cmdBus)
        {
            this.cmdBus = cmdBus;
        }
        
        [HttpPost]
        public void CreateProduct([FromBody]CreateProductCommand command)
        {
            cmdBus.Send(command);
        }

        [HttpPut("{id}")]
        public void UpdateProduct(int id, [FromBody]UpdateProductCommand command)
        {
            command.ProductId = id;
            cmdBus.Send(command);
        }

        [HttpDelete("{id}")]
        public void DeleteProduct(int id)
        {   
            var command = new DeleteProductCommand{ ProductId = id };
            cmdBus.Send(command);
        }
    }
}