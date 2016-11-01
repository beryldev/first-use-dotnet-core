using System;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Products
{
    public class ProductSearchCriteria : SearchCriteria<Product>
    {
        public override event EventHandler<Func<Product, bool>> OnBuildQuery;

        public ProductSearchCriteria WhereName(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (Product prod)=>{ return prod.Name.ToLower() == value.ToLower(); });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (Product prod)=>{ return prod.Name.ToLower().Contains(value.ToLower()); });

            return this;
        }

        public ProductSearchCriteria WhereCode(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (Product prod)=>{ return prod.Code == value.ToUpper(); });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (Product prod)=>{ return prod.Code.Contains(value.ToUpper()); });

            return this;
        }

         public ProductSearchCriteria WhereEAN(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (Product prod)=>{ return prod.EAN == value; });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (Product prod)=>{ return prod.EAN.Contains(value); });

            return this;
        }
    }


    public class ProductSearchCriteriaFactory : ISearchCriteriaFactory<Product>
    {
        public ISearchCriteria<Product> Create()
        {
            return new ProductSearchCriteria();
        }
    }
}