using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class AllocationServiceTests
    {
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ThrowsExceptionWhenTryRegisterAllocationWithEmptyLocationName(string location)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation("PROD1", 1, location);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterAllocation(allocation);
            });
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ThrowsExceptionWhenTryRegisterAllocationWithoutProductCode(string code)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation(code);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterAllocation(allocation);
            });
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ThrowsExceptionWhenTryRegisterDeallocationWithEmptyLocationName(string location)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeDeallocation("PROD1", -1, location);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterDeallocation(allocation);
            });
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ThrowsExceptionWhenTryRegisterDeallocationWithoutProductCode(string code)
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeDeallocation(code);
            var service = MakeService(repo);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterDeallocation(allocation);
            });
        }

        [Test]
        public void RegisterSaveAllocationInRepository()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(repo);

            service.RegisterAllocation(allocation);

            CollectionAssert.Contains(repo.Get().ToArray(), allocation);
        }

        [Test]
        public void RegisterSaveDeallocationInRepository()
        {
            var alloc = MakeAllocation("PROD1");
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(alloc);
            var service = MakeService(repo);
            var deallocation = MakeDeallocation("PROD1");

            service.RegisterDeallocation(deallocation);

            CollectionAssert.Contains(repo.Get().ToArray(), deallocation);
        }

        [Test]
        public void CantRegisterAllocationWithNegativeQuantity()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeDeallocation("PROD1");
            var service = MakeService(repo);

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterAllocation(allocation);
            });

            CollectionAssert.IsEmpty(repo.Get().ToArray());
        }

        [Test]
        public void CantRegisterDeallocationWithPositiveQuantity()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(repo);

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterDeallocation(allocation);
            });

            CollectionAssert.IsEmpty(repo.Get().ToArray());
        }

        [Test]
        public void GetAllocationsReturnAllocations()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            repo.Save(MakeAllocation("PROD2"));
            var service = MakeService(repo);

            var result = service.GetAllocations();

            Assert.AreEqual(repo.Get().Count(), result.Count());
            CollectionAssert.AreEquivalent(repo.Get().ToArray(), result);
        }

        [Test]
        public void GetAllocationsReturnRegisteredAllocation()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            repo.Save(MakeAllocation("PROD2"));
            var service = MakeService(repo);
            var allocation = MakeAllocation("PROD3");

            service.RegisterAllocation(allocation);
            var result = service.GetAllocations();

            CollectionAssert.Contains(result, allocation);
        }

        [Test]
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
            Assert.AreEqual(1, repo.Get().Count());
        }

        [Test]
        public void TransactionalRegisterAllocations()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            service.CommitTransaction();
            
            Assert.AreEqual(2, repo.Get().Count());
            Assert.AreEqual(3, repo.Get().Sum(item=>item.Quantity));
        }

        [Test]
        public void TransactionalRegisterDeallocations()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterDeallocation(MakeAllocation("PROD1", -1));
            service.CommitTransaction();
            
            Assert.AreEqual(2, repo.Get().Count());
            Assert.AreEqual(0, repo.Get().Sum(item=>item.Quantity));
        }

        [Test]
        public void UncomittedTransactionNotChangeRepo()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            
            Assert.AreEqual(1, repo.Get().Count());
            Assert.AreEqual(1, repo.Get().Sum(item=>item.Quantity));
        }

        [Test]
        public void RolledbackTransactionNotChangeRepo()
        {
            var repo = RepositoryFactory<Allocation>.Make();
            repo.Save(MakeAllocation("PROD1"));
            var service = MakeService(repo);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            service.RollbackTransaction();
            
            Assert.AreEqual(1, repo.Get().Count());
            Assert.AreEqual(1, repo.Get().Sum(item=>item.Quantity));
        }

        protected Allocation MakeAllocation(string code, decimal quantity=1, string location="LOC-001-01")
        {
            return new Allocation
            {
                Quantity = quantity,
                Location = location,
                Product = new Product{ Code = code }
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