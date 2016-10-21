using System.Linq;
using Wrhs.Data.Repository;
using Wrhs.Operations;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests
{
    public class AllocationRepositoryTests : TestsBase
    {
        WrhsContext context;

        public AllocationRepositoryTests()
        {
            context = CreateContext();
        }

        [Fact]
        public void CanSaveAllocation()
        {
           
            var product = CreateProduct();
            var repo = new AllocationRepository(context);

            var allocation = new Allocation
            {
                Product = product,
                Location = "LOC-001-01",
                Quantity = 100
            };

            repo.Save(allocation);

            Assert.Equal(1, repo.Get().Count());
        }

        [Fact]
        public void CanRetriveAllocationById()
        {
            var product = CreateProduct();
            var repo = new AllocationRepository(context);

            var allocation = new Allocation
            {
                Product = product,
                Location = "LOC-001-01",
                Quantity = 100
            };

            repo.Save(allocation);
            var alloc = repo.GetById(allocation.Id);

            Assert.Equal("LOC-001-01", alloc.Location);
            Assert.Equal(100, alloc.Quantity);
        }

        [Fact]
        public void SavedAllocationHaveRelatedProduct()
        {
            var product = CreateProduct();
            var repo = new AllocationRepository(context);

            var allocation = new Allocation
            {
                Product = product,
                Location = "LOC-001-01",
                Quantity = 100
            };

            repo.Save(allocation);
            var alloc = repo.GetById(allocation.Id);

            Assert.Equal("PROD1", alloc.Product.Code);
        }

        Product CreateProduct()
        {
            var productRepo = new ProductRepository(context);
            var product = new Product
            {
                Code = "PROD1",
                Name = "Product 1",
                EAN = "1111",
                SKU = "111"
            };
            productRepo.Save(product);
            return product;
        }
    }
}