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

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(99)]
        public void CalculateStocksByProductCode(int total)
        {
            var items = PrepareAllocations(total);
            items.Add(new Allocation{ProductCode="SPROD", Quantity=5, Location="LOC-001-01"});
            var warehouse = PrepareWarehouse(SetupAllocationRepository(items));

            var stocks = warehouse.CalculateStocks("SPROD");

            Assert.AreEqual(5, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void ProcessDeliveryOperationChangesStocks()
        {
            var items = PrepareAllocations(0);
            var allocRepo = SetupAllocationRepository(items);
            var operation = PrepareDeliveryOperation(allocRepo);
            var warehouse = PrepareWarehouse(allocRepo);
            
            var stocksBefore = warehouse.CalculateStocks("SPROD");
            Assert.AreEqual(0, stocksBefore.Sum(item=>item.Quantity));

            warehouse.ProcessOperation(operation);

            var stocks = warehouse.CalculateStocks("SPROD");
            Assert.AreEqual(5, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void ReadStocksReturnsStocksList()
        {
            var allocRepo = SetupAllocationRepository();
            var warehouse = PrepareWarehouse(allocRepo);

            var stocks = warehouse.ReadStocks();

            Assert.AreEqual(0, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void ReadStocksFromCachedStocks()
        {
            var allocRepo = SetupAllocationRepository();
            var stockCacheMock = new Mock<IStockCache>();
            var warehouse = PrepareWarehouse(allocRepo, stockCacheMock.Object);

            warehouse.ReadStocks();

            stockCacheMock.Verify(m=>m.Read(), Times.Once());
        }

        [Test]
        [TestCase("PROD1", 2, 7)]
        [TestCase("PROD2", 1, 1)]
        public void ReadStocksByProductCode(string productCode, int count, decimal quantity)
        {
            var allocRepo = SetupAllocationRepository();
            var cache = new Mock<IStockCache>();
            cache.Setup(m=>m.Read())
                .Returns(new List<Stock>{
                    new Stock{ProductCode = "PROD1", Quantity = 4},
                    new Stock{ProductCode = "PROD1", Quantity = 3},
                    new Stock{ProductCode = "PROD2", Quantity = 1}
                });

            var warehouse = PrepareWarehouse(allocRepo, cache.Object);

            var stocks = warehouse.ReadStocksByProductCode(productCode);

            Assert.AreEqual(count, stocks.Count);
            Assert.AreEqual(quantity, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        [TestCase("LOC-001-01", 2, 7)]
        [TestCase("LOC-001-02", 1, 1)]
        public void ReadStocksByLocation(string location, int count, decimal quantity)
        {
            var allocRepo = SetupAllocationRepository();
            var cache = new Mock<IStockCache>();
            cache.Setup(m=>m.Read())
                .Returns(new List<Stock>{
                    new Stock{Location = "LOC-001-01", Quantity = 4},
                    new Stock{Location = "LOC-001-01", Quantity = 3},
                    new Stock{Location = "LOC-001-02", Quantity = 1}
                });

            var warehouse = PrepareWarehouse(allocRepo, cache.Object);

            var stocks = warehouse.ReadStocksByLocation(location);

            Assert.AreEqual(count, stocks.Count);
            Assert.AreEqual(quantity, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void AfterProcessDeliveryOperationRefreshStockCache()
        {
            var stockCacheMock = new Mock<IStockCache>();
            var allocRepo = SetupAllocationRepository();
            var operation = PrepareDeliveryOperation(allocRepo);
            var warehouse = PrepareWarehouse(allocRepo, stockCacheMock.Object);

            warehouse.ProcessOperation(operation);

            stockCacheMock.Verify(m=>m.Refresh(warehouse), Times.Once());
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
                .Returns(items);

            return mock.Object;
        }

        protected Warehouse PrepareWarehouse(IRepository<Allocation> allocRepo)
        {
            var stockCacheMock = new Mock<IStockCache>();
            stockCacheMock.Setup(m=>m.Read()).Returns(new List<Stock>());
            return PrepareWarehouse(allocRepo, stockCacheMock.Object);
        }

        protected Warehouse PrepareWarehouse(IRepository<Allocation> allocRepo, IStockCache cache)
        {
            return new Warehouse(allocRepo, cache);
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