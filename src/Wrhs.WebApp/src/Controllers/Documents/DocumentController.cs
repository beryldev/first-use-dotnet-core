using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Documents;

namespace Wrhs.WebApp.Controllers.Documents
{
    public abstract class DocumentController<TDoc, TLine> : BaseController 
        where TDoc : IEntity, ISearchableDocument, IDocument<TLine>
        where TLine : IEntity, IDocumentLine
    {
        IRepository<TDoc> docRepo;

        public DocumentController(IRepository<TDoc> docRepo)
        {
            this.docRepo = docRepo;
        }

        [HttpGet]
        public IPaginateResult<TDoc> Get(string fullNumber="", DateTime? issueDate = null,
            int page = 1, int perPage = 10)
        {
            var paginator = new Paginator<TDoc>();
            var search = new ResourceSearch<TDoc>(docRepo, paginator,
                new DocumentSearchCriteriaFactory<TDoc>());

            var criteria = (DocumentSearchCriteria<TDoc>) search.MakeCriteria();
            criteria.Page = page;
            criteria.PerPage = perPage;

            if(!String.IsNullOrWhiteSpace(fullNumber))
                criteria.WhereFullNumber(Condition.Contains, fullNumber);

            if(issueDate != null)
                criteria.WhereIssueDate(Condition.Equal, issueDate.Value);
           
            return search.Exec(criteria);
        }
    }
}