using System.Collections.Generic;

namespace Wrhs.Core
{
    public class ResultPage<T>
    {
         public IEnumerable<T> Items { get; }
         
         public int Page { get; }

         public int PageSize { get; }

         public ResultPage(IEnumerable<T> items, int page, int pageSize)
         {
             Items = items;
             Page = page;
             PageSize = pageSize;
         }
    }
}