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

        public ResultPage<Product> FilterProducts(ProductFilter filter)
        {
            return FilterProducts(filter, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Product> FilterProducts(ProductFilter filter, int page, int pageSize)
        {
            var query = context.Products
                .Where(x => string.IsNullOrWhiteSpace(filter.Name) || x.Name.ToUpper().Contains(filter.Name.ToUpper()))
                .Where(x => string.IsNullOrWhiteSpace(filter.Code) || x.Code.ToUpper().Contains(filter.Code.ToUpper()))
                .Where(x => string.IsNullOrWhiteSpace(filter.Ean) || x.Ean.ToUpper().Contains(filter.Ean.ToUpper()));

            return PaginateQuery(query, page, pageSize);
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
    }
}