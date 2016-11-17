using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.WebApp.Controllers;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.src.Controllers.Operations
{
    [Route("api/operation/delivery")]
    public class DeliveryOperationController : BaseController
    {
        readonly ICache cache;
    
        public DeliveryOperationController(ICache cache)
        {
            this.cache = cache;
        }

        [HttpGet("new")]
        public IActionResult NewOperation()
        {
            var guid = Guid.NewGuid().ToString();

            cache.SetValue(guid, null);

            return Ok(guid);
        }
    }
}