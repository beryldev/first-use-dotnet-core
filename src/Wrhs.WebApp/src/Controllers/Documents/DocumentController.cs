using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.WebApp.Controllers.Documents
{
    public abstract class DocumentController : BaseController 
    {
        protected readonly IDocumentService documentSrv;

        public DocumentController(IDocumentService documentSrv)
        {
            this.documentSrv = documentSrv;
        }

        [HttpGet]
        public ResultPage<Document> Get(string fullNumber="", int page=1, int pageSize=20)
        {
            page = page < 1 ? 1 : page;
            pageSize = (pageSize < 1 || pageSize > 100) ? 20 : pageSize; 

            var filter = new Dictionary<string, object>
            {
                {"fullNumber", fullNumber}
            };

            return documentSrv.FilterDocuments(GetDocumentType(), filter, page, pageSize);
        }

        protected abstract DocumentType GetDocumentType();

        // [HttpGet]
        // public IPaginateResult<TDoc> Get(string fullNumber="", DateTime? issueDate = null,
        //     int page = 1, int perPage = 10)
        // {
        //     var paginator = new Paginator<TDoc>();
        //     var search = new ResourceSearch<TDoc>(docRepo, paginator,
        //         new DocumentSearchCriteriaFactory<TDoc>());

        //     var criteria = (DocumentSearchCriteria<TDoc>) search.MakeCriteria();
        //     criteria.Page = page;
        //     criteria.PerPage = perPage;

        //     if(!String.IsNullOrWhiteSpace(fullNumber))
        //         criteria.WhereFullNumber(Condition.Contains, fullNumber);

        //     if(issueDate != null)
        //         criteria.WhereIssueDate(Condition.Equal, issueDate.Value);
           
        //     return search.Exec(criteria);
        // }
    }
}