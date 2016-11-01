using System;
using System.Collections.Generic;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Core.Search
{
    public class PaginateResult<T> : IPaginateResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Page { get; set; }

        public int PerPage { get; set; }

        public int Total { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)Total / PerPage);
            }
        }
    }
}