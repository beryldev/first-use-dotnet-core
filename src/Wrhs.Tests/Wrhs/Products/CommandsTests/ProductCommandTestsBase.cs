using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Tests.Products
{
    public abstract class ProductCommandTestsBase
    {
        protected List<Product> MakeProductList()
        {
            var items = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Code = "PROD1",
                    Name = "Product 1",
                    Description = "Some desc"
                }
            };

            return items;
        }
        
        protected IRepository<Product> MakeProductRepository(List<Product> items)
        {
            var repo = RepositoryFactory<Product>.Make();
            foreach(var item in items)
                repo.Save(item);
            
            return repo;
        }
    }
}