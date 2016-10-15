using System.Collections.Generic;

namespace Wrhs.Core.Search.Interfaces
{
    public interface IPaginator<T>
    {
        IPaginateResult<T> Paginate(IEnumerable<T> items);

        IPaginateResult<T> Paginate(IEnumerable<T> items, int page);

        IPaginateResult<T> Paginate(IEnumerable<T> items, int page, int pageSize);

    }
}