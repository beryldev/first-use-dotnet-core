using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/document/delivery")]
    public class DeliveryDocumentController : BaseController
    {
        IRepository<DeliveryDocument> docRepository;

        public DeliveryDocumentController(IRepository<DeliveryDocument> repository)
        {
            docRepository = repository;
        }
        
        [HttpGet]
        public IPaginateResult<DeliveryDocument> Get(string fullNumber="", DateTime? issueDate = null)
        {
            var paginator = new Paginator<DeliveryDocument>();
            var search = new ResourceSearch<DeliveryDocument>(docRepository, paginator,
                new DocumentSearchCriteriaFactory<DeliveryDocument>());

             var criteria = (DocumentSearchCriteria<DeliveryDocument>) search.MakeCriteria();

            if(!String.IsNullOrWhiteSpace(fullNumber))
                criteria.WhereFullNumber(Condition.Contains, fullNumber);

            if(issueDate != null)
                criteria.WhereIssueDate(Condition.Equal, issueDate??DateTime.Today);
           
            return search.Exec(criteria);
        }

        [HttpGet("new")]
        public string NewDocument([FromServices]ICache cache, [FromServices]IRepository<Product> prodRepository,
            [FromServices]IValidator<IDocAddLineCmd> validator)
        {
            var builder = new DeliveryDocumentBuilder(prodRepository, validator);
            var guid = System.Guid.NewGuid().ToString();
            cache.SetValue(guid, builder.Build());

            return guid;
        }

        [HttpPost("new/{guid}/line")]
        public IActionResult AddLine(string guid, [FromBody]DocAddLineCmd cmd, [FromServices]ICache cache, 
            [FromServices]IRepository<Product> prodRepository, [FromServices]IValidator<IDocAddLineCmd> validator)
        {
            var errors = new List<ValidationResult>();
            var doc = (DeliveryDocument)cache.GetValue(guid);
            if(doc == null)
                return NotFound();

            var builder = new DeliveryDocumentBuilder(prodRepository, validator, doc);
            builder.OnAddLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };

            builder.AddLine(cmd);

            if(errors.Count > 0)
                return BadRequest(errors);

            cache.SetValue(guid, builder.Build());
            return Ok(builder.Lines);
        }

        [HttpGet("new/{guid}")]
        public IActionResult GetDocument(string guid, [FromServices]ICache cache,
            [FromServices]IRepository<Product> prodRepository, [FromServices]IValidator<IDocAddLineCmd> validator)
        {
            var doc = cache.GetValue(guid) as DeliveryDocument;
            if(doc == null)
                return NotFound();

            var builder = new DeliveryDocumentBuilder(prodRepository, validator, doc);

            var document = builder.Build();
            return Ok(document);
        }

        [HttpGet("new/{guid}/line")]
        public IActionResult GetDocumentLines(string guid, [FromServices]ICache cache,
            [FromServices]IRepository<Product> prodRepository, [FromServices]IValidator<IDocAddLineCmd> validator)
        {
            var document = cache.GetValue(guid) as DeliveryDocument;
            if(document == null)
                return NotFound();

            var builder = new DeliveryDocumentBuilder(prodRepository, validator, document);

            return Ok(builder.Lines);
        }

        [HttpPut("new/{guid}/line")]
        public IActionResult UpdateLine(string guid, [FromServices]ICache cache, [FromBody]DeliveryDocumentLine line,
            [FromServices]IRepository<Product> prodRepository, [FromServices]IValidator<IDocAddLineCmd> validator)
        {
            var errors = new List<ValidationResult>();
            var doc = cache.GetValue(guid) as DeliveryDocument;
            if(doc == null)
                return NotFound();
                
            var builder = new DeliveryDocumentBuilder(prodRepository, validator, doc);
            builder.OnUpdateLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };
            builder.UpdateLine(line);

            if(errors.Count > 0)
                return BadRequest(errors);

            cache.SetValue(guid, builder.Build());
            return Ok(builder.Lines);
        }  

        [HttpDelete("new/{guid}/line")]
        public IActionResult DeleteLine(string guid, [FromServices]ICache cache, [FromBody]DeliveryDocumentLine line,
            [FromServices]IRepository<Product> prodRepository, [FromServices]IValidator<IDocAddLineCmd> validator)
        {
            var doc = cache.GetValue(guid) as DeliveryDocument;
            if(doc == null)
                return NotFound();

            var builder = new DeliveryDocumentBuilder(prodRepository, validator, doc);
            builder.RemoveLine(line);

            var document = builder.Build();
            cache.SetValue(guid, document);

            return Ok(builder.Lines);
        }     

        [HttpPost("new/{guid}")]
        public IActionResult Register(string guid, [FromServices]ICache cache, IDocumentRegistrator<DeliveryDocument> registrator)
        {
            var doc = cache.GetValue(guid) as DeliveryDocument;
            if(doc == null)
                return NotFound();

            registrator.Register(doc);

            return Ok(doc);
        }
    }
}