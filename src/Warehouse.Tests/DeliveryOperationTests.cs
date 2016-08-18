using NUnit.Framework;
using Warehouse.Operations.Delivery;
using Warehouse.Orders;

namespace Warehouse.Tests
{
    [TestFixture]
    public class DeliveryOperationTests
    {
        [Test]
        public void PerformOperationReturnOperationResult()
        {
            var operation = new DeliveryOperation();
            
            var result = operation.Perform();

            Assert.IsNotNull(result);
        }

        [Test]
        public void OperationCanBasedOnOrder()
        {
            var order = new Order();
            var operation = new DeliveryOperation();

            operation.SetBaseDocument(order);

            Assert.AreEqual(order, operation.BaseDocument);
        }

        [Test]
        public void OperationCanBasedOnDeliveryDocument()
        {
            var document = new DeliveryDocument();
            var operation = new DeliveryOperation();

            operation.SetBaseDocument(document);

            Assert.AreEqual(document, operation.BaseDocument);
        }
    }
}