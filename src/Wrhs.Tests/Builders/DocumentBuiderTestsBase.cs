using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Tests
{
    public abstract class DocumentBuilderTestsBase
    {
        protected IRepository<Product> MakeProductRepository()
        {
            return MakeProductRepository(20);
        }

        protected IRepository<Product> MakeProductRepository(int itemsCount)
        {
            var repo = RepositoryFactory<Product>.Make();
            var items = MakeItems(itemsCount);
            foreach(var item in items)
                repo.Save(item);
           
           return repo;
        }

        protected List<Product> MakeItems(int count)
        {
            var items = new List<Product>();
            for(var i=0; i<count; i++)
            {
                items.Add(new Product
                {
                    Id = i+1,
                    Code = $"PROD{i+1}",
                    Name = $"Product {i+1}",
                    EAN = $"000{i+1}"
                });
            }

            return items;
        }
    }
}