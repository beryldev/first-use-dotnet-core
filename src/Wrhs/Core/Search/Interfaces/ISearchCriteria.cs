using System;

namespace Wrhs.Core.Search.Interfaces
{
    public interface ISearchCriteria<T> where T : IEntity
    {
        int Page { get; set; }

        int PerPage { get; set; }

        event EventHandler<Func<T, bool>> OnBuildQuery;
    }
}