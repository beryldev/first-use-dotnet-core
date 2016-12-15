using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.WebApp.Controllers.Documents
{
    public abstract class DocumentController : BaseController 
    {
        IDocumentService documentSrv;

        public DocumentController(IDocumentService documentSrv)
        {
            this.documentSrv = documentSrv;
        }

        public abstract ResultPage<Document> Get();

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