using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;

namespace Wrhs.WebApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ICommandBus cmdBus;

        public ProductController(ICommandBus cmdBus)
        {
            this.cmdBus = cmdBus;
        }
        
        public IActionResult CreateProduct(CreateProductCommand command)
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