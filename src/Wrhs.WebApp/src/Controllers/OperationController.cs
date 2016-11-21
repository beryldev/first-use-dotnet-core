
using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Operations;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers
{
    public abstract class OperationController<TOper, TDoc, TRequest> : BaseController, IOperationController<TOper, TDoc, TRequest>
        where TOper : IOperation
        where TDoc : class, IEntity  
    {
        protected readonly ICache cache;

        public OperationController(ICache cache)
        {
            this.cache = cache;
        }

        protected abstract TOper CreateOperation(TDoc baseDocument);

        protected abstract TOper CreateOperation(OperationState<TDoc> state);

        protected abstract OperationState<TDoc> ReadOperationState(TOper operation);

        protected OperationState<TDoc> GetCachedState(string guid)
        {
            return cache.GetValue(guid) as OperationState<TDoc>;
        }

        protected abstract void DoStep(TOper operation, TRequest request);

        [HttpGet("new/{documentId}")]
        public IActionResult NewOperation(int documentId, [FromServices]IRepository<TDoc> documentRepository)
        {
            var document = documentRepository.GetById(documentId);
            if(document == null)
                return NotFound();

            var operation = CreateOperation(document);

            var state = ReadOperationState(operation);
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

        [HttpPost("{guid}/step")]
        public IActionResult AddStep(string guid, [FromBody]TRequest request)
        {
            IActionResult result;
            var state = GetCachedState(guid);
            if(state == null)
                return NotFound();

            var operation = CreateOperation(state);
            
            try
            {
                DoStep(operation, request);
                result = Ok(ReadOperationState(operation));
            }
            catch(ArgumentException e)
            {
                var results = new ValidationResult[] { new ValidationResult("AllocateItem", e.Message) };
                result = BadRequest(results);
            }
            catch(InvalidOperationException e)
            {
                var results = new ValidationResult[] { new ValidationResult("AllocateItem", e.Message) };
                result = BadRequest(results);
            }
            finally
            {
                cache.SetValue(guid, ReadOperationState(operation));
            }
            
            return result;
        }

        [HttpPost("{guid}")]
        public IActionResult Perform(string guid, [FromServices]IWarehouse warehouse)
        {
            var state = GetCachedState(guid);
            if(state == null)
                return NotFound();

            var operation =  CreateOperation(state);

            try
            {
                warehouse.ProcessOperation(operation);
            }
            catch(InvalidOperationException e)
            {
                var result = new ValidationResult[] { new ValidationResult("Perform", e.Message) };
                return BadRequest(result);
            }
            catch(ArgumentException e)
            {
                var result = new ValidationResult[] { new ValidationResult("Perform", e.Message) };
                return BadRequest(result);
            }
            catch(Exception e)
            {
                var result = new ValidationResult[] { new ValidationResult("Perform", e.Message) };
                return BadRequest(result);
            }
            
            return Ok();
        }

    }
}