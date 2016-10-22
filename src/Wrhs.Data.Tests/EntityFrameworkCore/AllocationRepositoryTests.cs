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
        public void ShouldSaveAllocation()
        {
           
            var product = CreateProduct(context);
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
        public void ShouldRetriveAllocationById()
        {
            var product = CreateProduct(context);
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
            var product = CreateProduct(context);
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

        [Fact]
        public void ShouldUpdateAllocation()
        {
            var product = CreateProduct(context);
            var repo = new AllocationRepository(context);

            var allocation = new Allocation
            {
                Product = product,
                Location = "LOC-001-01",
                Quantity = 100
            };

            repo.Save(allocation);
            var id = allocation.Id;
            allocation.Location = "LOC-002-02";
            allocation.Quantity = 10;
            repo.Update(allocation);
            var updated = repo.Get().First();

            Assert.Equal("LOC-002-02", updated.Location);
            Assert.Equal(10, updated.Quantity);
        }

        [Fact]
        public void ShouldDeleteAllocation()
        {
            var product = CreateProduct(context);
            var repo = new AllocationRepository(context);

            var allocation = new Allocation
            {
                Product = product,
                Location = "LOC-001-01",
                Quantity = 100
            };

            repo.Save(allocation);
            repo.Delete(allocation);

            Assert.Empty(repo.Get());
        }
    }
}