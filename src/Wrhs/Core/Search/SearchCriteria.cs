using System;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Core.Search
{
    public abstract class SearchCriteria<T> : ISearchCriteria<T> where T : IEntity
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;

        public abstract event EventHandler<Func<T, bool>> OnBuildQuery;
    }

    public enum Condition
    {
        Equal = 1,
        
        Contains = 2
    }
}