using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Orders;

namespace Wrhs.Tests
{
    [TestFixture]
    public class WarehouseTests
    {


        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(99)]
        public void WarehouseCalculateStocks(int total)
        {
            var items = PrepareAllocations(total);
            var warehouse = PrepareWarehouse(SetupAllocationRepository(items));

            var stocks = warehouse.CalculateStocks();

            Assert.AreEqual(total, stocks.Sum(item=>item.Quantity));
        }

        public void CalculateStocksByProductCode()
        {
            var warehouse = PrepareWarehouse(SetupAllocationRepository());
            var stocks = warehouse.CalculateStocks("SPROD");

            Assert.AreEqual(0, stocks.Sum(item=>item.Quantity));
        }

        //[Test]
        public void ProcessDeliveryOperationChangesStocks()
        {
            var allocRepo = SetupAllocationRepository();
            
            var operation = PrepareDeliveryOperation(allocRepo);
            var warehouse = PrepareWarehouse(allocRepo);

            var stocksBefore = warehouse.CalculateStocks("SPROD");
            Assert.AreEqual(0, stocksBefore.Sum(item=>item.Quantity));

            warehouse.ProcessOperation(operation);

            var stocks = warehouse.CalculateStocks("SPROD");
            Assert.AreEqual(5, stocks.Sum(item=>item.Quantity));
        }

        protected IRepository<Allocation> SetupAllocationRepository()
        {
            return SetupAllocationRepository(new List<Allocation>());
        }

        protected List<Allocation> PrepareAllocations(int total)
        {
            return Enumerable.Range(0, total)
            .Select(i=>new Allocation()
            {
                ProductCode = "PROD1",
                Quantity = 1,
                Location = "LOC-001-01"
            }).ToList();
        }

        protected IRepository<Allocation> SetupAllocationRepository(List<Allocation> items)
        {
            var mock = new Mock<IRepository<Allocation>>();
            
            mock.Setup(m=>m.Save(It.IsAny<Allocation>()))
                .Callback((Allocation allocation)=>{
                    items.Add(allocation);
                });

            mock.Setup(m=>m.Get())
                .Returns(items.ToList());

            return mock.Object;
        }

        protected Warehouse PrepareWarehouse(IRepository<Allocation> allocRepo)
        {
            return new Warehouse(allocRepo);
        }

        protected Order PrepareOrder()
        {
            var order = new Order();
            order.Lines.Add(new OrderLine
            {
                ProductName = "Some product",
                ProductCode = "SPROD",
                EAN = "1234567890",
                SKU = "P12345",
                Quantity = 5,
                Remarks = ""
            });

            return order;
        }

        protected DeliveryOperation PrepareDeliveryOperation(IRepository<Allocation> allocRepo)
        {
            var allocSrvMock = new Mock<IAllocationService>();
            allocSrvMock.Setup(m=>m.RegisterAllocation(It.IsAny<Allocation>()))
                .Callback((Allocation allocation)=>{
                    allocRepo.Save(allocation);
                });

            var operation = new DeliveryOperation(allocSrvMock.Object);
            var order = PrepareOrder();
            operation.SetBaseDocument(order);
            operation.AllocateItem((OrderLine)order.Lines[0], 5, "LOC-001-01");

            return operation;
        }
    }
}