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
            return HandleCommand(command);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody]UpdateProductCommand command)
        {
            command.ProductId = id;
            return HandleCommand(command);
        }

        protected IActionResult HandleCommand(ICommand command)
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
    }
}