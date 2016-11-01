using System.Collections.Generic;

namespace Wrhs.Core.Search.Interfaces
{
    public interface IPaginateResult<T>
    {
        IEnumerable<T> Items { get; set; }

        int Page { get; set; }

        int PerPage { get; set; }

        int Total { get; set; }

        int TotalPages { get; }
    }
}