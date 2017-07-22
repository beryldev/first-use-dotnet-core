using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wrhs.Core.Exceptions;

namespace Wrhs.WebApp.Filters
{
    public class CommandValidationFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CommandValidationException ex)
            {
                context.Result = new BadRequestObjectResult(ex.ValidationResults);
            }
        }
    }
}
