using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Products.Search
{
    public class ProductSearch : ResourceSearcher<Product>
    {
        public ProductSearch(IRepository<Product> repository, IPaginator<Product> paginator) 
            : base(repository, paginator) { }

        protected override ISearchCriteria<Product> FactoryCriteria()
        {
            return new ProductSearchCriteria();
        }
    }
}