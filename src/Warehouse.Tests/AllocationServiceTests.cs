using System;
using Moq;
using NUnit.Framework;
using Warehouse.Operations;

namespace Warehouse.Tests
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
            var allocation = new Allocation()
            {
                Quantity = 1,
                ProductCode = "CODE",
                Location = location
            };

            var service = MakeService();

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
            var allocation = new Allocation
            {
                Quantity = 1,
                Location = "LOC-001-01",
                ProductCode = code
            };

            var service = MakeService();

            Assert.Throws<ArgumentException>(()=>{
                service.RegisterAllocation(allocation);
            });
        }

        protected AllocationService MakeService()
        {
            return new AllocationService();
        }
    }
}