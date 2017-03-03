using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation")]
    public class OperationReadController : BaseController
    {
        private readonly IOperationService operationSrv;

        public OperationReadController(IOperationService operationSrv)
        {
            this.operationSrv = operationSrv;
        }

        [HttpGet]
        public IActionResult GetOperations(int page=1, int pageSize=20)
        {
            var result = operationSrv.Get(page, pageSize);

            return Ok(result);
        }

        [HttpGet("{guid}")]
        public IActionResult GetOperation(string guid)
        {
            var operation = operationSrv.GetOperationByGuid(guid);
            if(operation == null)
                return NotFound();

            return Ok(operation);
        }
    }
}