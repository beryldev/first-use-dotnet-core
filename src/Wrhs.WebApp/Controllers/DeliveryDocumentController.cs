using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Wrhs.Products;

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
            [FromServices]IValidator<DocAddLineCmd> validator)
        {
            var builder = new DeliveryDocumentBuilder(prodRepository, validator);
            var guid = System.Guid.NewGuid().ToString();
            cache.SetValue(guid, builder);

            return guid;
        }

        [HttpPost("{guid}/Line")]
        public IActionResult AddLine(string guid, DocAddLineCmd cmd, [FromServices]ICache cache)
        {
            var errors = new List<ValidationResult>();
            var builder = cache.GetValue(guid) as DeliveryDocumentBuilder;
            builder.OnAddLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };

            builder.AddLine(cmd);

            if(errors.Count > 0)
                return BadRequest(errors);

            return Ok();
        }
    }
}