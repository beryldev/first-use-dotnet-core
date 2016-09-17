using System;
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
            var mock = new Mock<IRepository<Allocation>>();
            var allocation = MakeAllocation(code);
            var service = MakeService(mock.Object);

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterAllocation(allocation);
            });
        }

        [Test]
        public void RegisterSaveAllocationInRepository()
        {
            var mock = new Mock<IRepository<Allocation>>();
            var allocation = MakeAllocation("PROD1");
            var service = MakeService(mock.Object);

            service.RegisterAllocation(allocation);

            mock.Verify(m=>m.Save(allocation), Times.Once());
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