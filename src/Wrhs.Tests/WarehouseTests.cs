using System;
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
        public void CanProcessDeliveryOperation()
        {
            var operation = PrepareDeliveryOperation();
            var warehouse = PrepareWarehouse();

            warehouse.ProcessOperation(operation);
        }

        public void WarehouseCalculateStocks()
        {
            var warehouse = PrepareWarehouse();

            var stocks = warehouse.CalculateStocks();

            CollectionAssert.IsEmpty(stocks);
        }

        protected Warehouse PrepareWarehouse()
        {
            return new Warehouse();
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

        protected DeliveryOperation PrepareDeliveryOperation()
        {
            var allocSrvMock = new Mock<IAllocationService>();
            var operation = new DeliveryOperation(allocSrvMock.Object);
            var order = PrepareOrder();
            var warehouse = new Warehouse();
            operation.SetBaseDocument(order);
            operation.AllocateItem((OrderLine)order.Lines[0], 5, "LOC-001-01");

            return operation;
        }
    }
}