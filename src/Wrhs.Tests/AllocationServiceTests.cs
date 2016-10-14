using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wrhs.Products.Operations;

namespace Wrhs.Products.Tests
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
            var mock = new Mock<IRepository<Allocation>>();
            var allocation = MakeAllocation("PROD1", 1, location);
            var service = MakeService(mock.Object);

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
            var items = new List<Allocation>();
            var mock = PrepareAllocRepoMock(items);
            var allocation = MakeAllocation(code);
            var service = MakeService(mock.Object);

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
            var mock = new Mock<IRepository<Allocation>>();
            var allocation = MakeDeallocation("PROD1", -1, location);
            var service = MakeService(mock.Object);

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
            var items = new List<Allocation>();
            var mock = PrepareAllocRepoMock(items);
            var allocation = MakeDeallocation(code);
            var service = MakeService(mock.Object);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterDeallocation(allocation);
            });
        }

        [Test]
        public void RegisterSaveAllocationInRepository()
        {
            var items = new List<Allocation>();
            var mock = PrepareAllocRepoMock(items);
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(mock.Object);

            service.RegisterAllocation(allocation);

            mock.Verify(m=>m.Save(allocation), Times.Once());
            CollectionAssert.Contains(items, allocation);
        }

        [Test]
        public void RegisterSaveDeallocationInRepository()
        {
            var alloc = MakeAllocation("PROD1");
            var items = new List<Allocation>{alloc};
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);
            var deallocation = MakeDeallocation("PROD1");

            service.RegisterDeallocation(deallocation);

            mock.Verify(m=>m.Save(deallocation), Times.Once());
            CollectionAssert.Contains(items, deallocation);
        }

        [Test]
        public void CantRegisterAllocationWithNegativeQuantity()
        {
            var items = new List<Allocation>();
            var mock = PrepareAllocRepoMock(items);
            var allocation = MakeDeallocation("PROD1");
            var service = MakeService(mock.Object);

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterAllocation(allocation);
            });
            Assert.AreEqual(0, items.Count);
        }

        [Test]
        public void CantRegisterDeallocationWithPositiveQuantity()
        {
            var items = new List<Allocation>();
            var mock = PrepareAllocRepoMock(items);
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(mock.Object);

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterDeallocation(allocation);
            });
            Assert.AreEqual(0, items.Count);
        }

        [Test]
        public void GetAllocationsReturnAllocations()
        {
            var items = new List<Allocation>{MakeAllocation("PROD1"), MakeAllocation("PROD2")};
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);

            var result = service.GetAllocations();

            Assert.AreEqual(items.Count, result.Count());
            CollectionAssert.AreEquivalent(items, result);
        }

        [Test]
        public void GetAllocationsReturnRegisteredAllocation()
        {
            var items = new List<Allocation>{MakeAllocation("PROD1"), MakeAllocation("PROD2")};
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);
            var allocation = MakeAllocation("PROD3");

            service.RegisterAllocation(allocation);
            var result = service.GetAllocations();

            CollectionAssert.Contains(result, allocation);
        }

        [Test]
        public void CantRegisterDeallocationWhenResourceNotExistsOnLocation()
        {
            var items = new List<Allocation>{MakeAllocation("PROD1")};
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);
            var dealloc = MakeDeallocation("PROD1");
            dealloc.Location = "LOC-001-04";

            Assert.Throws<InvalidOperationException>(()=>
            {
                service.RegisterDeallocation(dealloc);
            });
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        public void TransactionalRegisterAllocations()
        {
            var items = new List<Allocation>{ MakeAllocation("PROD1") };
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            service.CommitTransaction();
            
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(3, items.Sum(item=>item.Quantity));
        }

        [Test]
        public void TransactionalRegisterDeallocations()
        {
            var items = new List<Allocation>{ MakeAllocation("PROD1") };
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);

            service.BeginTransaction();
            service.RegisterDeallocation(MakeAllocation("PROD1", -1));
            service.CommitTransaction();
            
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(0, items.Sum(item=>item.Quantity));
        }

        [Test]
        public void UncomittedTransactionNotChangeRepo()
        {
            var items = new List<Allocation>{ MakeAllocation("PROD1") };
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1, items.Sum(item=>item.Quantity));
        }

        [Test]
        public void RolledbackTransactionNotChangeRepo()
        {
            var items = new List<Allocation>{ MakeAllocation("PROD1") };
            var mock = PrepareAllocRepoMock(items);
            var service = MakeService(mock.Object);

            service.BeginTransaction();
            service.RegisterAllocation(MakeAllocation("PROD1", 2));
            service.RollbackTransaction();
            
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(1, items.Sum(item=>item.Quantity));
        }

        protected Mock<IRepository<Allocation>> PrepareAllocRepoMock(List<Allocation> items)
        {
            var mock = new Mock<IRepository<Allocation>>();

            mock.Setup(m=>m.Save(It.IsAny<Allocation>()))
                .Callback((Allocation alloc)=>{ items.Add(alloc); });

            mock.Setup(m=>m.Get())
                .Returns(items);

            return mock;
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