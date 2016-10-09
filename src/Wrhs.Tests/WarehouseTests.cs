using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Operations.Relocation;
using Wrhs.Operations.Release;
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
            var warehouse = PrepareWarehouse(PrepareAllocService(SetupAllocationRepository(items)));

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
            items.Add(new Allocation{Product = new Product{ Code="SPROD" }, Quantity=5, Location="LOC-001-01"});
            var warehouse = PrepareWarehouse(PrepareAllocService(SetupAllocationRepository(items)));

            var stocks = warehouse.CalculateStocks("SPROD");

            Assert.AreEqual(5, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void WarehouseConsumeIOperation()
        {
            var mock = new Mock<IOperation>();
            var items = PrepareAllocations(0);
            var allocService = PrepareAllocService(SetupAllocationRepository(items));
            var warehouse = PrepareWarehouse(allocService);

            warehouse.ProcessOperation(mock.Object);
            mock.Verify(m=>m.Perform(allocService), Times.Once());
        }

        [Test]
        public void ProcessDeliveryOperationChangesStocks()
        {
            var items = PrepareAllocations(0);
            var allocService = PrepareAllocService(SetupAllocationRepository(items));
            var operation = PrepareDeliveryOperation();
            var warehouse = PrepareWarehouse(allocService);
            
            var stocksBefore = warehouse.CalculateStocks("SPROD");
            Assert.AreEqual(0, stocksBefore.Sum(item=>item.Quantity));

            warehouse.ProcessOperation(operation);

            var stocks = warehouse.CalculateStocks("SPROD");
            Assert.AreEqual(5, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void ProcessRelocationOperationNotChangesStocks()
        {
            var items = PrepareAllocations(2);
            var allocService = PrepareAllocService(SetupAllocationRepository(items));
            var operation = PrepareRelocationOperation();
            var warehouse = PrepareWarehouse(allocService);

            var stocksBefore = warehouse.CalculateStocks("PROD1");
            Assert.AreEqual(2, stocksBefore.Sum(item=>item.Quantity));

            warehouse.ProcessOperation(operation);

            var stocks = warehouse.CalculateStocks("PROD1");
            Assert.AreEqual(2, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void ProcessRelocationOperationChangeLocation()
        {
            var items = PrepareAllocations(2);
            var allocService = PrepareAllocService(SetupAllocationRepository(items));
            var operation = PrepareRelocationOperation();
            var warehouse = PrepareWarehouse(allocService);

            var stockBefore = warehouse.CalculateStocks("PROD1");
            Assert.AreEqual(2, 
                stockBefore.Where(item=>item.Location.Equals("LOC-001-01")).Sum(item=>item.Quantity));
            Assert.AreEqual(0, 
                stockBefore.Where(item=>item.Location.Equals("LOC-001-02")).Sum(item=>item.Quantity));

            warehouse.ProcessOperation(operation);

            var stockAfter = warehouse.CalculateStocks("PROD1");
            Assert.AreEqual(0, 
                stockAfter.Where(item=>item.Location.Equals("LOC-001-01")).Sum(item=>item.Quantity));
            Assert.AreEqual(2, 
                stockAfter.Where(item=>item.Location.Equals("LOC-001-02")).Sum(item=>item.Quantity));
        }

        [Test]
        public void ProcessReleaseOperationChangesStocks()
        {
            var items = PrepareAllocations(2);
            var allocService = PrepareAllocService(SetupAllocationRepository(items));
            var operation = PrepareReleaseOperation();
            var warehouse = PrepareWarehouse(allocService);

            var stockBefore = warehouse.CalculateStocks("PROD1");
            Assert.AreEqual(2, stockBefore.Sum(item=>item.Quantity));

            warehouse.ProcessOperation(operation);

            var stockAfter = warehouse.CalculateStocks("PROD1");
            Assert.AreEqual(1, stockAfter.Sum(item=>item.Quantity));
        }

        [Test]
        public void ReadStocksReturnsStocksList()
        {
            var allocService = PrepareAllocService(SetupAllocationRepository());
            var warehouse = PrepareWarehouse(allocService);

            var stocks = warehouse.ReadStocks();

            Assert.AreEqual(0, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void ReadStocksFromCachedStocks()
        {
            var allocService = PrepareAllocService(SetupAllocationRepository());
            var stockCacheMock = new Mock<IStockCache>();
            var warehouse = PrepareWarehouse(allocService, stockCacheMock.Object);

            warehouse.ReadStocks();

            stockCacheMock.Verify(m=>m.Read(), Times.Once());
        }

        [Test]
        [TestCase("PROD1", 2, 7)]
        [TestCase("PROD2", 1, 1)]
        public void ReadStocksByProductCode(string productCode, int count, decimal quantity)
        {
            var allocService = PrepareAllocService(SetupAllocationRepository());
            var cache = new Mock<IStockCache>();
            cache.Setup(m=>m.Read())
                .Returns(new List<Stock>{
                    new Stock{Product = new Product{ Code = "PROD1" }, Quantity = 4},
                    new Stock{Product = new Product{ Code = "PROD1" }, Quantity = 3},
                    new Stock{Product = new Product{ Code = "PROD2" }, Quantity = 1}
                });

            var warehouse = PrepareWarehouse(allocService, cache.Object);

            var stocks = warehouse.ReadStocksByProductCode(productCode);

            Assert.AreEqual(count, stocks.Count);
            Assert.AreEqual(quantity, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        [TestCase("LOC-001-01", 2, 7)]
        [TestCase("LOC-001-02", 1, 1)]
        public void ReadStocksByLocation(string location, int count, decimal quantity)
        {
            var allocService = PrepareAllocService(SetupAllocationRepository());
            var cache = new Mock<IStockCache>();
            cache.Setup(m=>m.Read())
                .Returns(new List<Stock>{
                    new Stock{Location = "LOC-001-01", Quantity = 4},
                    new Stock{Location = "LOC-001-01", Quantity = 3},
                    new Stock{Location = "LOC-001-02", Quantity = 1}
                });

            var warehouse = PrepareWarehouse(allocService, cache.Object);

            var stocks = warehouse.ReadStocksByLocation(location);

            Assert.AreEqual(count, stocks.Count);
            Assert.AreEqual(quantity, stocks.Sum(item=>item.Quantity));
        }

        [Test]
        public void AfterProcessDeliveryOperationRefreshStockCache()
        {
            var cache = new List<Stock>();
            var stockCacheMock = new Mock<IStockCache>();
            
            stockCacheMock.Setup(m=>m.Read())
                .Returns(cache);
               
            stockCacheMock.Setup(m=>m.Refresh(It.IsAny<Warehouse>()))
                .Callback((Warehouse w)=>{
                    cache.Clear();
                    foreach(var s in w.CalculateStocks())
                        cache.Add(s);
                });    
                
            var allocService = PrepareAllocService(SetupAllocationRepository());
            var operation = PrepareDeliveryOperation();
            var warehouse = PrepareWarehouse(allocService, stockCacheMock.Object);

            warehouse.ProcessOperation(operation);

            stockCacheMock.Verify(m=>m.Refresh(warehouse), Times.Once());
            Assert.AreEqual(warehouse.CalculateStocks().Count, warehouse.ReadStocks().Count);
            CollectionAssert.AreEquivalent(warehouse.CalculateStocks(), warehouse.ReadStocks());
        }
        
        protected IAllocationService PrepareAllocService(IRepository<Allocation> allocRepo)
        {
            var mock = new Mock<IAllocationService>();
            
            mock.Setup(m=>m.RegisterAllocation(It.IsAny<Allocation>()))
                .Callback((Allocation allocation)=>{
                    allocRepo.Save(allocation);
                });

            mock.Setup(m=>m.RegisterDeallocation(It.IsAny<Allocation>()))
                .Callback((Allocation allocation)=>{
                    allocRepo.Save(allocation);
                });
                
            mock.Setup(m=>m.GetAllocations())
                .Returns(allocRepo.Get());
                
            
            return mock.Object;
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
                Product = new Product{ Code = "PROD1" },
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

        protected Warehouse PrepareWarehouse(IAllocationService allocService)
        {
            var stockCacheMock = new Mock<IStockCache>();
            stockCacheMock.Setup(m=>m.Read()).Returns(new List<Stock>());
            return PrepareWarehouse(allocService, stockCacheMock.Object);
        }

        protected Warehouse PrepareWarehouse(IAllocationService allocService, IStockCache cache)
        {
            return new Warehouse(allocService, cache);
        }

        protected Order PrepareOrder()
        {
            var order = new Order();
            order.Lines.Add(new OrderLine
            {
                Product = new Product{ Name = "Some product", Code = "SPROD" },
                EAN = "1234567890",
                SKU = "P12345",
                Quantity = 5,
                Remarks = ""
            });

            return order;
        }

        protected DeliveryOperation PrepareDeliveryOperation()
        {
            var operation = new DeliveryOperation();
            var order = PrepareOrder();
            operation.SetBaseDocument(order);
            operation.AllocateItem((OrderLine)order.Lines[0], 5, "LOC-001-01");

            return operation;
        }

        protected RelocationOperation PrepareRelocationOperation()
        {
            var operation = new RelocationOperation();
            var document = MakeRelocationDocument();
            operation.SetBaseDocument(document);
            operation.RelocateItem(document.Lines[0].Product, "LOC-001-01", "LOC-001-02", 2);

            return operation;
        }

        protected RelocationDocument MakeRelocationDocument()
        {
            var product = new Product()
            {
                Name =  "Product 1",
                Code = "PROD1",
                EAN = "123456789012",
                SKU = "P1234"
            };

            var docLine = new RelocationDocumentLine()
            {
                Product = product,
                Quantity = 2,
                From = "LOC-001-01",
                To = "LOC-001-02"
            };

            var doc = new RelocationDocument();
            doc.Lines.Add(docLine);

            return doc;
        }

        protected ReleaseOperation PrepareReleaseOperation()
        {
            var operation = new ReleaseOperation();
            var document = MakeReleaseDocument();
            operation.SetBaseDocument(document);
            operation.ReleaseItem(document.Lines[0].Product, "LOC-001-01", 1);
            
            return operation;
        }

        protected ReleaseDocument MakeReleaseDocument()
        {
            var document = new ReleaseDocument();

            document.Lines.Add(new ReleaseDocumentLine
            {
                Product = MakeProduct(),
                Quantity = 1,
                Location = "LOC-001-01"
            });

            return document;
        }

        protected Product MakeProduct(string code="PROD1", string name="Product 1", string ean = "111111111111")
        {
            var product = new Product
            {
                Code = code,
                Name = name,
                EAN = ean
            };

            return product;
        }

        
    }
}