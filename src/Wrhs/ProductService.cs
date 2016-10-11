
using System;
using System.Linq;

namespace Wrhs
{
    public class ProductService
    {
        IRepository<Product> repo;

        public ProductService(IRepository<Product> repo)
        {
            this.repo = repo;
        }

        public void CreateProduct(Product product)
        {
            ValidateProduct(product);

            repo.Save(product);
        }

        protected void ValidateProduct(Product product)
        {
            if(String.IsNullOrWhiteSpace(product.Code))
                throw new ArgumentException("Product code can't be empty");

            if(String.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name can't be empty");

            var result = repo.Get()
                .Where(item=>item.Code.Equals(product.Code) || item.EAN.Equals(product.EAN))
                .ToList();

            if(result.Where(item=>item.Code.Equals(product.Code)).Count() > 0)
                throw new ArgumentException("Product with this code already exists");

            if(result.Where(item=>item.EAN.Equals(product.EAN)).Count() > 0)
                throw new ArgumentException("Product with this EAN already exists");
        }
    }
}