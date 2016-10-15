using System;
using Wrhs.Core.Search;

namespace Wrhs.Products.Search
{
    public class ProductSearchCriteria : SearchCriteria<Product>
    {
        public override event EventHandler<Func<Product, bool>> OnBuildQuery;

        public ProductSearchCriteria WhereName(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (Product prod)=>{ return prod.Name == value; });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (Product prod)=>{ return prod.Name.Contains(value); });

            return this;
        }

        public ProductSearchCriteria WhereCode(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (Product prod)=>{ return prod.Code == value; });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (Product prod)=>{ return prod.Code.Contains(value); });

            return this;
        }
    }
}