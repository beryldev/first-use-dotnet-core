using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Operations.Delivery;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.Operations
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
        public IActionResult NewOperation(int documentId, [FromServices]IRepository<DeliveryDocument> documentRepository)
        {
            var document = documentRepository.GetById(documentId);
            if(document == null)
                return NotFound();

            var operation = new DeliveryOperation();
            operation.SetBaseDocument(document);

            var state = operation.ReadState();
            var guid = Guid.NewGuid().ToString();

            cache.SetValue(guid, state);

            return Ok(guid);
        }

        [HttpGet("{guid}")]
        public IActionResult GetOperation(string guid)
        {
            var state = cache.GetValue(guid);
            if(state == null)
                return NotFound();

            return Ok(state);
        }

        [HttpPost("{guid}/allocation")]
        public IActionResult AllocateItem(string guid, DeliveryDocumentLine line, decimal quantity, string location)
        {
            var state = cache.GetValue(guid) as DeliveryOperation.State;
            if(state == null)
                return NotFound();

            var operation = new DeliveryOperation(state);

            try
            {
                operation.AllocateItem(line, quantity, location);
            }
            catch(ArgumentException e)
            {
                var result = new ValidationResult("AllocateItem", e.Message);
                return BadRequest(result);
            }
            catch(InvalidOperationException e)
            {
                var result = new ValidationResult("AllocateItem", e.Message);
                return BadRequest(result);
            }
            

            state = operation.ReadState();
            cache.SetValue(guid, state);

            return Ok(state);
        }

        [HttpPost("{guid}")]
        public IActionResult Perform(string guid, IWarehouse warehouse)
        {
            var state = cache.GetValue(guid) as DeliveryOperation.State;
            if(state == null)
                return NotFound();

            var operation = new DeliveryOperation(state);

            try
            {
                warehouse.ProcessOperation(operation);
            }
            catch(InvalidOperationException e)
            {
                return BadRequest(new ValidationResult("Perform", e.Message));
            }
            catch(ArgumentException e)
            {
                return BadRequest(new ValidationResult("Perform", e.Message));
            }
            

            return Ok();
        }
    }
}