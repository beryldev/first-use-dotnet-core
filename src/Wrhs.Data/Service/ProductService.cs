using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Services;

namespace Wrhs.Data.Service
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(WrhsContext context) : base(context)
        {

        }

        public bool CheckProductExists(int id)
        {
            return context.Products.Any(p => p.Id == id);
        }

        public bool CheckProductExistsByCode(string code)
        {
            return context.Products.Any(p => p.Code == code);
        }

        public bool CheckProductExistsByName(string name)
        {
            return context.Products.Any(p => p.Name == name);
        }

        public Product GetProductByCode(string code)
        {
            return context.Products.FirstOrDefault(p => p.Code == code);
        }

        public Product GetProductById(int id)
        {
            return context.Products.Find(id);
        }

        public Product GetProductByName(string name)
        {
            return context.Products.FirstOrDefault(p => p.Name == name);
        }

        public ResultPage<Product> FilterProducts(Dictionary<string, object> filter)
        {
            return FilterProducts(filter, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Product> FilterProducts(Dictionary<string, object> filter, int page, int pageSize)
        {
            var query = context.Products.AsQueryable();
            
            return Filter(query, filter, page, pageSize);
        }

        protected override IQueryable<Product> GetQuery()
        {
            return context.Products;
        }

        protected override Dictionary<string, Func<Product, object, bool>> GetFilterMapping()
        {
            var mapping = new Dictionary<string, Func<Product, object, bool>>
            {
                {"name", (Product p, object val) => p.Name != null && p.Name.Contains(val as string) },
                {"code", (Product p, object val) => p.Code != null && p.Code.Contains(val as string) },
            };

            return mapping;
        }
    }
}