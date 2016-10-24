using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Core.Search
{
    public class ResourceSearch<T> : IResourceSearch<T> where T : IEntity
    {
        IEnumerable<T> items;

        IRepository<T> repository;

        IPaginator<T> paginator;

        ISearchCriteriaFactory<T> criteriaFactory;

        public ResourceSearch(IRepository<T> repository, IPaginator<T> paginator, ISearchCriteriaFactory<T> criteriaFactory)
        {
            this.paginator = paginator;
            this.repository = repository;
            this.criteriaFactory = criteriaFactory;

            items = this.repository.Get();
        }

        public IPaginateResult<T> Exec(ISearchCriteria<T> criteria)
        {
            var result = paginator.Paginate(items, criteria.Page, criteria.PerPage);

            items = repository.Get();

            return result;
        }

        public ISearchCriteria<T> MakeCriteria()
        {
            var criteria = criteriaFactory.Create();
            criteria.OnBuildQuery += OnBuildQuery;

            return criteria;
        }

        protected virtual void OnBuildQuery(object sender, Func<T, bool> c)
        {
            items = items.Where(c);
        }
    }
}