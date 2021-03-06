using System.Linq;
using Wrhs.Core;

namespace Wrhs.Data.Service
{
    public abstract class BaseService<T> where T : class
    {
        protected const int DEFAULT_PAGE = 1;
        protected const int DEFAULT_PAGE_SIZE = 20;

        protected readonly WrhsContext context;

        public BaseService(WrhsContext context)
        {
            this.context = context;
        }

        public ResultPage<T> Get()
        {
            return Get(DEFAULT_PAGE);
        }

        public ResultPage<T> Get(int page)
        {
            return Get(page, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<T> Get(int page, int pageSize)
        {
            var query = GetQuery();
            return PaginateQuery(query, page, pageSize);
        }

        protected abstract IQueryable<T> GetQuery();

        protected ResultPage<T> PaginateQuery(IQueryable<T> query, int page, int pageSize)
        {
            page = page > 0 ? page : 1;
            var items = query.Skip((page-1) * pageSize).Take(pageSize).ToList();
            return new ResultPage<T>(items, page, pageSize);  
        }
    }
}