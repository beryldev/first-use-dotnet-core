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
        public IActionResult CreateProduct([FromBody]CreateProductCommand command)
        {
            IActionResult result;

            try
            {
                cmdBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }
            
            return result;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody]UpdateProductCommand command)
        {
            IActionResult result;

            command.ProductId = id;

            try
            {
                cmdBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }
            
            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            IActionResult result;
            
            var command = new DeleteProductCommand{ ProductId = id };

            try
            {
                cmdBus.Send(command);
                result = Ok();
            }
            catch(CommandValidationException e)
            {
                result = BadRequest(e.ValidationResults);
            }
            
            return result;
        }
    }
}