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
            cache.SetValue(guid, builder);

            return guid;
        }

        [HttpPost("new/{guid}/line")]
        public IActionResult AddLine(string guid, [FromBody]DocAddLineCmd cmd, [FromServices]ICache cache)
        {
            var errors = new List<ValidationResult>();
            var builder = cache.GetValue(guid) as DeliveryDocumentBuilder;

            if(builder == null)
                return NotFound();

            builder.OnAddLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };

            builder.AddLine(cmd);

            if(errors.Count > 0)
                return BadRequest(errors);

            cache.SetValue(guid, builder);
            return Ok();
        }

        [HttpGet("new/{guid}")]
        public IActionResult GetDocument(string guid, [FromServices]ICache cache)
        {
            var builder = cache.GetValue(guid) as DeliveryDocumentBuilder;

            if(builder == null)
                return NotFound();

            var document = builder.Build();
            return Ok(document);
        }

        [HttpGet("new/{guid}/line")]
        public IActionResult GetDocumentLines(string guid, [FromServices]ICache cache)
        {
            var builder = cache.GetValue(guid) as DeliveryDocumentBuilder;

            if(builder == null)
                return NotFound();

            return Ok(builder.Lines);
        }

        [HttpPut("new/{guid}/line")]
        public IActionResult UpdateLine(string guid, [FromServices]ICache cache, [FromBody]DeliveryDocumentLine line)
        {
            var errors = new List<ValidationResult>();
            var builder = cache.GetValue(guid) as DeliveryDocumentBuilder;
            if(builder == null)
                return NotFound();
                
            builder.OnUpdateLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };
            builder.UpdateLine(line);

            if(errors.Count > 0)
                return BadRequest(errors);

            return Ok();
        }  

        [HttpDelete("new/{guid}/line")]
        public IActionResult DeleteLine(string guid, [FromServices]ICache cache, [FromBody]DeliveryDocumentLine line)
        {
            var builder = cache.GetValue(guid) as DeliveryDocumentBuilder;
            if(builder == null)
                return NotFound();

            builder.RemoveLine(line);

            return Ok();
        }     
    }
}