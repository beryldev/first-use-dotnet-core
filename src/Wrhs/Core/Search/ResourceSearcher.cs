using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Core.Search
{
    public abstract class ResourceSearcher<T> : IResourceSearcher<T> where T : IEntity
    {
        IEnumerable<T> items;

        IRepository<T> repository;

        IPaginator<T> paginator;

        public ResourceSearcher(IRepository<T> repository, IPaginator<T> paginator)
        {
            this.paginator = paginator;
            this.repository = repository;
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
            var criteria = FactoryCriteria();
            criteria.OnBuildQuery += OnBuildQuery;

            return criteria;
        }

        protected abstract ISearchCriteria<T> FactoryCriteria();

        protected virtual void OnBuildQuery(object sender, Func<T, bool> c)
        {
            items = items.Where(c);
        }
    }
}