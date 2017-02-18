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

        public void Delete(Product product)
        {
            if(product == null)
                return;
                
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public int Save(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product.Id;
        }

        public void Update(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
        }

        protected override IQueryable<Product> GetQuery()
        {
            return context.Products;
        }

        protected override Dictionary<string, Func<Product, object, bool>> GetFilterMapping()
        {
            var mapping = new Dictionary<string, Func<Product, object, bool>>
            {
                {"name", (Product p, object val) => p.Name != null && p.Name.ToLower().Contains((val as string).ToLower()) },
                {"code", (Product p, object val) => p.Code != null && p.Code.ToLower().Contains((val as string).ToLower()) },
                {"ean", (Product p, object val) => p.Ean != null && p.Ean.ToLower().Contains((val as string).ToLower()) }
            };

            return mapping;
        }
    }
}