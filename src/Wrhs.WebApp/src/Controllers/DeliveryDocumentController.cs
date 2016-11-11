using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Operations.Delivery;

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
                criteria.WhereIssueDate(Condition.Equal, issueDate.Value);
           
            return search.Exec(criteria);
        }
    }
}