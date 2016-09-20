using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;

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
                ProductCode = code
            };
        }

        protected AllocationService MakeService(IRepository<Allocation> repo)
        {
            return new AllocationService(repo);
        }
    }
}