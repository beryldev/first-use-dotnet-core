using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Core.Search
{
    public class Paginator<T> : IPaginator<T>
    {
        const int DEFAULT_PAGE = 1;

        const int DEFAULT_PAGE_SIZE = 10;

        public IPaginateResult<T> Paginate(IEnumerable<T> items)
        {
            return Paginate(items, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
        }

        public IPaginateResult<T> Paginate(IEnumerable<T> items, int page)
        {
            return Paginate(items, page, DEFAULT_PAGE_SIZE);
        }

        public IPaginateResult<T> Paginate(IEnumerable<T> items, int page, int pageSize)
        {
            if(page < 1)
                throw new ArgumentException("Invalid page number. Must be greater than zero.");

            if(pageSize < 1)
                throw new ArgumentException("Invalid page size. Must be greater than zero.");


            return new PaginateResult<T>
            {
                Items = items.Skip((page-1)*pageSize).Take(pageSize).ToArray(),
                Total = items.Count(),
                Page = page,
                PerPage = pageSize
            };
        }
    }
}