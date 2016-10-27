using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;

namespace Wrhs.WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IActionResult HandleCommand<T>(ICommandHandler<T> handler, IValidator<T> validator, T cmd)
        {
            var service = new ValidationCommandHandlerDecorator<T>
                (handler, validator);

            service.Handle(cmd);
            
            if(service.ValidationResults.Count() > 0)
                return BadRequest(service.ValidationResults);

            return Ok();
        }
    }
}