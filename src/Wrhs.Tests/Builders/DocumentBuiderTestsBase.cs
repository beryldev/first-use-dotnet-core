using System.Collections.Generic;
using Moq;
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

        protected IWarehouse MakeWarehouse(IRepository<Product> repo)
        {
            var warehouseMock = new Mock<IWarehouse>();
            warehouseMock.Setup(m=>m.CalculateStocks(It.IsAny<string>()))
                .Returns(new List<Stock>
                {
                    new Stock { Product=repo.GetById(5), Location="LOC-001-01", Quantity=5},
                    new Stock { Product=repo.GetById(8), Location="LOC-001-01", Quantity=24},
                    new Stock { Product=repo.GetById(5), Location="LOC-001-02", Quantity=15}
                });

            return warehouseMock.Object;
        }
    }
}