using System;
using System.Linq;
using Wrhs.Operations;
using Wrhs.Core;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests
{
    public class AllocationServiceTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ThrowsExceptionWhenTryRegisterAllocationWithEmptyLocationName(string location)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation("PROD1", 1, location);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterAllocation(allocation);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ThrowsExceptionWhenTryRegisterAllocationWithoutProductCode(string code)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation(code);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterAllocation(allocation);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ThrowsExceptionWhenTryRegisterDeallocationWithEmptyLocationName(string location)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeDeallocation("PROD1", -1, location);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterDeallocation(allocation);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void ThrowsExceptionWhenTryRegisterDeallocationWithoutProductCode(string code)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeDeallocation(code);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterDeallocation(allocation);
            });
        }

        [Theory]
        public void RegisterSaveAllocationInRepository()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(repo);

            service.RegisterAllocation(allocation);

            Assert.Contains(allocation, repo.Get().ToArray());
        }

        [Fact]
        public void RegisterSaveDeallocationInRepository()
        {
            var alloc = MakeAllocation("PROD1");
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(alloc);
            var service = MakeService(repo);
            var deallocation = MakeDeallocation("PROD1");

            service.RegisterDeallocation(deallocation);

            Assert.Contains( deallocation, repo.Get().ToArray());
        }

        [Fact]
        public void CantRegisterAllocationWithNegativeQuantity()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeDeallocation("PROD1");
            var service = MakeService(repo);

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterAllocation(allocation);
            });

            Assert.Empty(repo.Get().ToArray());
        }

        [Fact]
        public void CantRegisterDeallocationWithPositiveQuantity()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(repo);

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterDeallocation(allocation);
            });

            Assert.Empty(repo.Get().ToArray());
        }

        [Fact]
        public void GetAllocationsReturnAllocations()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            repo.Save(MakeAllocation("PROD2"));
            var service = MakeService(repo);

            var result = service.GetAllocations();

            Assert.Equal(repo.Get().Count(), result.Count());
            Assert.Equal(repo.Get().ToArray(), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ShouldReturnAllocationsByProductId(int productId)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1", id: 1));
            repo.Save(MakeAllocation("PROD2", id: 2));
            var service = MakeService(repo);

            var result = service.GetAllocations(productId);

            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("PROD1")]
        [InlineData("PROD2")]
        public void ShouldReturnAllocationsByProductCode(string code)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1", id: 1));
            repo.Save(MakeAllocation("PROD2", id: 2));
            var service = MakeService(repo);

            var result = service.GetAllocations(code);

            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAllocationsReturnRegisteredAllocation()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            repo.Save(MakeAllocation("PROD2"));
            var service = MakeService(repo);
            var allocation = MakeAllocation("PROD3");

            service.RegisterAllocation(allocation);
            var result = service.GetAllocations();

            Assert.Contains(allocation, result);
        }

        [Fact]
        public void CantRegisterDeallocationWhenResourceNotExistsOnLocation()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);
            var dealloc = MakeDeallocation("PROD1");
            dealloc.Location = "LOC-001-04";

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterDeallocation(dealloc);
            });
            Assert.Equal(1, repo.Get().Count());
        }

        [Fact]
        public void TransactionalRegisterAllocations()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            service.CommitTransaction();
            
            Assert.Equal(2, repo.Get().Count());
            Assert.Equal(3, repo.Get().Sum(item=>item.Quantity));
        }

        [Fact]
        public void TransactionalRegisterDeallocations()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterDeallocation(MakeAllocation("PROD1", -1));
            service.CommitTransaction();
            
            Assert.Equal(2, repo.Get().Count());
            Assert.Equal(0, repo.Get().Sum(item=>item.Quantity));
        }

        [Fact]
        public void UncomittedTransactionNotChangeRepo()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            
            Assert.Equal(1, repo.Get().Count());
            Assert.Equal(1, repo.Get().Sum(item=>item.Quantity));
        }

        [Fact]
        public void RolledbackTransactionNotChangeRepo()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            service.RollbackTransaction();
            
            Assert.Equal(1, repo.Get().Count());
            Assert.Equal(1, repo.Get().Sum(item=>item.Quantity));
        }

        protected Allocation MakeAllocation(string code, decimal quantity=1, string location="LOC-001-01", int id=1)
        {
            return new Allocation
            {
                Quantity = quantity,
                Location = location,
                Product = new Product{ Id = id, Code = code }
            };
        }

        protected Allocation MakeDeallocation(string code, decimal quantity=-1, string location="LOC-001-01")
        {
            return new Allocation
            {
                Quantity = quantity,
                Location = location,
                Product = new Product{ Code = code }
            };
        }

        protected AllocationService MakeService(IRepository<Allocation> repo)
        {
            return new AllocationService(repo);
        }
    }
}